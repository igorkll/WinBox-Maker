﻿using DiscUtils.Raw;
using DiscUtils.Udf;
using DiscUtils.Vfs;
using ManagedWimLib;
using Microsoft.VisualBasic.ApplicationServices;
using Shell32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using WinBox_Maker.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using IWshShortcut = IWshRuntimeLibrary.IWshShortcut;
using WshShell = IWshRuntimeLibrary.WshShell;

namespace WinBox_Maker
{
    public class WinBoxProject
    {
        const string resourcesDirectoryName = "winbox_resources";
        const string imagesDirectoryName = "winbox_images";
        public WinBoxConfig winBoxConfig;
        string wnbFilePath;
        public string baseDirectoryPath;
        public string buildDirectoryPath;
        public string resourcesDirectoryPath;
        public string imagesDirectoryPath;
        string tempDirectoryPath;
        string unpackedWimFile;
        string newWimFile;
        string wimMountPath;
        string unpackIsoPath;
        string name;
        string? err;

        public WinBoxProject(string wnbFilePath)
        {
            winBoxConfig = new WinBoxConfig();
            this.wnbFilePath = wnbFilePath;
            baseDirectoryPath = Path.GetDirectoryName(wnbFilePath) ?? "";
            buildDirectoryPath = Path.Combine(baseDirectoryPath, "winbox_build");
            resourcesDirectoryPath = Path.Combine(baseDirectoryPath, resourcesDirectoryName);
            imagesDirectoryPath = Path.Combine(baseDirectoryPath, imagesDirectoryName);
            tempDirectoryPath = Path.Combine(baseDirectoryPath, "winbox_temp");
            unpackedWimFile = Path.Combine(tempDirectoryPath, "base_install.wim");
            newWimFile = Path.Combine(tempDirectoryPath, "new_install.wim");
            wimMountPath = Path.Combine(tempDirectoryPath, "wim_mount");
            unpackIsoPath = Path.Combine(tempDirectoryPath, "iso_unpack");
            name = Path.GetFileName(baseDirectoryPath);

            if (File.Exists(wnbFilePath))
            {
                WinBoxConfig? config = WinBoxConfig.Load(wnbFilePath);
                if (config == null)
                {
                    err = "failed to load .wnb config";
                    return;
                }
                winBoxConfig = config;
            }
            else
            {
                winBoxConfig.Save(wnbFilePath);
            }

            Program.Execute("reg.exe", $"unload HKLM\\WINBOX_SOFTWARE");

            if (Directory.Exists(wimMountPath))
            {
                Process process = new Process();
                process.StartInfo.FileName = "dism.exe";
                process.StartInfo.Arguments = $"/Unmount-Wim /MountDir:\"{wimMountPath}\" /discard";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                try
                {
                    process.Start();
                    process.WaitForExit();
                }
                catch (Exception ex) {}

                try
                {
                    Directory.Delete(wimMountPath, true);
                }
                catch (Exception ex) {}
            }

            if (Directory.Exists(unpackIsoPath))
            {
                try
                {
                    Directory.Delete(unpackIsoPath, true);
                }
                catch (Exception ex) { }
            }

            Program.CreateDirectory(buildDirectoryPath);
            Program.CreateDirectory(resourcesDirectoryPath);
            Program.CreateDirectory(imagesDirectoryPath);
            Program.CreateDirectory(tempDirectoryPath);
            Program.CreateDirectory(wimMountPath);
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "files"));
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "program"));
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "drivers"));
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "packages"));

            string gitignorePath = Path.Combine(baseDirectoryPath, ".gitignore");
            if (!File.Exists(gitignorePath)) {
                File.WriteAllText(gitignorePath, $"## WinBox-Maker\n\nwinbox_build\nwinbox_temp\nwinbox_images\n");
            }
        }

        public string? GetName()
        {
            return name;
        }

        public string? GetError()
        {
            return err;
        }

        public void SaveConfig()
        {
            winBoxConfig.Save(wnbFilePath);
        }

        public async Task<string?> SelectResourceAsync(Action<string> processName, Action<int> processValue, string filter, string defaultDirectory, bool onlyDefaultDirectory)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = defaultDirectory;
                openFileDialog.Filter = filter;
                openFileDialog.Title = "Select Resource";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    string fileName = Path.GetFileName(filePath);

                    if (onlyDefaultDirectory)
                    {
                        if (Program.IsPathInsideDirectory(filePath, defaultDirectory))
                        {
                            return Path.GetRelativePath(defaultDirectory, filePath);
                        }
                        else
                        {
                            MessageBox.Show($"you can select a file only from the directory: {defaultDirectory}", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return null;
                        }
                    }

                    if (Program.IsPathInsideDirectory(filePath, defaultDirectory))
                    {
                        return Path.Combine(defaultDirectory, fileName);
                    }

                    DialogResult result = MessageBox.Show("the file is not located in the project's directory, if you use it like this, then the project config will have the absolute path to the file, which will make it impossible to build on another computer. do you want to copy the file so that you don't have to use an absolute path?", "copy the file?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        processName("Copying a resource file");
                        string projectFolderToCopy = Path.GetRelativePath(baseDirectoryPath, defaultDirectory);
                        await Program.CopyFileAsync(filePath, Path.Combine(baseDirectoryPath, projectFolderToCopy, fileName), processValue);
                        return Path.Combine(projectFolderToCopy, fileName);
                    }
                    else if (result == DialogResult.No)
                    {
                        return filePath;
                    }
                }
            }

            return null;
        }

        public string GetAbsoluteResourcePath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }
            else
            {
                return Path.Combine(baseDirectoryPath, path);
            }
        }

        public bool NeedLoadWindows()
        {
            return winBoxConfig.BaseWindowsImage != null && !File.Exists(unpackedWimFile);
        }

        public async Task LoadWindowsImageAsync(Action<string> processName, Action<int> processValue)
        {
            if (winBoxConfig.BaseWindowsImage == null) return;
            string baseWindowsImageFullPath = GetAbsoluteResourcePath(winBoxConfig.BaseWindowsImage);

            processName("Extracting install.wim");
            using (FileStream isoStream = File.Open(baseWindowsImageFullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                UdfReader cd = new UdfReader(isoStream);
                using (var wimFile = cd.OpenFile(@"sources\install.wim", FileMode.Open, FileAccess.Read))
                {
                    long totalBytes = wimFile.Length;
                    long bytesCopied = 0;

                    using (FileStream outputStream = new FileStream(unpackedWimFile, FileMode.Create, FileAccess.Write))
                    {
                        byte[] buffer = new byte[81920];
                        int bytesRead;

                        while ((bytesRead = await wimFile.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await outputStream.WriteAsync(buffer, 0, bytesRead);
                            bytesCopied += bytesRead;

                            processValue((int)((bytesCopied * 100) / totalBytes));
                        }
                    }
                }
            }
        }

        public void UnloadWindowsImage()
        {
            if (File.Exists(unpackedWimFile)) {
                File.Delete(unpackedWimFile);
            }
        }

        public WindowsDescription[] GetWindowsDescriptions()
        {
            List<WindowsDescription> windowsVersions = new List<WindowsDescription>();

            if (File.Exists(unpackedWimFile))
            {
                using (Wim wimHandle = Wim.OpenWim(unpackedWimFile, OpenFlags.None))
                {
                    WimInfo wimInfo = wimHandle.GetWimInfo();
                    for (int i = 1; i <= wimInfo.ImageCount; i++)
                    {
                        //Console.WriteLine($"Image Index: {i}");
                        //Console.WriteLine($"Name: {wimHandle.GetImageName(i)}");
                        //Console.WriteLine($"Description: {wimHandle.GetImageDescription(i)}");
                        WindowsDescription windowVersion = new WindowsDescription
                        {
                            name = wimHandle.GetImageName(i) ?? "failed to read windows name",
                            description = wimHandle.GetImageDescription(i) ?? "failed to read windows description"
                        };
                        windowsVersions.Add(windowVersion);
                    }
                }
            }

            return windowsVersions.ToArray();
        }

        static string ReplaceAndPrependBackslash(string input)
        {
            string modified = input.Replace('/', '\\');

            if (!modified.StartsWith("\\"))
            {
                modified = "\\" + modified;
            }

            return modified;
        }

        private async Task RegMod(string baseTree, string path, string key, string value)
        {
            path = ReplaceAndPrependBackslash(path);
            string tempRegPath = Path.Combine(tempDirectoryPath, "temp.reg");
            string regMod = $@"Windows Registry Editor Version 5.00

[HKEY_LOCAL_MACHINE\WINBOX_{baseTree}{path}]
""{key}""={value}
";
            await File.WriteAllTextAsync(tempRegPath, regMod);
            await Program.ExecuteAsync("reg.exe", $"import \"{tempRegPath}\"");
            File.Delete(tempRegPath);
        }

        public async Task WriteHiddenBatExecuter(string ExecuterPath, string batPath, string? args)
        {
            string vbsFile = $@"Set WshShell = CreateObject(""WScript.Shell"")
WshShell.Run """"""{batPath}"""" {args ?? ""}"", 0, False";
            await File.WriteAllTextAsync(ExecuterPath, vbsFile);
            
        }

        public async Task CopyResource(string name)
        {
            await Program.CopyFileAsync(Program.ResourcePath(Path.Combine("resources", name)), Path.Combine(wimMountPath, "WinboxResources", name));
        }

        public async Task CopyBlob(string name)
        {
            string? path = Program.getBlobPath(winBoxConfig, name);
            if (path != null)
            {
                await Program.CopyFileAsync(path, Path.Combine(wimMountPath, "WinboxResources", name));
            }
        }

        public async Task MakeModWim(Action<string> processName, Action<int> processValue, WindowsDescription newWindowsDescription, string newWimPath, string? imgPartitionPath)
        {
            processName("Preparing of install.wim");
            processValue(20);
            await Task.Run(() =>
            {
                using (Wim wimHandle = Wim.OpenWim(unpackedWimFile, OpenFlags.None))
                {
                    WimInfo wimInfo = wimHandle.GetWimInfo();
                    for (int i = (int)wimInfo.ImageCount; i >= 1; i--)
                    {
                        if (wimHandle.GetImageName(i) != winBoxConfig.BaseWindowsVersion)
                        {
                            wimHandle.DeleteImage(i);
                        }
                    }
                    wimHandle.SetImageName(1, newWindowsDescription.name);
                    wimHandle.SetImageDescription(1, newWindowsDescription.description);
                    wimHandle.Write(newWimPath, 1, WriteFlags.None, Wim.DefaultThreads);
                }
            });

            // ------------------------------------ mounting system
            processName("Mounting install.wim");
            processValue(30);
            await Program.ExecuteAsync("dism.exe", $"/Mount-Wim /WimFile:\"{newWimPath}\" /index:1 /MountDir:\"{wimMountPath}\"");

            processName("Modification of the system files");
            processValue(50);
            await Program.ExecuteAsync("reg.exe", $"load HKLM\\WINBOX_SOFTWARE \"{Path.Combine(wimMountPath, "Windows\\System32\\config\\SOFTWARE")}\"");
            //await Program.ExecuteAsync("reg.exe", $"load HKLM\\WINBOX_SYSTEM \"{Path.Combine(wimMountPath, "Windows\\System32\\config\\SYSTEM")}\"");

            // ------------------------------------ tweaks
            string WindowsScriptsPath = Path.Combine(wimMountPath, "Windows\\Setup\\Scripts");
            string WinboxResourcesPath = Path.Combine(wimMountPath, "WinboxResources");
            Directory.CreateDirectory(WindowsScriptsPath);
            Directory.CreateDirectory(WinboxResourcesPath);

            await Program.ExecuteAsync("reg.exe", $"import reg\\tweak.reg");

            string lockScreenAppPath = Path.Combine(wimMountPath, "Windows\\SystemApps\\Microsoft.LockApp_cw5n1h2txyewy");
            if (Directory.Exists(lockScreenAppPath))
            {
                Directory.Delete(lockScreenAppPath, true);
            }

            // ------------------------------------ system init
            string baseSetup = $@"@echo off
reagentc.exe /disable
chkntfs /x *

bcdedit /set {{current}} bootstatuspolicy ignoreallfailures
bcdedit /set {{current}} recoveryenabled no
bcdedit /set {{bootmgr}} displaybootmenu no
bcdedit /set {{bootmgr}} timeout 0
bcdedit /set loadoptions DISABLE_INTEGRITY_CHECKS
bcdedit /set NOINTEGRITYCHECKS ON
bcdedit /set TESTSIGNING ON

powercfg -h off
powercfg -change -standby-timeout-ac 0
powercfg -change -standby-timeout-dc 0
powercfg -change -monitor-timeout-dc {winBoxConfig.ScreenTimeout}
powercfg -change -monitor-timeout-ac {winBoxConfig.ScreenTimeout}
powercfg -setacvalueindex SCHEME_CURRENT SUB_BUTTONS LIDSWITCH 0
powercfg -setdcvalueindex SCHEME_CURRENT SUB_BUTTONS LIDSWITCH 0
powercfg -s SCHEME_CURRENT

dism /online /enable-feature /all /featurename:Client-EmbeddedLogon
dism /online /enable-feature /all /featurename:Client-DeviceLockdown
dism /online /enable-feature /all /featurename:Client-KeyboardFilter

sc config wbengine start= disabled
sc config wuauserv start= disabled
sc config RemoteRegistry start= disabled
sc config WSearch start= disabled
sc config SysMain start= disabled
sc config WerSvc start= disabled
sc config shellhwdetection start= disabled
sc config SSDPSRV start= disabled
sc config TermService start= disabled
sc config lanmanserver start= disabled
sc config napagent start= disabled
net stop wbengine
net stop wuauserv
net stop RemoteRegistry
net stop WSearch
net stop SysMain
net stop WerSvc
net stop shellhwdetection
net stop SSDPSRV
net stop TermService
net stop lanmanserver
net stop napagent

reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Keyboard Layout"" /v ""Scancode Map"" /t REG_BINARY /d 000000000000000030000000000021e000006ce000006de0000011e000006be000003b0000004400000057000000580000006400000065000000660000006700000068000000690000006a0000003c0000006b0000006c0000006d0000006e0000006f0000003d0000003e0000003f0000004000000041000000420000004300000013e0000014e0000012e00000380000005be000005ee0000037e0000038e000005ce000005fe0000063e000006ae0000066e0000069e0000032e0000067e0000065e0000068e000000000 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"" /v AutoReboot /t REG_DWORD /d 1 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"" /v CrashDumpEnabled /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\HardwareEvents"" /v MaxSize /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\Application"" /v MaxSize /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\Security"" /v MaxSize /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\System"" /v MaxSize /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"" /v AutoChkTimeout /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power"" /v HibernateEnabledDefault /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile"" /v EnableFirewall /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile"" /v EnableFirewall /t REG_DWORD /d 0 /f

reg add ""HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\SessionData"" /v AllowLockScreen /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Embedded\EmbeddedLogon"" /v HideAutoLogonUI /t REG_DWORD /d 1 /f
reg add ""HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Embedded\EmbeddedLogon"" /v HideFirstLogonAnimation /t REG_DWORD /d 1 /f
reg add ""HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Embedded\EmbeddedLogon"" /v BrandingNeutral /t REG_DWORD /d 1 /f

reg load HKLM\DEFAULT_USER ""C:\Users\Default\NTUSER.DAT""
reg add ""HKEY_LOCAL_MACHINE\DEFAULT_USER\Control Panel\Accessibility\StickyKeys"" /v Flags /t REG_DWORD /d 506 /f
reg add ""HKEY_LOCAL_MACHINE\DEFAULT_USER\Control Panel\Sound"" /v Beep /t REG_SZ /d no /f
reg add ""HKEY_LOCAL_MACHINE\DEFAULT_USER\Control Panel\Sound"" /v ExtendedSounds /t REG_SZ /d no /f
reg add ""HKEY_LOCAL_MACHINE\DEFAULT_USER\SOFTWARE\Microsoft\Windows\DWM"" /v AccentColor /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\DEFAULT_USER\SOFTWARE\Microsoft\Windows\DWM"" /v ColorizationColor /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\DEFAULT_USER\Software\Microsoft\Windows\Windows Error Reporting"" /v DontShowUI /t REG_DWORD /d 1 /f
reg add ""HKEY_LOCAL_MACHINE\DEFAULT_USER\Software\Microsoft\Windows\Windows Error Reporting"" /v Disabled /t REG_DWORD /d 1 /f
reg unload HKLM\DEFAULT_USER

net user winbox /add
net localgroup Administrators winbox /add";

            if (winBoxConfig.UseOemKey == true && winBoxConfig.OemKey != null && !winBoxConfig.OemKey.Contains("\""))
            {
                baseSetup += $"\r\ncscript /B \"%windir%\\system32\\slmgr.vbs\" /ipk \"{winBoxConfig.OemKey}\"\ncscript /B \"%windir%\\system32\\slmgr.vbs\" /ato";
            }

            if (Program.isTweakEnabled(winBoxConfig, "Integrate microsoft edge") || winBoxConfig.ProgramType == ProgramTypeEnum.WebSite)
            {
                await CopyBlob("MicrosoftEdge.msi");
                baseSetup += $"\r\nstart /wait msiexec /i \"C:\\WinboxResources\\MicrosoftEdge.msi\" /quiet /norestart";
            }

            if (Program.isTweakEnabled(winBoxConfig, "Hide Cursor"))
            {
                await CopyResource("empty.cur");
                await CopyResource("hide_cursor.reg");
                baseSetup += $"\r\nregedit /s \"C:\\WinboxResources\\hide_cursor.reg\"";
            }

            if (winBoxConfig.PostInstall_reg != null && !winBoxConfig.PostInstall_reg.Contains("\""))
            {
                string regPath = Path.Combine(resourcesDirectoryPath, winBoxConfig.PostInstall_reg);
                if (File.Exists(regPath))
                {
                    await Program.CopyFileAsync(regPath, Path.Combine(WinboxResourcesPath, "postinstall.reg"));
                    baseSetup += $"\r\nregedit /s \"C:\\WinboxResources\\postinstall.reg\"";
                }
            }

            if (winBoxConfig.PostInstall_bat != null && !winBoxConfig.PostInstall_bat.Contains("\""))
            {
                string batPath = Path.Combine(resourcesDirectoryPath, winBoxConfig.PostInstall_bat);
                if (File.Exists(batPath))
                {
                    await Program.CopyFileAsync(batPath, Path.Combine(WinboxResourcesPath, "postinstall.bat"));
                    baseSetup += $"\r\ncall \"C:\\WinboxResources\\postinstall.bat\"";
                }
            }

            if (winBoxConfig.CustomBootLogo != null && !winBoxConfig.CustomBootLogo.Contains("\""))
            {
                string logoPath = Path.Combine(resourcesDirectoryPath, winBoxConfig.CustomBootLogo);
                if (File.Exists(logoPath))
                {
                    await CopyBlob("HackBGRT.zip");
                    await Program.CopyFileAsync(logoPath, Path.Combine(WinboxResourcesPath, "logo.bmp"));
                    baseSetup += "\r\npowershell \"Expand-Archive -Path C:\\WinboxResources\\HackBGRT.zip -DestinationPath C:\\WinboxResources\"";
                    baseSetup += "\r\ncopy /Y C:\\WinboxResources\\logo.bmp C:\\WinboxResources\\HackBGRT-2.5.2\\splash.bmp";
                    baseSetup += "\r\nC:\\WinboxResources\\HackBGRT-2.5.2\\setup.exe batch install";
                }
            }

            await File.WriteAllTextAsync(Path.Combine(tempDirectoryPath, "debug_SetupComplete.cmd"), baseSetup);
            await File.WriteAllTextAsync(Path.Combine(WindowsScriptsPath, "SetupComplete.cmd"), baseSetup);

            // ------------------------------------ copy files
            string filesPath = Path.Combine(resourcesDirectoryPath, "files");
            if (Directory.Exists(filesPath))
            {
                await Program.CopyFilesRecursivelyAsync(filesPath, wimMountPath);
            }

            // ------------------------------------ copy program files
            string programPath = Path.Combine(resourcesDirectoryPath, "program");
            if (Directory.Exists(filesPath))
            {
                await Program.CopyFilesRecursivelyAsync(programPath, Path.Combine(wimMountPath, "WinboxProgram"));
            }

            // ------------------------------------ setup application autorun
            string command = "";
            switch (winBoxConfig.ProgramType)
            {
                case ProgramTypeEnum.ExecutableFile:
                    {
                        string execFilePath = @$"C:\WinboxProgram\{winBoxConfig.ProgramName}";
                        string extension = Path.GetExtension(winBoxConfig.ProgramName);
                        if (extension != null && (extension.Equals(".bat", StringComparison.OrdinalIgnoreCase) ||
                            extension.Equals(".cmd", StringComparison.OrdinalIgnoreCase)))
                        {
                            await WriteHiddenBatExecuter(Path.Combine(WinboxResourcesPath, "run_user_script_hidden.vbs"), execFilePath, winBoxConfig.ProgramArgs);
                            command = "wscript \"C:\\WinboxResources\\run_user_script_hidden.vbs\"";
                        }
                        else
                        {
                            command = "\"" + execFilePath + "\"";
                            if (winBoxConfig.ProgramArgs != null && winBoxConfig.ProgramArgs.Length > 0)
                            {
                                command += " " + winBoxConfig.ProgramArgs;
                            }
                        }
                    }
                    break;

                case ProgramTypeEnum.RawCommand:
                    if (winBoxConfig.RawCommand != null) {
                        command = winBoxConfig.RawCommand;
                    }
                    break;

                case ProgramTypeEnum.WebSite:
                    if (winBoxConfig.WebSite != null && !winBoxConfig.WebSite.Contains("\""))
                    {
                        string execFilePath = @"C:\WinboxResources\run_edge.bat";
                        string batFile = $@"@echo off

set ""edgePath1=C:\WinboxResources\edge\msedge.exe""
set ""edgePath1=C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe""
set ""edgePath2=C:\Program Files\Microsoft\Edge\Application\msedge.exe""

set ""msedgePath=""
if exist ""%edgePath1%"" (
    set ""msedgePath=%edgePath1%""
) else (
    if exist ""%edgePath2%"" (
        set ""msedgePath=%edgePath2%""
    ) else (
        if exist ""%edgePath3%"" (
            set ""msedgePath=%edgePath3%""
        )
    )
)

if ""%msedgePath%""=="""" (
    powershell -Command ""Add-Type -AssemblyName System.Windows.Forms; [System.Windows.Forms.MessageBox]::Show('Microsoft Edge not found. please check the winbox configuration','Winbox Broken', [System.Windows.Forms.MessageBoxButtons]::OK, [System.Windows.Forms.MessageBoxIcon]::Error)""
    exit /b
)

:restart
start """" ""%msedgePath%"" --kiosk ""{winBoxConfig.WebSite}"" --edge-kiosk-type=fullscreen --kiosk-idle-timeout-minutes={winBoxConfig.WebSessionTimeout} --no-first-run

:loop
timeout /t 1
tasklist | find /i ""msedge.exe"" >nul
if %errorlevel%==0 (
    goto loop
) else (
    goto restart
)";
                        await File.WriteAllTextAsync(Path.Combine(WinboxResourcesPath, "run_edge.bat"), batFile);
                        await WriteHiddenBatExecuter(Path.Combine(WinboxResourcesPath, "run_edge_script_hidden.vbs"), execFilePath, null);
                        command = "wscript \"C:\\WinboxResources\\run_edge_script_hidden.vbs\"";
                    }
                    break;
            }
            await RegMod("SOFTWARE", "Microsoft\\Windows NT\\CurrentVersion\\Winlogon", "Shell", Program.EscapeForRegFile(command));

            // ------------------------------------ save & export
            await Program.ExecuteAsync("reg.exe", $"unload HKLM\\WINBOX_SOFTWARE");
            //await Program.ExecuteAsync("reg.exe", $"unload HKLM\\WINBOX_SYSTEM");

            string driversPath = Path.Combine(resourcesDirectoryPath, "drivers");
            if (Program.IsDirectoryNotEmpty(driversPath)) {
                processName("Installing user drivers");
                processValue(55);
                await Program.ExecuteAsync("dism.exe", $"/image:\"{wimMountPath}\" /add-driver /driver:\"{driversPath}\" /recurse /forceunsigned");
            }

            string packagesPath = Path.Combine(resourcesDirectoryPath, "packages");
            if (Program.IsDirectoryNotEmpty(packagesPath))
            {
                processName("Installing .cab/.msu packages");
                processValue(58);
                await Program.ExecuteAsync("dism.exe", $"/image:\"{wimMountPath}\" /add-package /PackagePath:\"{packagesPath}\"");
            }

            processName("Enabling necessary windows components");
            processValue(60);
            await Program.ExecuteAsync("dism.exe", $"/image:\"{wimMountPath}\" /enable-feature /all /featurename:Client-EmbeddedLogon");
            await Program.ExecuteAsync("dism.exe", $"/image:\"{wimMountPath}\" /enable-feature /all /featurename:Client-DeviceLockdown");
            await Program.ExecuteAsync("dism.exe", $"/image:\"{wimMountPath}\" /enable-feature /all /featurename:Client-KeyboardFilter");

            if (imgPartitionPath != null)
            {
                processName("Generating an .img image of a partition");
                processValue(65);
                await Program.ExecuteAsync("dism.exe", $"/Capture-Image /ImageFile:\"{imgPartitionPath}\" /CaptureDir:\"{wimMountPath}\" /Name:\"{winBoxConfig.WinboxName}\"");

                processName("Unmounting install.wim");
                processValue(70);
                await Program.ExecuteAsync("dism.exe", $"/Unmount-Wim /MountDir:\"{wimMountPath}\" /discard");
            }
            else
            {
                processName("Unmounting and save install.wim");
                processValue(70);
                await Program.ExecuteAsync("dism.exe", $"/Unmount-Wim /MountDir:\"{wimMountPath}\" /commit");
            }
        }
        private async Task CompleteExportMsg(Action<string> processName, Action<int> processValue)
        {
            processName("Completed!");
            processValue(100);
            await Task.Delay(2000);
        }
 
        public async Task BuildIsoAsync(Action<string> processName, Action<int> processValue, string exportPath, WindowsDescription newWindowsDescription)
        {
            if (winBoxConfig.BaseWindowsImage == null || winBoxConfig.BaseWindowsVersion == null) return;

            string baseWindowsImageFullPath = GetAbsoluteResourcePath(winBoxConfig.BaseWindowsImage);

            processName("Unpacking the iso");
            string[] unpackBlacklist = { "sources\\install.wim" };
            await Program.UnpackUdfIso(baseWindowsImageFullPath, unpackIsoPath, processValue, unpackBlacklist);

            await MakeModWim(processName, processValue, newWindowsDescription, Path.Combine(unpackIsoPath, "sources\\install.wim"), null);

            processName("ISO modification");
            processValue(80);
            if (winBoxConfig.UseOemKey == true)
            {
                await File.WriteAllTextAsync(Path.Combine(unpackIsoPath, "Sources\\PID.txt"), $"[PID]\nValue={winBoxConfig.OemKey}");
            }

            processName("Building an ISO image");
            processValue(85);
            await Program.ExecuteAsync(Program.oscdimgPath, $"-m -u2 -b\"{Path.Combine(unpackIsoPath, "boot\\etfsboot.com")}\" \"{unpackIsoPath}\" \"{exportPath}\"");

            processName("Deleting unpacked ISO files");
            processValue(90);
            await Task.Run(() =>
            {
                Directory.Delete(unpackIsoPath, true);
            });

            await CompleteExportMsg(processName, processValue);
        }

        public async Task BuildWimAsync(Action<string> processName, Action<int> processValue, string exportPath, WindowsDescription newWindowsDescription)
        {
            if (winBoxConfig.BaseWindowsImage == null || winBoxConfig.BaseWindowsVersion == null) return;

            await MakeModWim(processName, processValue, newWindowsDescription, exportPath, null);

            await CompleteExportMsg(processName, processValue);
        }

        public async Task BuildImgPartitionAsync(Action<string> processName, Action<int> processValue, string exportPath, WindowsDescription newWindowsDescription)
        {
            if (winBoxConfig.BaseWindowsImage == null || winBoxConfig.BaseWindowsVersion == null) return;

            await MakeModWim(processName, processValue, newWindowsDescription, newWimFile, exportPath);

            processName("Deleting temp install.wim");
            processValue(90);
            File.Delete(newWimFile);

            await CompleteExportMsg(processName, processValue);
        }
    }
}
