using DiscUtils.Raw;
using DiscUtils.Udf;
using DiscUtils.Vfs;
using ManagedWimLib;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.Win32;
using Shell32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml.Linq;
using WinBox_Maker.Properties;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using IWshShortcut = IWshRuntimeLibrary.IWshShortcut;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using WshShell = IWshRuntimeLibrary.WshShell;

namespace WinBox_Maker
{
    public class WinBoxProject
    {
        const string resourcesDirectoryName = "winbox_resources";
        const string imagesDirectoryName = "winbox_images";
        public WinBoxConfig winBoxConfig;
        public string wnbFilePath;
        public string baseDirectoryPath;
        public string buildDirectoryPath;
        public string resourcesDirectoryPath;
        public string imagesDirectoryPath;
        public string sourcesDirectoryPath;
        public string debugBuildProgramsPath;
        public string tempDirectoryPath;
        public string imageTimeZonesInfo;
        public string imageKeyboardLayoutsInfo;
        public string unpackedWimFile;
        public string wimInfoFile;
        public string newWimFile;
        public string wimMountPath;
        public string wimWinPeMountPath;
        public string recoveryMountPath;
        public string unpackIsoPath;
        public string name;
        string? err;
        public string debugFolder;
        public string WinboxApiPath;

        public string[] imageInfoFiles;

        public WinBoxProject(string wnbFilePath)
        {
            winBoxConfig = new WinBoxConfig();
            Program.winBoxConfig = winBoxConfig;
            
            this.wnbFilePath = wnbFilePath;
            baseDirectoryPath = Path.GetDirectoryName(wnbFilePath) ?? "";
            buildDirectoryPath = Path.Combine(baseDirectoryPath, "winbox_build");
            resourcesDirectoryPath = Path.Combine(baseDirectoryPath, resourcesDirectoryName);
            imagesDirectoryPath = Path.Combine(baseDirectoryPath, imagesDirectoryName);
            tempDirectoryPath = Path.Combine(baseDirectoryPath, "winbox_temp");
            unpackedWimFile = Path.Combine(tempDirectoryPath, "base_install.wim");
            wimInfoFile = Path.Combine(tempDirectoryPath, "installWimInfo.json");
            imageTimeZonesInfo = Path.Combine(tempDirectoryPath, "timeZonesInfo.json");
            imageKeyboardLayoutsInfo = Path.Combine(tempDirectoryPath, "keyboardLayoutsInfo.json");
            newWimFile = Path.Combine(tempDirectoryPath, "new_install.wim");
            wimMountPath = Path.Combine(tempDirectoryPath, "wim_mount");
            wimWinPeMountPath = Path.Combine(tempDirectoryPath, "wim_boot_mount");
            recoveryMountPath = Path.Combine(tempDirectoryPath, "recovery_mount");
            unpackIsoPath = Path.Combine(tempDirectoryPath, "iso_unpack");
            sourcesDirectoryPath = Path.Combine(resourcesDirectoryPath, "sources");
            debugFolder = Path.Combine(tempDirectoryPath, "debug");
            debugBuildProgramsPath = Path.Combine(debugFolder, "program");
            WinboxApiPath = Path.Combine(wimMountPath, "WinboxApi");
            name = Path.GetFileName(baseDirectoryPath);

            imageInfoFiles = new string[] {
                wimInfoFile,
                imageTimeZonesInfo,
                imageKeyboardLayoutsInfo
            };

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

            if (winBoxConfig.winboxMakerVersion > Program.version_num)
            {
                err = $"this project was saved in winbox maker {winBoxConfig.winboxMakerVersionStr} and you have {Program.version_str} installed. update winbox maker to open this project";
                return;
            }

            Program.Execute("reg.exe", $"unload HKLM\\WINBOX_SOFTWARE");
            Program.Execute("reg.exe", $"unload HKLM\\WINBOX_WINPE_SOFTWARE");
            Program.Execute("reg.exe", $"unload HKLM\\WINBOX_WINPE_SYSTEM");

            bool umount(string path)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (Directory.Exists(path))
                    {
                        Process process = new Process();
                        process.StartInfo.FileName = "dism.exe";
                        process.StartInfo.Arguments = $"/Unmount-Wim /MountDir:\"{path}\" /discard";
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;

                        try
                        {
                            process.Start();
                            process.WaitForExit();
                        }
                        catch (Exception ex) { }

                        try
                        {
                            Directory.Delete(path, true);
                        }
                        catch (Exception ex) { }
                    }
                    else
                    {
                        break;
                    }
                }

                if (Directory.Exists(path))
                {
                    err = "the old Windows image could not be completely unmounted. restart your computer and try again. if this does not help, then delete the winbox_temp directory from the project";
                    return true;
                }

                return false;
            }

            if (umount(recoveryMountPath)) return;
            if (umount(wimMountPath)) return;
            if (umount(wimWinPeMountPath)) return;

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
            Program.CreateDirectory(wimWinPeMountPath);
            Program.CreateDirectory(recoveryMountPath);
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "files"));
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "boot_files"));
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "recovery_files"));
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "program"));
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "drivers"));
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "nvidia_drivers"));
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "amd_drivers"));
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "intel_drivers"));
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "driver_installers"));
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "packages"));
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "cursor"));
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "iso_files"));
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "vc_redist"));
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "net"));
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "net_framework"));
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "app_runtime"));
            Program.CreateDirectory(sourcesDirectoryPath);
            Program.CreateDirectory(debugFolder);

            string gitignorePath = Path.Combine(baseDirectoryPath, ".gitignore");
            if (!File.Exists(gitignorePath)) {
                File.WriteAllText(gitignorePath, $"## WinBox-Maker\n\nwinbox_build\nwinbox_temp\nwinbox_images\n");
            }

            foreach (BuildItem buildItem in winBoxConfig.BuildItems)
            {
                buildItem.initDefaults();
            }

            if (winBoxConfig.actionAtEndOfApplication == ActionAtEndOfApplication.invalid)
            {
                updateActionAtEndOfApplication();
            }
        }

        public void updateActionAtEndOfApplication()
        {
            switch (winBoxConfig.ProgramType)
            {
                case ProgramTypeEnum.ExecutableFile:
                    winBoxConfig.actionAtEndOfApplication = ActionAtEndOfApplication.restart_app;
                    break;

                case ProgramTypeEnum.RawCommand:
                    winBoxConfig.actionAtEndOfApplication = ActionAtEndOfApplication.none;
                    break;

                case ProgramTypeEnum.WebSite:
                    winBoxConfig.actionAtEndOfApplication = ActionAtEndOfApplication.restart_app;
                    break;

                default:
                    winBoxConfig.actionAtEndOfApplication = ActionAtEndOfApplication.none;
                    break;
            }
        }

        public void breakpointStop(string eventname, bool after)
        {
            MessageBox.Show($"{(after ? "after" : "before")} {eventname} event\n", "breakpoint", MessageBoxButtons.OK);
        }

        public string getDebugFilePath(string name)
        {
            return Path.Combine(debugFolder, name + ".txt");
        }

        public async Task writeDebugFile(string name, string content, bool addTxt=true)
        {
            Program.CreateDirectory(debugFolder);
            await File.WriteAllTextAsync(Path.Combine(debugFolder, name + (addTxt ? ".txt" : "")), content);
        }

        public async Task copyToDebugFile(string name, string sourcePath)
        {
            Program.CreateDirectory(debugFolder);
            await Program.CopyFileAsync(sourcePath, Path.Combine(debugFolder, name));
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

                    DialogResult result;
                    if (onlyDefaultDirectory)
                    {
                        if (Program.IsPathInsideDirectory(filePath, defaultDirectory))
                        {
                            return Path.GetRelativePath(defaultDirectory, filePath);
                        }

                        result = MessageBox.Show("the file is not in the project directory, it must be copied to the project in order to use it. do you want to copy the file?", "copy the file?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            processName("Copying a resource file");
                            await Program.CopyFileAsync(filePath, Path.Combine(defaultDirectory, fileName), processValue);
                            return fileName;
                        }

                        return null;
                    }

                    if (Program.IsPathInsideDirectory(filePath, defaultDirectory))
                    {
                        return Path.GetRelativePath(baseDirectoryPath, filePath);
                    }

                    result = MessageBox.Show("the file is not located in the project directory, if you use it like this, then the project config will have the absolute path to the file, which will make it impossible to build on another computer. do you want to copy the file so that you don't have to use an absolute path?", "copy the file?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
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

        public async Task<string?> SelectResourceFolderAsync(Action<string> processName, Action<int> processValue, string defaultDirectory)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.InitialDirectory = defaultDirectory;
                folderBrowserDialog.UseDescriptionForTitle = true;
                folderBrowserDialog.Description = "Select Folder";
                folderBrowserDialog.ShowNewFolderButton = true;

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    if (Program.IsPathInsideDirectory(folderBrowserDialog.SelectedPath, defaultDirectory))
                    {
                        return Path.GetRelativePath(defaultDirectory, folderBrowserDialog.SelectedPath);
                    }

                    MessageBox.Show("select folder from the default directory", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
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

        async Task<string?> getWindowsImagePath(Action<string>? processName = null, Action<int>? processValue=null)
        {
            if (winBoxConfig.BaseWindowsImage == null) return null;

            if (winBoxConfig.BaseWindowsImage.StartsWith("http://") || winBoxConfig.BaseWindowsImage.StartsWith("https://"))
            {
                string downloadPath = Path.Combine(Program.downloadImagesPath, Program.CalculateMD5(winBoxConfig.BaseWindowsImage) + ".iso");

                if (!Program.isFileDownloaded(downloadPath))
                {
                    processName("Downloading a windows image by URL");
                    await Program.downloadFile(winBoxConfig.BaseWindowsImage, downloadPath, processValue);
                }

                if (Program.isFileDownloaded(downloadPath))
                {
                    return downloadPath;
                }
            }
            else
            {
                string path = GetAbsoluteResourcePath(winBoxConfig.BaseWindowsImage);
                if (File.Exists(path))
                {
                    return path;
                }
            }

            return null;
        }

        public bool NeedLoadWindows()
        {
            return winBoxConfig.BaseWindowsImage != null && !Program.AllFileExists(imageInfoFiles);
        }

        public async Task<bool> ExtractInstallWim(Action<string> processName, Action<int> processValue)
        {
            string? baseWindowsImageFullPath = await getWindowsImagePath(processName, processValue);
            if (baseWindowsImageFullPath == null) return false;

            using (FileStream isoStream = File.Open(baseWindowsImageFullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                string wimPath = @"sources\install.wim";
                string esdPath = @"sources\install.esd";

                UdfReader cd = new UdfReader(isoStream);

                async Task unpackFile(string input, string output)
                {
                    using (var wimFile = cd.OpenFile(input, FileMode.Open, FileAccess.Read))
                    {
                        long totalBytes = wimFile.Length;
                        long bytesCopied = 0;

                        using (FileStream outputStream = new FileStream(output, FileMode.Create, FileAccess.Write))
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

                if (cd.Exists(wimPath))
                {
                    processName("Extracting install.wim");
                    await unpackFile(wimPath, unpackedWimFile);
                } else if (cd.Exists(esdPath))
                {
                    processName("Extracting install.esd");
                    string unpackEsdFile = Path.Combine(tempDirectoryPath, "base_install.esd");
                    await unpackFile(esdPath, unpackEsdFile);
                    processName("Converting install.esd to install.wim");
                    processValue(20);
                    await Program.ExecuteAsync("dism.exe", @$"/Export-Image /SourceImageFile:""{unpackEsdFile}"" /All /DestinationImageFile:""{unpackedWimFile}"" /Compress:max /CheckIntegrity", null, debugFolder);
                    if (File.Exists(unpackEsdFile))
                    {
                        processName("Deleting install.esd");
                        processValue(80);
                        await Task.Run(() =>
                        {
                            File.Delete(unpackEsdFile);
                        });
                    }
                }
            }

            return File.Exists(unpackedWimFile);
        }

        public async Task DeleteInstallWim(Action<string> processName)
        {
            if (File.Exists(unpackedWimFile))
            {
                processName("Deleting install.wim");
                await Task.Run(() =>
                {
                    File.Delete(unpackedWimFile);
                });
            }
        }

        async Task mountDism(string wimPath, string? mountPath = null, int index = 1)
        {
            if (mountPath == null) mountPath = wimMountPath;
            await Program.ExecuteAsync("dism.exe", $"/Mount-Wim /WimFile:\"{wimPath}\" /index:{index} /MountDir:\"{mountPath}\"", null, debugFolder);
        }

        async Task umountDism(bool commit, string? path = null)
        {
            if (path == null) path = wimMountPath;
            await Program.ExecuteAsync("dism.exe", $"/Unmount-Wim /MountDir:\"{path}\" {(commit ? "/commit" : "/discard")}", null, debugFolder);
        }

        public async Task LoadWindowsImageAsync(Action<string> processName, Action<int> processValue)
        {
            await ExtractInstallWim(processName, processValue);

            if (File.Exists(unpackedWimFile))
            {
                using (Wim wimHandle = Wim.OpenWim(unpackedWimFile, OpenFlags.None))
                {
                    WimInfo wimInfo = wimHandle.GetWimInfo();
                    List<WindowsDescription> windowsDescriptions = new List<WindowsDescription>();

                    for (int i = 1; i <= wimInfo.ImageCount; i++)
                    {
                        WindowsDescription windowVersion = new WindowsDescription
                        {
                            name = wimHandle.GetImageName(i) ?? "failed to read windows name",
                            description = wimHandle.GetImageDescription(i) ?? "failed to read windows description"
                        };

                        windowsDescriptions.Add(windowVersion);
                    }

                    string json = JsonSerializer.Serialize(windowsDescriptions, new JsonSerializerOptions { WriteIndented = true });
                    await File.WriteAllTextAsync(wimInfoFile, json);

                    // ---------------------------------------------------

                    processName("Extracting image data");
                    processValue(40);

                    List<string> timeZones = new List<string>();
                    List<TwoStrings> keyboardLayouts = new List<TwoStrings>();

                    await mountDism(unpackedWimFile);
                    await RegChanger.mountReg();
                    await RegChanger.mountReg("SYSTEM");

                    using (RegistryKey? root = Registry.LocalMachine.OpenSubKey($@"WINBOX_SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones"))
                    {
                        if (root != null)
                        {
                            foreach (string name in root.GetSubKeyNames())
                            {
                                timeZones.Add(name);
                            }
                        }
                    }

                    using (RegistryKey? root = Registry.LocalMachine.OpenSubKey($@"WINBOX_SYSTEM\ControlSet001\Control\Keyboard Layouts"))
                    {
                         if (root != null)
                        {
                            var tempList = new List<TwoStrings>();

                            foreach (string id in root.GetSubKeyNames())
                            {
                                using (RegistryKey? layoutKey = root.OpenSubKey(id))
                                {
                                    if (layoutKey == null) continue;
                                    TwoStrings twoStrings = new TwoStrings();
                                    twoStrings.string1 = (layoutKey.GetValue("Layout Text") as string) + $" ({id})";
                                    twoStrings.string2 = id;
                                    tempList.Add(twoStrings);
                                }
                            }

                            keyboardLayouts.AddRange(tempList.OrderByDescending(l => l.string2 == "00000409").ThenBy(l => l.string1));
                        }
                    }

                    if (winBoxConfig.keyboard_layouts_firstAdded != true)
                    {
                        winBoxConfig.keyboard_layouts.Add(keyboardLayouts[0]);
                        winBoxConfig.keyboard_layouts_firstAdded = true;
                    }

                    await RegChanger.umountReg();
                    await RegChanger.umountReg("SYSTEM");
                    await umountDism(false);

                    json = JsonSerializer.Serialize(timeZones, new JsonSerializerOptions { WriteIndented = true });
                    await File.WriteAllTextAsync(imageTimeZonesInfo, json);

                    json = JsonSerializer.Serialize(keyboardLayouts, new JsonSerializerOptions { WriteIndented = true });
                    await File.WriteAllTextAsync(imageKeyboardLayoutsInfo, json);
                }
            }

            processValue(60);
            await DeleteInstallWim(processName);
        }

        public void UnloadWindowsImage()
        {
            Program.DeleteFiles(imageInfoFiles);
        }

        public WindowsDescription[] GetWindowsDescriptions()
        {
            if (File.Exists(wimInfoFile))
            {
                string json = File.ReadAllText(wimInfoFile);

                List<WindowsDescription>? windowsVersions = JsonSerializer.Deserialize<List<WindowsDescription>>(json);
                if (windowsVersions != null)
                {
                    return windowsVersions.ToArray();
                }
            }

            return [];
        }

        public string[] GetWindowsTimeZones()
        {
            if (File.Exists(imageTimeZonesInfo))
            {
                string json = File.ReadAllText(imageTimeZonesInfo);

                List<string>? list = JsonSerializer.Deserialize<List<string>>(json);
                if (list != null)
                {
                    return list.ToArray();
                }
            }

            return [];
        }

        public TwoStrings[] GetWindowsKeyboardLayouts()
        {
            if (File.Exists(imageKeyboardLayoutsInfo))
            {
                string json = File.ReadAllText(imageKeyboardLayoutsInfo);

                List<TwoStrings>? list = JsonSerializer.Deserialize<List<TwoStrings>>(json);
                if (list != null)
                {
                    return list.ToArray();
                }
            }

            return [];
        }

        public string[] GetWindowsKeyboardLayoutNames()
        {
            if (File.Exists(imageKeyboardLayoutsInfo))
            {
                string json = File.ReadAllText(imageKeyboardLayoutsInfo);

                List<TwoStrings>? list = JsonSerializer.Deserialize<List<TwoStrings>>(json);
                if (list != null)
                {
                    return list.Select(x => x.string1 ?? string.Empty).ToArray();
                }
            }

            return Array.Empty<string>();
        }

        public async Task WriteHiddenBatExecuter(string ExecuterPath, string batPath, string? args)
        {
            string vbsFile = $@"Set WshShell = CreateObject(""WScript.Shell"")
WshShell.Run """"""{batPath}"""" {args ?? ""}"", 0, False";
            await File.WriteAllTextAsync(ExecuterPath, vbsFile);
            
        }

        public async Task WriteHiddenBatExecuterAdmin(string executerPath, string batPath, string? args)
        {
            string argsStr = "";
            if (args != null && args.Length > 0) argsStr = $@"-ArgumentList '{args.Replace("'", "''")}'";
            string vbsFile = $@"Set WshShell = CreateObject(""WScript.Shell"")
WshShell.Run ""powershell -Command """"Start-Process '{batPath}' {argsStr} -Verb RunAs -WindowStyle Hidden"""" "", 0, False";

            await File.WriteAllTextAsync(executerPath, vbsFile);
        }


        public async Task CopyResource(string name)
        {
            await Program.CopyFileAsync(Program.ResourcePath(Path.Combine("resources", name)), Path.Combine(wimMountPath, "WinboxResources", name));
        }

        public async Task CopyBlob(string name, string? subfolder = null)
        {
            string? path = Program.getBlobPath(winBoxConfig, name);
            if (path != null)
            {
                string pathToCopy;
                if (subfolder != null)
                {
                    pathToCopy = Path.Combine(wimMountPath, "WinboxResources", subfolder);
                    Program.CreateDirectory(pathToCopy);
                    pathToCopy = Path.Combine(pathToCopy, name);
                }
                else
                {
                    pathToCopy = Path.Combine(wimMountPath, "WinboxResources", name);
                }
                await Program.CopyFileAsync(path, pathToCopy);
            }
        }

        public async Task CopyBlobFromArchWithRename(string name, string newName, string arch)
        {
            string? path = Program.getBlobPathFromArch(winBoxConfig, name, arch);
            if (path != null)
            {
                await Program.CopyFileAsync(path, Path.Combine(wimMountPath, "WinboxResources", newName));
            }
        }

        public async Task UnpackBlob(string name, string? subfolder = null)
        {
            string? path = Program.getBlobPath(winBoxConfig, name);
            if (path != null)
            {
                await Task.Run(() =>
                {
                    try
                    {
                        string? outputPath;
                        if (subfolder != null)
                        {
                            outputPath = Path.Combine(wimMountPath, "WinboxResources", subfolder);
                        }
                        else
                        {
                            outputPath = Path.Combine(wimMountPath, "WinboxResources");
                        }
                        Program.CreateDirectory(outputPath);
                        ZipFile.ExtractToDirectory(path, outputPath);
                    }
                    catch (Exception ex)
                    {
                    }
                });
            }
        }

        private async Task RemoveTempFolder(string folder, bool recreate=false)
        {
            string tempPath = Path.Combine(tempDirectoryPath, folder);
            if (Directory.Exists(tempPath))
            {
                Directory.Delete(tempPath, true);
            }

            if (recreate)
            {
                Program.CreateDirectory(tempPath);
            }
        }

        private async Task RemoveTemp(Action<string> processName, bool recreate=false) {
            processName("Cleaning temporary files");
            await RemoveTempFolder("files");
            await RemoveTempFolder("program");
            await RemoveTempFolder("drivers");
            await RemoveTempFolder("nvidia_drivers");
            await RemoveTempFolder("amd_drivers");
            await RemoveTempFolder("intel_drivers");
            await RemoveTempFolder("driver_installers");
            await RemoveTempFolder("packages");
            await RemoveTempFolder("iso_files");
            await RemoveTempFolder("vc_redist");
            await RemoveTempFolder("net");
            await RemoveTempFolder("net_framework");
            await RemoveTempFolder("app_runtime");
            await RemoveTempFolder("boot_files");
            await RemoveTempFolder("recovery_files");
            if (recreate)
            {
                await RemoveTempFolder("usertemp", true);
            }
        }

        public async Task BuildCMakeProject(int index, BuildItem buildItem, string cmakeFolder, string output)
        {
            string configuration = buildItem.cmake_configuration;
            string architecture = winBoxConfig.Architecture;
            if (architecture == "arm64") architecture = "ARM64";
            if (architecture == "x86") architecture = "Win32";

            string buildDir = Path.Combine(tempDirectoryPath, "cmake_build");
            if (Directory.Exists(buildDir))
            {
                Directory.Delete(buildDir, true);
            }
            Directory.CreateDirectory(buildDir);

            await Program.ExecuteAsync(Program.winboxSettings.path_cmake, $"-A \"{architecture}\" -DCMAKE_BUILD_TYPE=\"{configuration}\" \"{cmakeFolder}\"", buildDir, getDebugFilePath($"build_cmake_preparation_{index}"));
            await Program.ExecuteAsync(Program.winboxSettings.path_cmake, $"--build . --config \"{configuration}\"", buildDir, getDebugFilePath($"build_cmake_build_{index}"));
            await Program.CopyFilesRecursivelyAsync(Path.Combine(buildDir, configuration), output);

            Directory.Delete(buildDir, true);
        }

        public async Task BuildCargoProject(int index, BuildItem buildItem, string cargoFolder, string output)
        {
            string name = Path.GetFileName(cargoFolder);
            string exeName = name + ".exe";

            string? target = null;
            switch (winBoxConfig.Architecture)
            {
                case "x64":
                    target = "x86_64-pc-windows-msvc";
                    break;

                case "x86":
                    target = "i686-pc-windows-msvc";
                    break;

                case "arm64":
                    target = "aarch64-pc-windows-msvc";
                    break;
            }

            string buildDir = Path.Combine(tempDirectoryPath, "cargo_build");
            if (Directory.Exists(buildDir))
            {
                Directory.Delete(buildDir, true);
            }
            Directory.CreateDirectory(buildDir);

            await Program.ExecuteAsync(Program.winboxSettings.path_cargo, $"build --release --target=\"{target}\" --target-dir=\"{buildDir}\"", cargoFolder, getDebugFilePath($"build_cargo_{index}"));
            await Program.CopyFileAsync(Path.Combine(buildDir, target, "release", exeName), Path.Combine(output, exeName));

            Directory.Delete(buildDir, true);
        }

        public async Task RunCustomBuildSystem(int index, BuildItem buildItem, string sourcesFolder, string output)
        {
            string tempBatFilePath = Path.Combine(tempDirectoryPath, "custom_build.bat");
            await File.WriteAllTextAsync(tempBatFilePath, buildItem.custom_command);
            await Program.ExecuteAsync(tempBatFilePath, $"\"{sourcesFolder}\" \"{output}\" \"{winBoxConfig.Architecture}\"", sourcesFolder, getDebugFilePath($"build_custom_{index}"));
            File.Delete(tempBatFilePath);
        }

        public async Task<bool> RunElectronBuildSystem(int index, BuildItem buildItem, string electronFolder, string output)
        {
            bool successfully = false;

            string architecture = winBoxConfig.Architecture;
            if (architecture == "x86") architecture = "ia32";

            string buildDir = Path.Combine(tempDirectoryPath, "electron_build");
            if (Directory.Exists(buildDir))
            {
                Directory.Delete(buildDir, true);
            }
            Directory.CreateDirectory(buildDir);

            await Program.ExecuteAsync("cmd.exe", "/c npm install", electronFolder, getDebugFilePath($"npm_install_{index}"));
            await Program.ExecuteAsync("cmd.exe", $"/c npx electron-rebuild --arch=\"{architecture}\"", electronFolder, getDebugFilePath($"npx_electron_rebuild_{index}"));
            await Program.ExecuteAsync("cmd.exe", $"/c npx electron-packager . \"{buildItem.electron_packager_name}\" --platform=win32 --arch=\"{architecture}\" --out=\"{buildDir}\"", electronFolder, getDebugFilePath($"build_electron_{index}"));

            string? releaseDirectory = null;
            foreach (string file in Directory.GetDirectories(buildDir))
            {
                releaseDirectory = file;
                successfully = true;
                break;
            }

            if (successfully)
            {
                await Program.CopyFilesRecursivelyAsync(releaseDirectory, output);
            }

            Directory.Delete(buildDir, true);

            return successfully;
        }

        public async Task<bool> BuildUserProject(int index, BuildItem buildItem, bool debug = false)
        {
            string outputDir = debugBuildProgramsPath;
            if (!debug)
            {
                if (buildItem.folderInProject.Contains("..")) return false;
                outputDir = Path.Combine(baseDirectoryPath, buildItem.folderInProject);
            }

            if (buildItem.subdirectory_enabled == true)
            {
                if (buildItem.subdirectory.Contains("..")) return false;
                outputDir = Path.Combine(outputDir, buildItem.subdirectory ?? "");
            }

            Program.CreateDirectory(outputDir);

            switch (buildItem.type)
            {
                case BuildItemType.msbuild:
                    if (Program.winboxSettings.path_msbuild != null)
                    {
                        string architecture = winBoxConfig.Architecture;
                        if (architecture == "arm64") architecture = "ARM64";
                        await Program.ExecuteAsync(Program.winboxSettings.path_msbuild,
                            $"\"{Path.Combine(sourcesDirectoryPath, buildItem.msbuild_path)}\" /p:Configuration=\"{buildItem.msbuild_configuration}\" /p:Platform=\"{architecture}\" /p:OutputPath=\"{outputDir}\" /p:OutDir=\"{outputDir}\"",
                            Path.GetDirectoryName(Path.Combine(sourcesDirectoryPath, buildItem.msbuild_path)),
                            getDebugFilePath($"build_msbuild_{index}"));
                        return true;
                    }
                    break;

                case BuildItemType.cmake:
                    if (Program.winboxSettings.path_cmake != null)
                    {
                        await BuildCMakeProject(index, buildItem, Path.GetDirectoryName(Path.Combine(sourcesDirectoryPath, buildItem.cmake_path)), outputDir);
                        return true;
                    }
                    break;

                case BuildItemType.cargo:
                    if (Program.winboxSettings.path_cargo != null)
                    {
                        await BuildCargoProject(index, buildItem, Path.GetDirectoryName(Path.Combine(sourcesDirectoryPath, buildItem.cargo_path)), outputDir);
                        return true;
                    }
                    break;

                case BuildItemType.custom:
                    await RunCustomBuildSystem(index, buildItem, Path.Combine(sourcesDirectoryPath, buildItem.custom_path), outputDir);
                    return true;

                case BuildItemType.electron_packager:
                    return await RunElectronBuildSystem(index, buildItem, Path.GetDirectoryName(Path.Combine(sourcesDirectoryPath, buildItem.electron_packager_path)), outputDir);
            }

            return false;
        }

        public async Task DownloadFile(DownloadItem downloadItem, Action<int> processValue)
        {
            if (downloadItem.path.Contains("..")) return;

            // -------------------------------- download

            bool needDelete = false;
            string downloadPath;
            if (downloadItem.cache == true)
            {
                downloadPath = Path.Combine(Program.downloadCachePath, Program.CalculateMD5(downloadItem.url));
                if (!Program.isFileDownloaded(downloadPath))
                {
                    await Program.downloadFile(downloadItem.url, downloadPath, processValue);
                }
            }
            else
            {
                needDelete = true;
                downloadPath = Path.Combine(Program.appdataPath, "last_download");
                await Program.downloadFile(downloadItem.url, downloadPath, processValue);
            }

            // -------------------------------- copy

            string outputPath = Path.Combine(baseDirectoryPath, downloadItem.path);

            if (downloadItem.unpack == true)
            {
                Directory.CreateDirectory(outputPath);
                await Program.ExecuteAsync(Program.z7Path, @$"x ""{downloadPath}"" -o""{outputPath}""", null, debugFolder);
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                await Program.CopyFileAsync(downloadPath, outputPath);
            }

            if (needDelete)
            {
                File.Delete(downloadPath);
            }
        }

        public static async Task ExportImg(string wimPath, string imgExportPath)
        {
            if (!File.Exists(wimPath))
                throw new FileNotFoundException("WIM file not found", wimPath);

            string vhdPath = Path.ChangeExtension(imgExportPath, ".vhd");
            long vhdSizeMb = 15000; // 40 GB в мегабайтах

            string workDir = Path.Combine(Path.GetTempPath(), "WimConv_" + Guid.NewGuid());
            Directory.CreateDirectory(workDir);

            try
            {
                // 1) Создаём скрипт для diskpart, который создаст VHD и разметит его
                string diskpartScript = Path.Combine(workDir, "diskpart.txt");
                await File.WriteAllTextAsync(diskpartScript, $@"
create vdisk file=""{vhdPath}"" maximum={vhdSizeMb} type=expandable
select vdisk file=""{vhdPath}""
attach vdisk
convert gpt
create partition efi size=100
format quick fs=fat32 label=System
assign letter=S
create partition msr size=16
create partition primary
format quick fs=ntfs label=Windows
assign letter=W
exit
");

                // 2) Запускаем diskpart для создания VHD и разделов
                await Program.ExecuteAsync("diskpart", $"/s \"{diskpartScript}\"");

                // 3) Применяем WIM образ в раздел Windows
                await Program.ExecuteAsync("dism", $"/Apply-Image /ImageFile:\"{wimPath}\" /Index:1 /ApplyDir:W:\\");

                // 4) Создаём EFI загрузчик
                await Program.ExecuteAsync("bcdboot", "W:\\Windows /s S: /f UEFI");

                // 5) Отключаем диск (VHD)
                // Отмонтировать диск можно через diskpart:
                string detachScript = Path.Combine(workDir, "detach.txt");
                await File.WriteAllTextAsync(detachScript, $@"
select vdisk file=""{vhdPath}""
detach vdisk
exit
");
                await Program.ExecuteAsync("diskpart", $"/s \"{detachScript}\"");

                // 6) Конвертируем VHD в RAW IMG через qemu-img
                await Program.ExecuteAsync(Path.Combine(Program.winboxSettings.path_qemu_folder, "qemu-img.exe"), $"convert -O raw \"{vhdPath}\" \"{imgExportPath}\"");

                Console.WriteLine("Экспорт завершён успешно.");
            }
            finally
            {
                if (Directory.Exists(workDir))
                    Directory.Delete(workDir, true);
            }
        }

        public async Task InstallToImg(string isoPath, string imgPath, bool useUefi=false)
        {
            string? emuName = null;
            switch (winBoxConfig.Architecture)
            {
                case "x64":
                    emuName = "qemu-system-x86_64.exe";
                    break;

                case "x86":
                    emuName = "qemu-system-i386.exe";
                    break;

                case "arm64":
                    emuName = "qemu-system-aarch64.exe";
                    break;

                default:
                    return;
            }

            string qemuPath = Path.Combine(Program.winboxSettings.path_qemu_folder, emuName);
            string qemuParameters = $"-drive file=\"{imgPath}\",format=raw -cdrom \"{isoPath}\" -boot d -m {winBoxConfig.img_install_ram} -smp {winBoxConfig.img_install_cpu} -D \"{getDebugFilePath("qemu-log")}\"";

            if (useUefi)
            {
                qemuParameters += $" -bios \"{Program.getBlobPath(winBoxConfig, Path.Combine("OVMF", winBoxConfig.Architecture + ".fd"))}\"";
            }

            await writeDebugFile("qemu-launch", $"\"{qemuPath}\" {qemuParameters}");

            await Program.ExecuteAsync(Path.Combine(Program.winboxSettings.path_qemu_folder, "qemu-img.exe"), $"create -f raw \"{imgPath}\" {winBoxConfig.img_size}M", null, debugFolder);
            await Program.ExecuteAsync(qemuPath, qemuParameters, null, debugFolder);
        }

        public async Task OverwriteSystemCursorEmpty(string cursorsPath)
        {
            string empty_cur = Program.ResourcePath(Path.Combine("resources", "empty.cur"));
            string empty_ani = Program.ResourcePath(Path.Combine("resources", "empty.ani"));
            string empty_svg = Program.ResourcePath(Path.Combine("resources", "empty.svg"));

            if (Directory.Exists(cursorsPath))
            {
                string[] files = Directory.GetFiles(cursorsPath);

                foreach (string file in files)
                {
                    string extension = Path.GetExtension(file).ToLower();

                    switch (extension)
                    {
                        case ".cur":
                            await Program.CopyFileAsync(empty_cur, file);
                            break;

                        case ".ani":
                            await Program.CopyFileAsync(empty_ani, file);
                            break;

                        case ".svg":
                            await Program.CopyFileAsync(empty_svg, file);
                            break;
                    }
                }
            }
        }

        string getPowercfgSetup()
        {
            string powerScheme = Program.powerSchemes[(int)winBoxConfig.powerScheme];
            string powercfgSetup = $@"powercfg -s {powerScheme}
{(winBoxConfig.enable_hibernation == true ? "powercfg -h on" : "powercfg -h off")}
powercfg -change -standby-timeout-ac {winBoxConfig.StandbyTimeout}
powercfg -change -standby-timeout-dc {(winBoxConfig.dc_use == true ? winBoxConfig.StandbyTimeout_dc : winBoxConfig.StandbyTimeout)}
powercfg -change -hibernate-timeout-ac {winBoxConfig.HibernateTimeout}
powercfg -change -hibernate-timeout-dc {(winBoxConfig.dc_use == true ? winBoxConfig.HibernateTimeout_dc : winBoxConfig.HibernateTimeout)}
powercfg -change -monitor-timeout-ac {winBoxConfig.ScreenTimeout}
powercfg -change -monitor-timeout-dc {(winBoxConfig.dc_use == true ? winBoxConfig.ScreenTimeout_dc : winBoxConfig.ScreenTimeout)}
powercfg -change -disk-timeout-ac {winBoxConfig.DiskTimeout}
powercfg -change -disk-timeout-dc {(winBoxConfig.dc_use == true ? winBoxConfig.DiskTimeout_dc : winBoxConfig.DiskTimeout)}
powercfg -setacvalueindex {powerScheme} SUB_BUTTONS LIDACTION {(int)winBoxConfig.action_closingLaptop}
powercfg -setdcvalueindex {powerScheme} SUB_BUTTONS LIDACTION {(int)(winBoxConfig.dc_use == true ? winBoxConfig.action_closingLaptop_dc : winBoxConfig.action_closingLaptop)}
powercfg -setacvalueindex {powerScheme} SUB_BUTTONS SBUTTONACTION {(int)winBoxConfig.action_sleepButton}
powercfg -setdcvalueindex {powerScheme} SUB_BUTTONS SBUTTONACTION {(int)(winBoxConfig.dc_use == true ? winBoxConfig.action_sleepButton_dc : winBoxConfig.action_sleepButton)}
powercfg -setacvalueindex {powerScheme} SUB_BUTTONS PBUTTONACTION {(int)winBoxConfig.action_powerButton}
powercfg -setdcvalueindex {powerScheme} SUB_BUTTONS PBUTTONACTION {(int)(winBoxConfig.dc_use == true ? winBoxConfig.action_powerButton_dc : winBoxConfig.action_powerButton)}
powercfg -s {powerScheme}";

            return powercfgSetup;
        }

        string getKeyboardFilterSetup()
        {
            string keyboardFilterSetup = "";

            if (needEnableKeyboardFilter())
            {
                keyboardFilterSetup = $@"reg add ""HKLM\SOFTWARE\Microsoft\Windows Embedded\KeyboardFilter"" /v EnableKeyboardFilter /t REG_DWORD /d 1 /f
reg add ""HKLM\SOFTWARE\Microsoft\Windows Embedded\KeyboardFilter"" /v ForceOffAccessibility /t REG_DWORD /d {(winBoxConfig.keyboard_filter_ForceOffAccessibility == true ? 1 : 0)} /f
reg add ""HKLM\SOFTWARE\Microsoft\Windows Embedded\KeyboardFilter"" /v DisableKeyboardFilterForAdministrators /t REG_DWORD /d {(winBoxConfig.keyboard_filter_DisableKeyboardFilterForAdministrators == true ? 1 : 0)} /f
reg add ""HKLM\SOFTWARE\Microsoft\Windows Embedded\KeyboardFilter"" /v BreakoutKeyScanCode /t REG_DWORD /d {winBoxConfig.keyboard_filter_BreakoutKeyScanCode} /f" + "\r\n";

                void blockKey(string key, bool blocked=true)
                {
                    keyboardFilterSetup += "\r\n" + $@"reg add ""HKLM\SOFTWARE\Microsoft\Windows Embedded\KeyboardFilter"" /v ""{key}"" /t REG_SZ /d ""{(blocked ? "Blocked" : "Allowed")}"" /f";
                }

                foreach (string key in winBoxConfig.keyboard_filter_blockList)
                {
                    blockKey(key);
                }
                
            }

            return keyboardFilterSetup;
        }

        string[] getCustomStopList()
        {
            return splitRickTextboxLinesWithoutEmptyLines(winBoxConfig.services_stop ?? "");
        }

        string[] getCustomDeleteList()
        {
            return splitRickTextboxLinesWithoutEmptyLines(winBoxConfig.services_deleteFromList ?? "");
        }

        string[] getCustomStartList()
        {
            return splitRickTextboxLinesWithoutEmptyLines(winBoxConfig.services_start ?? "");
        }

        public string[] getStopServicesList()
        {
            List<string> stopServices = new List<string>();

            string[] stopServicesList = {
                "SecurityHealthService",
                "Sense",
                "WdBoot",
                "WdFilter",
                "WdNisDrv",
                "WdNisSvc",
                "Superfetch",
                "OneSyncSvc",
                "OfficeClickToRun",
                "OneDrive",
                "Cortana",
                "SyncHost",
                "CompatTelRunner",
                "UsoSvc",
                "wlidsvcNetwork",

                "WpnUserService",
                "ClickToRunSvc",
                "VSS",

                "edgeupdate",
                "edgeupdatem",
                "wbengine",
                "wuauserv",
                "RemoteRegistry",
                "WSearch",
                "SysMain",
                "WerSvc",
                "shellhwdetection",
                "SSDPSRV",
                "TermService",
                "napagent",
                "WinDefend",
                "wlidsvc",
                "DiagTrack",
                "dmwappushservice",
                "Dosvc",
                "XboxGipSvc",
                "XblAuthManager",
                "XblGameSave",
                "XboxNetApiSvc",
                "WaaSMedicSvc",
                "WdNisSvc",
                "wscsvc",
                "wisvc",

                "PhoneSvc",
                "SessionEnv",
                "UmRdpService",
                "svsvc",
                "TapiSrv",
                "SDRSVC",
                "WbioSrvc",
                "mpssvc",
                "Wecsvc",
                "ClipSVC",
                "WpnService",
                "PushToInstall",
                "WinRM",
                "workfolderssvc",
                "WwanSvc",
                "AarSvc",
                "cbdhsvc",
                "CloudBackupRestoreSvc",
                "CDPUserSvc",
                "ConsentUxUserSvc",
                "PimIndexMaintenanceSvc",
                "UnistoreSvc",
                "wercplsupport",
                "PcaSvc",
                "RasMan",
                "DevicePickerUserSvc",
                "DevicesFlowUserSvc",
                "BcastDVRUserService",
                "MessagingService",
                "UdkUserSvc",
                "UserDataSvc",
                "AppXSvc",
                "CscService",
                "CSC"

                //"eventlog",
                //"lanmanserver"
                //"LanmanWorkstation"
            };

            if (winBoxConfig.services_stopOnlyList != true)
            {
                if (Program.isTweakEnabled(winBoxConfig, "make a quiet SPP"))
                {
                    stopServices.Add("sppsvc");
                }

                if (winBoxConfig.DisableNtp == true)
                {
                    stopServices.Add("w32time");
                }

                stopServices.AddRange(stopServicesList);
            }

            stopServices.AddRange(getCustomStopList());

            Program.DelRange(stopServices, getStartServicesList());
            Program.DelRange(stopServices, getCustomDeleteList());
            return stopServices.Distinct().ToArray();
        }

        bool needEnableKeyboardFilter()
        {
            return !Program.isTweakEnabled(winBoxConfig, "Do not disable hotkeys by keyboard filter") && winBoxConfig.keyboard_filter_enabled == true;
        }

        public string[] getStartServicesList()
        {
            List<string> startServices = new List<string>();
            if (winBoxConfig.services_startOnlyList != true)
            {
                if (needEnableKeyboardFilter()) startServices.Add("MsKeyboardFilter");
            }

            Program.DelRange(startServices, getCustomStopList());
            startServices.AddRange(getCustomStartList());
            Program.DelRange(startServices, getCustomDeleteList());
            return startServices.Distinct().ToArray();
        }

        string[] getEnableFeatures()
        {
            List<string> enableFeatures = new List<string>();
            if (winBoxConfig.manual_setup != true)
            {
                enableFeatures.Add("Client-DeviceLockdown");
                enableFeatures.Add("Client-EmbeddedLogon");
                if (needEnableKeyboardFilter()) enableFeatures.Add("Client-KeyboardFilter");
                enableFeatures.Add("Client-EmbeddedBootExp");
            }

            if (winBoxConfig.customdism_enabled == true)
            {
                foreach (string feature in splitRickTextboxLinesWithoutEmptyLines(winBoxConfig.customdism_features ?? ""))
                {
                    enableFeatures.Add(feature);
                }
            }

            return enableFeatures.Distinct().ToArray();
        }

        string getServicesSetup(bool onlyRegStop=false)
        {
            string[] stopServices = getStopServicesList();
            string[] startServices = getStartServicesList();

            string servicesSetup = "";
            foreach (string service in stopServices)
            {
                servicesSetup += $"echo stop service: {service} >> C:\\WinboxResources\\setup.log\r\n";
                servicesSetup += $"echo only reg stop: {onlyRegStop} >> C:\\WinboxResources\\setup.log\r\n";
                if (!onlyRegStop)
                {
                    servicesSetup += $"sc stop {service}\r\n";
                    servicesSetup += $"sc config {service} start= disabled\r\n";
                    servicesSetup += $"net stop {service}\r\n";
                }
                servicesSetup += $@"reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\{service}"" /v Start /t REG_DWORD /d 4 /f" + "\r\n";
                servicesSetup += $"echo service {service} stoped >> C:\\WinboxResources\\setup.log\r\n";
            }

            foreach (string service in startServices)
            {
                servicesSetup += $"echo start service: {service} >> C:\\WinboxResources\\setup.log\r\n";
                servicesSetup += $"sc config {service} start= auto\r\n";
                servicesSetup += $"sc start {service}\r\n";
                servicesSetup += $"net start {service}\r\n";
                servicesSetup += $"echo service {service} started >> C:\\WinboxResources\\setup.log\r\n";
            }

            return servicesSetup;
        }

        public string[] getStopOrDeleteSchtasksList()
        {
            List<string> stopOrDeleteSchtasks = new List<string>();
            if (winBoxConfig.schtasks_stopOrDeleteOnlyFromList != true)
            {
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\Application Experience\Microsoft Compatibility Appraiser");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\Application Experience\ProgramDataUpdater");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\Autochk\Proxy");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\Customer Experience Improvement Program\Consolidator");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\Customer Experience Improvement Program\KernelCeipTask");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\Customer Experience Improvement Program\UsbCeip");

                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\AppID\SmartScreenSpecific");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\Application Experience\AitAgent");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\Application Experience\StartupAppTask");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\ApplicationData\appuriverifierdaily");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\ApplicationData\appuriverifierinstall");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\Device Information\Device");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\Diagnosis\Scheduled");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\DiskDiagnostic\Microsoft-Windows-DiskDiagnosticDataCollector");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\LanguageComponentsInstaller\Installation");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\LanguageComponentsInstaller\Uninstallation");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\Maintenance\WinSAT");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\Maps\MapsToastTask");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\Maps\MapsUpdateTask");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\Mobile Broadband Accounts\MNO Metadata Parser");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\MobilePC\HotStart");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\MUI\LPRemove");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\NetTrace\GatherNetworkInfo");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\Power Efficiency Diagnostics\AnalyzeSystem");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\RAC\RacTask");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\RemoteAssistance\RemoteAssistanceTask");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\RetailDemo\CleanupOfflineContent");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\SettingSync\BackgroundUploadTask");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\SettingSync\BackupTask");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\SettingSync\NetworkStateChangeTask");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\Setup\EOSNotify");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\Setup\EOSNotify2");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\Setup\SetupCleanupTask");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\Speech\SpeechModelDownloadTask");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\SystemRestore\SR");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\Time Synchronization\SynchronizeTime");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\Windows Error Reporting\QueueReporting");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\WindowsBackup\ConfigNotification");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\WS\License Validation");
                stopOrDeleteSchtasks.Add(@"\Microsoft\Windows\WS\WSRefreshBannedAppsListTask");
                stopOrDeleteSchtasks.Add(@"\Microsoft\XblGameSave\XblGameSaveTask");
                stopOrDeleteSchtasks.Add(@"\Microsoft\XblGameSave\XblGameSaveTaskLogon");
            }

            foreach (string _schtask in splitRickTextboxLinesWithoutEmptyLines(winBoxConfig.schtasks_stopOrDelete ?? ""))
            {
                string schtask = _schtask.Replace("/", "\\");
                if (!schtask.Contains("..") && !schtask.Contains(":"))
                {
                    stopOrDeleteSchtasks.Add(schtask);
                }
            }

            Program.DelRange(stopOrDeleteSchtasks, splitRickTextboxLinesWithoutEmptyLines(winBoxConfig.schtasks_stopOrDelete_deleteFromList ?? ""));
            return stopOrDeleteSchtasks.Distinct().ToArray();
        }

        string getSchtasksSetup()
        {
            string[] stopOrDeleteSchtasks = getStopOrDeleteSchtasksList();

            string schtasksSetup = "";
            foreach (string schtask in stopOrDeleteSchtasks)
            {
                bool delete = !schtask.StartsWith("!");
                string schtaskPath = schtask;
                if (!delete)
                {
                    schtaskPath = schtask.Substring(1);
                }

                schtasksSetup += $"echo {(delete ? "delete" : "stop")} schtask: {schtaskPath} >> C:\\WinboxResources\\setup.log\r\n";
                if (delete)
                {
                    schtasksSetup += $"schtasks /Delete /TN \"{schtaskPath}\" /F\r\n";
                }
                else
                {
                    schtasksSetup += $"schtasks /Change /TN \"{schtaskPath}\" /disable\r\n";
                }
                schtasksSetup += $"echo schtask {schtaskPath} {(delete ? "deleted" : "stoped")} >> C:\\WinboxResources\\setup.log\r\n";
            }
            return schtasksSetup;
        }

        string getKeyboardLayoutsSetup()
        {
            string cmd = @"reg delete ""HKEY_LOCAL_MACHINE\DEFAULT_USER\Keyboard Layout\Preload"" /va /f" + "\r\n";

            int index = 1;
            foreach (TwoStrings twoStrings in winBoxConfig.keyboard_layouts) {
                cmd += @$"reg add ""HKEY_LOCAL_MACHINE\DEFAULT_USER\Keyboard Layout\Preload"" /v ""{index++}"" /t REG_SZ /d ""{twoStrings.string2}"" /f" + "\r\n";
            }

            return cmd;
        }

        string[] getSchtasksDeletePaths()
        {
            string[] stopOrDeleteSchtasks = getStopOrDeleteSchtasksList();
            List<string> deletePaths = new List<string>();

            foreach (string schtask in stopOrDeleteSchtasks)
            {
                bool delete = !schtask.StartsWith("!");
                if (delete)
                {
                    deletePaths.Add(schtask.TrimStart('\\', '/'));
                }
            }

            return stopOrDeleteSchtasks.Distinct().ToArray();
        }

        async Task addAdFiles(string path, WindowsDescription newWindowsDescription, bool? advertising = true, bool? info = true)
        {
            if (advertising == true) await File.WriteAllTextAsync(Path.Combine(path, "README.txt"), $"this image was created by the {Program.version} free software\r\nhttps://github.com/igorkll/WinBox-Maker");
            if (info == true) await File.WriteAllTextAsync(Path.Combine(path, "INFO.txt"), $"name: {newWindowsDescription.name}\r\ndescription: {newWindowsDescription.description}");
        }

        public string[] splitRickTextboxLines(string text)
        {
            return text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        public string[] splitRickTextboxLinesWithoutEmptyLines(string text)
        {
            string[] array = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> list = new List<string>(array);
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].Contains("\"") || list[i].Contains("'"))
                {
                    list.RemoveAt(i);
                }
            }
            return list.ToArray();
        }

        bool needMountRecovery()
        {
            return winBoxConfig.aaf_readme_recovery == true ||
                winBoxConfig.aaf_info_recovery == true ||
                winBoxConfig.recoveryMountedEarly_breakafter == true ||
                winBoxConfig.recoveryMountedEarly_breakbefore == true ||
                winBoxConfig.recoveryMountedEarlyEnabled == true ||
                winBoxConfig.recovery_winPE_mod.enabled == true ||
                Program.hasDirectoryNotEmpty(Path.Combine(resourcesDirectoryPath, "recovery_files")) ||
                Program.hasDirectoryNotEmpty(Path.Combine(tempDirectoryPath, "recovery_files"));
        }

        bool needMountInstallerBoot()
        {
            return winBoxConfig.aaf_readme_boot == true ||
                winBoxConfig.aaf_info_boot == true ||
                Program.hasDirectoryNotEmpty(Path.Combine(resourcesDirectoryPath, "boot_files")) ||
                Program.hasDirectoryNotEmpty(Path.Combine(tempDirectoryPath, "boot_files"));
        }

        async Task patchRecoveryPartition(string mountedRecoveryPath, WindowsDescription newWindowsDescription)
        {
            await addAdFiles(mountedRecoveryPath, newWindowsDescription, winBoxConfig.aaf_readme_recovery, winBoxConfig.aaf_info_recovery);

            string recoveryFilesPath = Path.Combine(resourcesDirectoryPath, "recovery_files");
            if (Directory.Exists(recoveryFilesPath))
            {
                await Program.CopyFilesRecursivelyAsync(recoveryFilesPath, mountedRecoveryPath);
            }

            recoveryFilesPath = Path.Combine(tempDirectoryPath, "recovery_files");
            if (Directory.Exists(recoveryFilesPath))
            {
                await Program.CopyFilesRecursivelyAsync(recoveryFilesPath, mountedRecoveryPath);
            }
        }

        async Task WriteApiScript(string scriptname, string script)
        {
            await File.WriteAllTextAsync(Path.Combine(WinboxApiPath, scriptname), script);
        }

        string? ExtractAnyFromDismResult(string dismLine, string prefix)
        {
            // DISM выводит строку примерно так:
            // Package Identity : Microsoft-Windows-Subsystem-Linux-Package~31bf3856ad364e35~amd64~~10.0.19041.1
            // Нужно вытащить всё после "Package Identity : "

            var match = Regex.Match(dismLine, prefix + @"\s*:\s*(.+)");
            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }

            return null;
        }

        async Task<string[]> getAnyFromDism(string args, string prefix)
        {
            string result = await Program.ExecuteAsync("dism.exe", args, null, debugFolder);
            string[] resultLines = result.Split(new[] { "\r\n", "\n\r", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

            var outputLines = new List<string>();

            foreach (string line in resultLines)
            {
                string? outputLine = ExtractAnyFromDismResult(line, prefix);
                if (outputLine != null)
                {
                    outputLines.Add(outputLine);
                }
            }

            return outputLines.ToArray();
        }

        async Task<string[]> getFromDismWithOffset(string args, string lineStartsWith, string prefix, int offset)
        {
            string result = await Program.ExecuteAsync("dism.exe", args, null, debugFolder);
            string[] resultLines = result.Split(new[] { "\r\n", "\n\r", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

            var outputLines = new List<string>();

            int outputLineIndex = 0;
            foreach (string line in resultLines)
            {
                if (line.StartsWith(lineStartsWith))
                {
                    string? outputLine = ExtractAnyFromDismResult(resultLines[outputLineIndex + offset], prefix);
                    if (outputLine != null) outputLines.Add(outputLine);
                }
                outputLineIndex++;
            }

            return outputLines.ToArray();
        }

        async Task<string[]> getImagePackagesList(bool provisioned=false)
        {
            // я удивлен что недокументированый ключ "/English" вообще сработал
            return await getAnyFromDism(
                provisioned ? $"/English /image:\"{wimMountPath}\" /Get-ProvisionedAppxPackages" : $"/English /image:\"{wimMountPath}\" /Get-Packages",
                provisioned ? @"PackageName" : @"Package Identity"
            );
        }

        string[] getFullPackagesNames(string[] packageNames, string packageNamePart)
        {
            return packageNames.Where(line => line.Contains(packageNamePart, StringComparison.OrdinalIgnoreCase)).ToArray();
        }

        public async Task<bool> MakeModWim(Action<string> processName, Action<int> processValue, WindowsDescription newWindowsDescription, string newWimPath, string? imgExportPath, bool initViaVmMode = false)
        {
            string RemovePaths_log = "";

            async Task removeSystemObject(string path, string? systemRoot=null)
            {
                RemovePaths_log += $"remove path request: {path}\n";

                bool createEmptyDir = true;
                if (path.StartsWith("!"))
                {
                    createEmptyDir = false;
                    path = path.Substring(1);
                    RemovePaths_log += $"disable create empty dir: {path}\n";
                }
                path = path.Replace("/", "\\");
                path = path.TrimStart('\\', '/');

                RemovePaths_log += $"path processing: {path}\n";

                if (path.StartsWith("/") || path.StartsWith("\\") || path.Contains("..") || path.Contains(":"))
                {
                    RemovePaths_log += $"bad path: {path}\n";
                    return;
                }

                RemovePaths_log += $"launching deleting: {path}\n";

                if (systemRoot != null)
                {
                    RemovePaths_log += $"in folder: {systemRoot}\n";
                    path = Path.Combine(wimMountPath, systemRoot, path);
                }
                else
                {
                    path = Path.Combine(wimMountPath, path);
                }

                RemovePaths_log += $"result host path: {path}\n";

                await Task.Run(() => {
                    bool recreateDir = false;
                    bool successfully = false;

                    if (Directory.Exists(path))
                    {
                        RemovePaths_log += $"try delete directory\n";
                        Program.SetAttributesRecursive(path, FileAttributes.Normal);
                        Directory.Delete(path, true);
                        recreateDir = true;
                        successfully = true;
                    }

                    if (File.Exists(path))
                    {
                        RemovePaths_log += $"try delete file\n";
                        File.SetAttributes(path, FileAttributes.Normal);
                        File.Delete(path);
                        successfully = true;
                    }

                    if (Directory.Exists(path))
                    {
                        RemovePaths_log += $"failed to delete directory\n";
                    }
                    else if (File.Exists(path))
                    {
                        RemovePaths_log += $"failed to delete file\n";
                    }
                    else if (successfully)
                    {
                        RemovePaths_log += $"successfully deleted\n";
                    }
                    else
                    {
                        RemovePaths_log += $"path not found\n";
                    }

                    if (recreateDir && createEmptyDir && successfully)
                    {
                        RemovePaths_log += $"create empty directory\n";
                        Program.CreateDirectory(path);
                    }
                });

                RemovePaths_log += "\n";
            }

            bool manual = winBoxConfig.manual_setup == true;

            processValue(2);
            await RemoveTemp(processName, true);

            if (winBoxConfig.prebuild_breakbefore == true) breakpointStop("pre-build", false);
            if (winBoxConfig.prebuildEnabled == true)
            {
                processValue(5);
                processName("Executing a pre-build event");
                await Program.executeBuildEvent(baseDirectoryPath, winBoxConfig.prebuildEvent);
            }
            if (winBoxConfig.prebuild_breakafter == true) breakpointStop("pre-build", true);

            // ------------------------------------ compiling a user program

            if (winBoxConfig.downloadEnabled == true)
            {
                processValue(10);
                processName("Downloading user files");
                foreach (DownloadItem downloadItem in winBoxConfig.DownloadItems)
                {
                    try
                    {
                        await DownloadFile(downloadItem, processValue);
                    } catch (Exception ex) {
                        Program.Error("couldn't download user file: " + ex);
                        return false;
                    }
                }
            }

            string tempProgramPath = Path.Combine(tempDirectoryPath, "program");
            if (winBoxConfig.buildEnabled == true)
            {
                processValue(15);
                processName("Compiling a user project");
                int index = 1;
                foreach (BuildItem buildItem in winBoxConfig.BuildItems)
                {
                    if (!await BuildUserProject(index, buildItem))
                    {
                        Program.Error("couldn't build a custom project. the paths to the required build system may not be configured in the winbox maker settings");
                        return false;
                    }
                    index++;
                }
            }

            // ------------------------------------ creating a new install.wim file for subsequent modification

            if (!await ExtractInstallWim(processName, processValue))
            {
                Program.Error("couldn't extract install.wim, make sure the path to the iso file is correct");
                return false;
            }

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

            processValue(25);
            await DeleteInstallWim(processName);

            // ------------------------------------ mounting system
            processName("Mounting install.wim");
            processValue(30);
            await mountDism(newWimPath);

            if (winBoxConfig.winmountedEarly_breakbefore == true) breakpointStop("win-mounted-early", false);
            if (winBoxConfig.winmountedEarlyEnabled == true)
            {
                processValue(63);
                processName("Executing a win-mounted-early event");
                await Program.executeBuildEvent(baseDirectoryPath, winBoxConfig.winmountedEarlyEvent);
            }
            if (winBoxConfig.winmountedEarly_breakafter == true) breakpointStop("win-mounted-early", true);

            // ------------------------------------ modification of the recovery menu
            if (!manual || winBoxConfig.recoverymod_manual_allow == true)
            {
                processName("Modification of the recovery menu");
                processValue(35);

                string winREpath = Path.Combine(wimMountPath, "Windows\\System32\\Recovery\\Winre.wim");

                bool deleteRecovery = false;
                switch (winBoxConfig.recoveryMenuAction)
                {
                    case RecoveryMenuAction.Replace:
                        if (winBoxConfig.ReplaceRecovery != null && winBoxConfig.ReplaceRecovery.Length > 0)
                        {
                            string path = GetAbsoluteResourcePath(winBoxConfig.ReplaceRecovery);
                            if (File.Exists(path))
                            {
                                await Program.CopyFileAsync(path, winREpath);
                            }
                            else
                            {
                                deleteRecovery = true;
                            }
                        }
                        else
                        {
                            deleteRecovery = true;
                        }
                        break;

                    case RecoveryMenuAction.StayDefault:
                        break;

                    default:
                        deleteRecovery = true;
                        break;
                }

                if (deleteRecovery)
                    await removeSystemObject("Windows\\System32\\Recovery");

                if (File.Exists(winREpath) && needMountRecovery()) {
                    await mountDism(winREpath, recoveryMountPath);

                    if (winBoxConfig.recoveryMountedEarly_breakbefore == true) breakpointStop("recovery-mounted", false);
                    if (winBoxConfig.recoveryMountedEarlyEnabled == true)
                    {
                        processValue(63);
                        processName("Executing a recovery-mounted event");
                        await Program.executeBuildEvent(baseDirectoryPath, winBoxConfig.recoveryMountedEarlyEvent);
                    }
                    if (winBoxConfig.recoveryMountedEarly_breakafter == true) breakpointStop("recovery-mounted", true);

                    await patchRecoveryPartition(recoveryMountPath, newWindowsDescription);
                    await winBoxConfig.recovery_winPE_mod.modMountedWim(recoveryMountPath);
                    await umountDism(true, recoveryMountPath);
                }
            }

            // ------------------------------------ tweaks

            if (!manual) {
                processName("Modification of BCD");
                processValue(45);
                await BcdChanger.modifyWinBCD(wimMountPath);
            }

            bool modSystemReg = !manual || winBoxConfig.onbuild_reg != null;

            processName("Modification of the system files");
            processValue(50);
            if (modSystemReg)
            {
                await RegChanger.mountReg();
                //await Program.ExecuteAsync("reg.exe", $"load HKLM\\WINBOX_SYSTEM \"{Path.Combine(wimMountPath, "Windows\\System32\\config\\SYSTEM")}\"");
            }

            string WindowsScriptsPath = Path.Combine(wimMountPath, "Windows\\Setup\\Scripts");
            string WinboxResourcesPath = Path.Combine(wimMountPath, "WinboxResources");
            if (!manual)
            {
                Directory.CreateDirectory(WindowsScriptsPath);
                Directory.CreateDirectory(WinboxResourcesPath);
                Directory.CreateDirectory(WinboxApiPath);
            }

            async Task addOtherDrivers(string baseDir)
            {
                string nvidiaDriversPath = Path.Combine(baseDir, "nvidia_drivers");
                if (Directory.Exists(nvidiaDriversPath))
                {
                    string[] files = Directory.GetFiles(nvidiaDriversPath);
                    int number = 0;
                    foreach (string file in files)
                    {
                        string path = Path.Combine(tempDirectoryPath, "drivers", "nvidia" + number);
                        Directory.CreateDirectory(path);
                        await Program.ExecuteAsync(Program.z7Path, @$"x ""{file}"" -o""{path}""", null, debugFolder);
                        number++;
                    }
                }

                string amdDriversPath = Path.Combine(baseDir, "amd_drivers");
                if (Directory.Exists(amdDriversPath))
                {
                    string[] files = Directory.GetFiles(amdDriversPath);
                    int number = 0;
                    foreach (string file in files)
                    {
                        string path = Path.Combine(tempDirectoryPath, "drivers", "amd" + number);
                        Directory.CreateDirectory(path);
                        await Program.ExecuteAsync(Program.z7Path, @$"x ""{file}"" -o""{path}""", null, debugFolder);
                        number++;
                    }
                }

                string intelDriversPath = Path.Combine(baseDir, "intel_drivers");
                if (Directory.Exists(intelDriversPath))
                {
                    string[] files = Directory.GetFiles(intelDriversPath);
                    int number = 0;
                    foreach (string file in files)
                    {
                        string path = Path.Combine(tempDirectoryPath, "drivers", "intel" + number);
                        Directory.CreateDirectory(path);
                        await Program.ExecuteAsync(Program.z7Path, @$"x ""{file}"" -o""{path}""", null, debugFolder);
                        number++;
                    }
                }

                string anyDriversPath = Path.Combine(baseDir, "driver_installers");
                if (Directory.Exists(anyDriversPath))
                {
                    string[] files = Directory.GetFiles(anyDriversPath);
                    int number = 0;
                    foreach (string file in files)
                    {
                        string path = Path.Combine(tempDirectoryPath, "drivers", "any" + number);
                        Directory.CreateDirectory(path);
                        await Program.ExecuteAsync(Program.z7Path, @$"x ""{file}"" -o""{path}""", null, debugFolder);
                        number++;
                    }
                }
            }

            await addOtherDrivers(resourcesDirectoryPath);
            await addOtherDrivers(tempDirectoryPath);

            if (!manual)
            {
                await Program.ExecuteAsync("reg.exe", $"import \"{Program.ResourcePath(Path.Combine("reg", "tweak.reg"))}\"", null, debugFolder);
                if (Program.isTweakEnabled(winBoxConfig, "make a quiet SPP"))
                {
                    await Program.ExecuteAsync("reg.exe", $"import \"{Program.ResourcePath(Path.Combine("reg", "quiet_spp.reg"))}\"", null, debugFolder);
                }

                string executablePath = Path.Combine(WinboxResourcesPath, "executable");
                Directory.CreateDirectory(executablePath);
            }


            // ------------------------------------ system init

            string applicationScript = $@"@echo off" + "\r\n";
            string reboot_to_desktop_cmd = "reg add \"HKLM\\Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\" /v Shell /t REG_SZ /d \"explorer.exe\" /f\r\nshutdown /r /t 0\r\npause";

            if (!manual)
            {
                string bcdeditSetup = BcdChanger.getBcdeditSetup();
                string powercfgSetup = getPowercfgSetup();

                string enableDismOnlineCommands = "";
                foreach (string feature in getEnableFeatures())
                {
                    enableDismOnlineCommands += $"dism /online /enable-feature /all /featurename:\"{feature}\"\r\n";
                }

                string setupCompleteAndFirstInit = $@"echo SetupComplete and FirstInit - start >> C:\WinboxResources\setup.log

echo SetupComplete and FirstInit - setup recovery >> C:\WinboxResources\setup.log
reagentc.exe {(winBoxConfig.EnableRecovery == true ? "/enable" : "/disable")}

echo SetupComplete and FirstInit - setup BCD >> C:\WinboxResources\setup.log
{bcdeditSetup}

echo SetupComplete and FirstInit - disable firewall >> C:\WinboxResources\setup.log
netsh advfirewall set allprofiles state off

echo SetupComplete and FirstInit - setup dism >> C:\WinboxResources\setup.log
{enableDismOnlineCommands}

echo SetupComplete and FirstInit - setup powercfg >> C:\WinboxResources\setup.log
{powercfgSetup}

echo SetupComplete and FirstInit - setup keyboard filter >> C:\WinboxResources\setup.log
{getKeyboardFilterSetup()}

echo SetupComplete and FirstInit - end >> C:\WinboxResources\setup.log";

                string updateSystemSettingsAndFirstInit = $@"powershell -Command ""Set-MpPreference -DisableBlockAtFirstSeen $true""
powershell -Command ""Set-MpPreference -DisableTamperProtection $true""
powershell -Command ""Set-MpPreference -DisableRealtimeMonitoring $true""
powershell -Command ""Set-MpPreference -DisableIOAVProtection $true""
powershell -Command ""Set-MpPreference -DisableBehaviorMonitoring $true""
powershell -Command ""Set-MpPreference -DisableScriptScanning $true""
powershell -Command ""Set-MpPreference -SubmitSamplesConsent 2""
powershell -Command ""Set-MpPreference -MAPSReporting 0""
powershell -Command ""Set-MpPreference -DisableEnhancedNotifications $true

powershell -Command ""Set-MpPreference -DisableBlockAtFirstSeen $true""
powershell -Command ""Set-MpPreference -DisableTamperProtection $true""
powershell -Command ""Set-MpPreference -DisableRealtimeMonitoring $true""
powershell -Command ""Set-MpPreference -DisableIOAVProtection $true""
powershell -Command ""Set-MpPreference -DisableBehaviorMonitoring $true""
powershell -Command ""Set-MpPreference -DisableScriptScanning $true""
powershell -Command ""Set-MpPreference -SubmitSamplesConsent 2""
powershell -Command ""Set-MpPreference -MAPSReporting 0""
powershell -Command ""Set-MpPreference -DisableEnhancedNotifications $true""

{bcdeditSetup}";
                //why do I change the bcd every time I start?
                //because in some versions of windows (old enterprise),
                //bcd changes may otherwise remain unchanged if done in setup complete,
                //which will create a vulnerability so that the system restore window can open.
                //This is one of those cases where it is better to solve a problem in several ways at once.
                int hiberboot = (winBoxConfig.enable_hibernation == true && winBoxConfig.enable_hiberboot == true) ? 1 : 0;
                int disabledisplay = (winBoxConfig.bsod_disabledisplay == true) ? 1 : 0;
                string baseSetup = $@"echo SetupComplete - start >> C:\WinboxResources\setup.log

echo SetupComplete - call SetupComplete and FirstInit >> C:\WinboxResources\setup.log
{setupCompleteAndFirstInit}

echo SetupComplete - setup services >> C:\WinboxResources\setup.log
{getServicesSetup(true)}

echo SetupComplete - add executable to PATH >> C:\WinboxResources\setup.log
setx PATH ""%PATH%;C:\WinboxResources\executable"" /M

echo SetupComplete - call UpdateSystemSettings >> C:\WinboxResources\setup.log
call ""C:\WinboxResources\UpdateSystemSettings.bat""

echo SetupComplete - add UpdateSystemSettings to schtasks >> C:\WinboxResources\setup.log
schtasks /create /tn ""winbox_UpdateSystemSettings"" /tr ""C:\WinboxResources\UpdateSystemSettings.bat"" /sc onlogon /rl highest /ru ""SYSTEM""

echo SetupComplete - setup schtasks >> C:\WinboxResources\setup.log
{getSchtasksSetup()}

echo SetupComplete - DisableTamperProtection >> C:\WinboxResources\setup.log
powershell -Command ""Set-MpPreference -DisableTamperProtection $true""

echo SetupComplete - setup SYSTEM >> C:\WinboxResources\setup.log
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"" /v AutoReboot /t REG_DWORD /d {((winBoxConfig.bsod_autoreboot == true) ? 1 : 0)} /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"" /v CrashDumpEnabled /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"" /v LogEvent /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"" /v Overwrite /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"" /v EnableLogFile /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"" /v DisplayError /t REG_DWORD /d {(1 - disabledisplay)} /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl"" /v DisplayDisabled /t REG_DWORD /d {disabledisplay} /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\HardwareEvents"" /v MaxSize /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\Application"" /v MaxSize /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\Security"" /v MaxSize /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\System"" /v MaxSize /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile"" /v EnableFirewall /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile"" /v EnableFirewall /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power"" /v HiberbootEnabled /t REG_DWORD /d {hiberboot} /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance"" /v fAllowFullControl /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance"" /v fAllowToGetHelp /t REG_DWORD /d 0 /f
{(Program.isTweakEnabled(winBoxConfig, "Hide system errors") ? @"reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Windows"" /v ErrorMode /t REG_DWORD /d 2 /f" : "")}

echo SetupComplete - setup Memory Management >> C:\WinboxResources\setup.log
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control"" /v ProcessTerminationOnMemoryExhaustion /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"" /v DisableAutomaticTermination /t REG_DWORD /d 1 /f

echo SetupComplete - setup EmbeddedLogon >> C:\WinboxResources\setup.log
reg add ""HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Embedded\EmbeddedLogon"" /v HideAutoLogonUI /t REG_DWORD /d 1 /f
reg add ""HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Embedded\EmbeddedLogon"" /v HideFirstLogonAnimation /t REG_DWORD /d 1 /f
reg add ""HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Embedded\EmbeddedLogon"" /v BrandingNeutral /t REG_DWORD /d 1 /f
reg add ""HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Embedded\EmbeddedLogon"" /v NoLockScreen /t REG_DWORD /d 1 /f
reg add ""HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Embedded\EmbeddedLogon"" /v AnimationDisabled /t REG_DWORD /d 1 /f
reg add ""HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Embedded\EmbeddedLogon"" /v UIVerbosityLevel /t REG_DWORD /d 1 /f

echo SetupComplete - load DEFAULT_USER >> C:\WinboxResources\setup.log
reg load HKLM\DEFAULT_USER ""C:\Users\Default\NTUSER.DAT""
reg add ""HKEY_LOCAL_MACHINE\DEFAULT_USER\Control Panel\Accessibility\StickyKeys"" /v Flags /t REG_DWORD /d 506 /f
reg add ""HKEY_LOCAL_MACHINE\DEFAULT_USER\Control Panel\Sound"" /v Beep /t REG_SZ /d no /f
reg add ""HKEY_LOCAL_MACHINE\DEFAULT_USER\Control Panel\Sound"" /v ExtendedSounds /t REG_SZ /d no /f
reg add ""HKEY_LOCAL_MACHINE\DEFAULT_USER\SOFTWARE\Microsoft\Windows\DWM"" /v AccentColor /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\DEFAULT_USER\SOFTWARE\Microsoft\Windows\DWM"" /v ColorizationColor /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\DEFAULT_USER\Software\Microsoft\Windows\Windows Error Reporting"" /v DontShowUI /t REG_DWORD /d 1 /f
reg add ""HKEY_LOCAL_MACHINE\DEFAULT_USER\Software\Microsoft\Windows\Windows Error Reporting"" /v Disabled /t REG_DWORD /d 1 /f
reg add ""HKEY_LOCAL_MACHINE\DEFAULT_USER\Control Panel\Desktop"" /v UserPreferencesMask /t REG_BINARY /d 9012038010000000 /f
reg add ""HKEY_LOCAL_MACHINE\DEFAULT_USER\Control Panel\Desktop\WindowMetrics"" /v MinAnimate /t REG_SZ /d ""0"" /f
reg add ""HKEY_LOCAL_MACHINE\DEFAULT_USER\Software\NVIDIA Corporation\Global\NVTweak"" /v OverlayHook /t REG_DWORD /d 0 /f
reg add ""HKEY_LOCAL_MACHINE\DEFAULT_USER\Control Panel\Desktop"" /v HungAppTimeout /t REG_SZ /d ""2147483647"" /f
reg add ""HKEY_LOCAL_MACHINE\DEFAULT_USER\Control Panel\Desktop"" /v WaitToKillAppTimeout /t REG_SZ /d ""5000"" /f
reg add ""HKEY_LOCAL_MACHINE\DEFAULT_USER\Control Panel\Desktop"" /v AutoEndTasks /t REG_SZ /d ""1"" /f

echo SetupComplete - setup keyboard layouts >> C:\WinboxResources\setup.log
{getKeyboardLayoutsSetup()}";

                string updateSystemSettings = $@"reg add ""HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\SessionData"" /v AllowLockScreen /t REG_DWORD /d 0 /f

{updateSystemSettingsAndFirstInit}";

                string firstInit = $@"echo FirstInit - start >> C:\WinboxResources\setup.log

{getServicesSetup()}

echo FirstInit - end >> C:\WinboxResources\setup.log";

                void regAppScriptFirstInitCmd(string name, string cmd, bool writeFirst = false)
                {
                    string writeFileCmd = $"\r\necho. > \"C:\\WinboxResources\\{name}.installed\"";
                    applicationScript += $"\r\nIF NOT EXIST \"C:\\WinboxResources\\{name}.installed\" (";
                    if (writeFirst) applicationScript += writeFileCmd;
                    applicationScript += $"\r\n{cmd}";
                    if (!writeFirst) applicationScript += writeFileCmd;
                    applicationScript += $"\r\n)\r\n";
                }

                void baseSetupLog(string log)
                {
                    baseSetup += "\r\n" + $@"echo SetupComplete - {log} >> C:\WinboxResources\setup.log" + "\r\n";
                }

                regAppScriptFirstInitCmd("firstInit1", setupCompleteAndFirstInit);
                regAppScriptFirstInitCmd("firstInit2", updateSystemSettingsAndFirstInit);
                regAppScriptFirstInitCmd("firstInit3", firstInit);

                if (winBoxConfig.computername_use == true)
                {
                    baseSetupLog("rename computer");
                    baseSetup += $"\r\nPowerShell -Command \"Rename-Computer -NewName '{winBoxConfig.computername}'\"";
                }

                if (!Program.isTweakEnabled(winBoxConfig, "Allow check-disk"))
                {
                    baseSetupLog("disable checkdisk");
                    baseSetup += $"\r\n" + @"reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"" /v AutoChkTimeout /t REG_DWORD /d 0 /f";
                    baseSetup += $"\r\n" + @"reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager"" /v BootExecute /t REG_MULTI_SZ /d ""autocheck autochk /k:*"" /f";
                }

                if (Program.isTweakEnabled(winBoxConfig, "Disable security mitigations (performance boost)"))
                {
                    baseSetupLog("disable security mitigations");
                    baseSetup += $"\r\n" + @"reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"" /v FeatureSettingsOverride /t REG_DWORD /d 0xFFFFFFFF /f";
                    baseSetup += $"\r\n" + @"reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management"" /v FeatureSettingsOverrideMask /t REG_DWORD /d 0xFFFFFFFF /f";
                }

                if (Program.isTweakEnabled(winBoxConfig, "Enable CrashOnCtrlScroll (BSOD)"))
                {
                    baseSetupLog("Enable CrashOnCtrlScroll");
                    baseSetup += $"\r\n" + @"reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\kbdhid\Parameters"" /v CrashOnCtrlScroll /t REG_DWORD /d 1 /f";
                    baseSetup += $"\r\n" + @"reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\i8042prt\Parameters"" /v CrashOnCtrlScroll /t REG_DWORD /d 1 /f";
                }

                if (winBoxConfig.DynamicDaylightTimeDisabled == true)
                {
                    baseSetupLog("disable DynamicDaylightTimeDisabled");
                    baseSetup += $"\r\n" + @"reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\TimeZoneInformation"" /v DynamicDaylightTimeDisabled /t REG_DWORD /d 1 /f";
                }

                if (winBoxConfig.DisableNtp == true)
                {
                    baseSetupLog("disable NtpClient");
                    baseSetup += $"\r\n" + @"reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\TimeProviders\NtpClient"" /v Enabled /t REG_DWORD /d 0 /f";
                }

                if (winBoxConfig.RealTimeIsUniversal == true)
                {
                    baseSetupLog($"set RealTimeIsUniversal: {winBoxConfig.RealTimeIsUniversal}");
                    baseSetup += $"\r\n" + $@"reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\TimeProviders\RealTimeIsUniversal"" /v Enabled /t REG_DWORD /d {(winBoxConfig.RealTimeIsUniversal == true ? 1 : 0)} /f";
                }

                if (winBoxConfig.ChangeTimezone == true) {
                    baseSetupLog($"change time zone: {winBoxConfig.TimeZoneKeyName}");
                    baseSetup += $"\r\n" + $@"tzutil /s ""{winBoxConfig.TimeZoneKeyName}""";
                }

                if (winBoxConfig.UseCustomDisplaySettings == true)
                {
                    await CopyResource("ChangeResolution.ps1");
                    string args = "";
                    if (winBoxConfig.cds_width_use == true) args += $@"-Width ""{winBoxConfig.cds_width}"" ";
                    if (winBoxConfig.cds_height_use == true) args += $@"-Height ""{winBoxConfig.cds_height}"" ";
                    if (winBoxConfig.cds_bitDepth_use == true) args += $@"-BitDepth ""{winBoxConfig.cds_bitDepth}"" ";
                    if (winBoxConfig.cds_refreshRate_use == true) args += $@"-Refresh ""{winBoxConfig.cds_refreshRate}"" ";
                    if (winBoxConfig.cds_orientation_use == true) args += $@"-Orientation ""{winBoxConfig.cds_orientation}"" ";
                    string customDisplaySettingsCmd = $@"powershell -ExecutionPolicy Bypass -File ""C:\WinboxResources\ChangeResolution.ps1"" {args}";
                    applicationScript += $"\r\n" + customDisplaySettingsCmd;
                    baseSetupLog("Change Display Settings");
                    baseSetup += $"\r\n" + customDisplaySettingsCmd;
                }

                if (winBoxConfig.UseCustomDisplaySettings_scale == true)
                {
                    await CopyResource("ChangeScale.ps1");
                    string customDisplaySettingsCmd = $@"powershell -ExecutionPolicy Bypass -File ""C:\WinboxResources\ChangeScale.ps1"" -Scaling ""{winBoxConfig.cds_scaling}""";
                    applicationScript += $"\r\n" + customDisplaySettingsCmd;
                    baseSetupLog("Change Display Scale");
                    baseSetup += $"\r\n" + customDisplaySettingsCmd;
                }

                bool customBootLogo = winBoxConfig.CustomBootLogo != null && !winBoxConfig.CustomBootLogo.Contains("\"");
                string cursorPath = Path.Combine(resourcesDirectoryPath, "cursor");
                bool customCursor = Directory.Exists(cursorPath) && !Program.IsDirectoryEmpty(cursorPath);
                bool useWinboxService = winBoxConfig.UseEmbeddedDisplay == true;

                if (!Program.isTweakEnabled(winBoxConfig, "Do not disable hotkeys by changing the layout"))
                {
                    baseSetupLog("Disable hotkey by change keyboard layout");
                    baseSetup += "\r\n";
                    baseSetup += $@"reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Keyboard Layout"" /v ""Scancode Map"" /t REG_BINARY /d 000000000000000030000000000021e000006ce000006de0000011e000006be000003b0000004400000057000000580000006400000065000000660000006700000068000000690000006a0000003c0000006b0000006c0000006d0000006e0000006f0000003d0000003e0000003f0000004000000041000000420000004300000013e0000014e0000012e00000380000005be000005ee0000037e0000038e000005ce000005fe0000063e000006ae0000066e0000069e0000032e0000067e0000065e0000068e000000000 /f";
                }

                if (winBoxConfig.oemkey_slmgr == true && winBoxConfig.isValidOemKey())
                {
                    baseSetupLog("Apply OEM key");
                    baseSetup += $"\r\ncscript /B \"%windir%\\system32\\slmgr.vbs\" /ipk \"{winBoxConfig.OemKey}\"\ncscript /B \"%windir%\\system32\\slmgr.vbs\" /ato";
                }

                void regRedist(string name)
                {
                    baseSetupLog($"install vc redist {name}");
                    baseSetup += $"\r\nstart /wait C:\\WinboxResources\\{name} /install /quiet /norestart";
                }

                bool compatibleVcRedist = Program.isTweakEnabled(winBoxConfig, "Integrate vc redist (compatible architectures)");
                if (compatibleVcRedist || Program.isTweakEnabled(winBoxConfig, "Integrate vc redist"))
                {
                    if (compatibleVcRedist)
                    {
                        List<string> archs = new List<string>();
                        switch (winBoxConfig.Architecture)
                        {
                            case "x64":
                                archs.Add("x64");
                                archs.Add("x86");
                                break;

                            case "x86":
                                archs.Add("x86");
                                break;

                            case "arm64":
                                archs.Add("arm64");
                                archs.Add("x64");
                                archs.Add("x86");
                                break;
                        }
                        foreach (string arch in archs)
                        {
                            string newName = arch + "_vc_redist.exe";
                            await CopyBlobFromArchWithRename("vc_redist.exe", newName, arch);
                            regRedist(newName);
                        }
                    }
                    else
                    {
                        await CopyBlob("vc_redist.exe");
                        regRedist("vc_redist.exe");
                    }
                }

                if (Program.isTweakEnabled(winBoxConfig, "Integrate nircmd"))
                {
                    await CopyBlob("nircmd.exe", "executable");
                    await CopyBlob("nircmdc.exe", "executable");
                }

                void regNetFramework(string name)
                {
                    baseSetupLog($"install net framework {name}");
                    baseSetup += $"\r\nstart /wait C:\\WinboxResources\\{name} /q";
                }

                if (Program.isTweakEnabled(winBoxConfig, "Integrate net 4.8.1"))
                {
                    await CopyBlob("net481.exe");
                    regNetFramework("net481.exe");
                }

                if (Program.isTweakEnabled(winBoxConfig, "Integrate net 4.7.2") || customBootLogo)
                {
                    await CopyBlob("net472.exe");
                    regNetFramework("net472.exe");
                }

                void regNet(string name)
                {
                    baseSetupLog($"install net {name}");
                    baseSetup += $"\r\nstart /wait C:\\WinboxResources\\{name} /quiet /norestart";
                }

                if (Program.isTweakEnabled(winBoxConfig, "Integrate net 8.0.17") || useWinboxService)
                {
                    await CopyBlob("net8017.exe");
                    regNet("net8017.exe");
                }

                if (Program.isTweakEnabled(winBoxConfig, "Integrate net 9.0.6"))
                {
                    await CopyBlob("net906.exe");
                    regNet("net906.exe");
                }

                if (Program.isTweakEnabled(winBoxConfig, "Integrate app runtime 1.7.3"))
                {
                    await CopyBlob("appruntime173.exe");
                    regAppScriptFirstInitCmd("appruntime173", "C:\\WinboxResources\\appruntime173.exe");
                }

                async Task addCustomInstallers(string baseDir)
                {
                    async Task copyCustomInstaller(string path, string name)
                    {
                        string dir = Path.Combine(WinboxResourcesPath, "CustomInstallers");
                        Program.CreateDirectory(dir);
                        await Program.CopyFileAsync(path, Path.Combine(dir, name));
                    }

                    string installersDir = Path.Combine(baseDir, "vc_redist");
                    if (Directory.Exists(installersDir))
                    {
                        string[] files = Directory.GetFiles(installersDir);
                        int index = 1;
                        foreach (string file in files)
                        {
                            string installerName = $"vc_redist_{index++}.exe";
                            await copyCustomInstaller(file, installerName);
                            regRedist("CustomInstallers\\" + installerName);
                        }
                    }

                    installersDir = Path.Combine(baseDir, "net_framework");
                    if (Directory.Exists(installersDir))
                    {
                        string[] files = Directory.GetFiles(installersDir);
                        int index = 1;
                        foreach (string file in files)
                        {
                            string installerName = $"net_framework_{index++}.exe";
                            await copyCustomInstaller(file, installerName);
                            regNetFramework("CustomInstallers\\" + installerName);
                        }
                    }

                    installersDir = Path.Combine(baseDir, "net");
                    if (Directory.Exists(installersDir))
                    {
                        string[] files = Directory.GetFiles(installersDir);
                        int index = 1;
                        foreach (string file in files)
                        {
                            string installerName = $"net_{index++}.exe";
                            await copyCustomInstaller(file, installerName);
                            regNet("CustomInstallers\\" + installerName);
                        }
                    }

                    installersDir = Path.Combine(baseDir, "app_runtime");
                    if (Directory.Exists(installersDir))
                    {
                        string[] files = Directory.GetFiles(installersDir);
                        int index = 1;
                        foreach (string file in files)
                        {
                            string installerName = $"app_runtime_{index}.exe";
                            await copyCustomInstaller(file, installerName);
                            regAppScriptFirstInitCmd($"custom_app_runtime_{index++}", $"C:\\WinboxResources\\CustomInstallers\\{installerName}");
                        }
                    }
                }

                await addCustomInstallers(resourcesDirectoryPath);
                await addCustomInstallers(tempDirectoryPath);

                if (Program.isTweakEnabled(winBoxConfig, "Integrate microsoft edge") || winBoxConfig.ProgramType == ProgramTypeEnum.WebSite)
                {
                    await CopyBlob("MicrosoftEdge.msi");
                    baseSetupLog($"install Microsoft edge");
                    baseSetup += $"\r\nstart /wait msiexec /i C:\\WinboxResources\\MicrosoftEdge.msi /quiet /norestart";
                }

                if (Program.isTweakEnabled(winBoxConfig, "Hide Cursor"))
                {
                    await CopyResource("empty.cur");
                    await CopyResource("hide_cursor.reg");
                    string regCmd = "regedit /s \"C:\\WinboxResources\\hide_cursor.reg\"";
                    baseSetupLog($"hide cursor");
                    baseSetup += $"\r\n" + regCmd;
                    regAppScriptFirstInitCmd("hide_cursor", regCmd);
                    await OverwriteSystemCursorEmpty(Path.Combine(wimMountPath, "Windows", "Cursors"));
                }
                else if (customCursor)
                {
                    await Program.CopyFilesRecursivelyAsync(cursorPath, Path.Combine(WinboxResourcesPath, "cursor"));
                    await CopyResource("custom_cursor.reg");
                    string regCmd = "regedit /s \"C:\\WinboxResources\\custom_cursor.reg\"";
                    baseSetupLog($"custom cursor");
                    baseSetup += $"\r\n" + regCmd;
                    regAppScriptFirstInitCmd("custom_cursor", regCmd);
                    await OverwriteSystemCursorEmpty(Path.Combine(wimMountPath, "Windows", "Cursors"));
                }

                if (Program.isTweakEnabled(winBoxConfig, "Hide Touchscreen Visualization"))
                {
                    await CopyResource("hide_touch.reg");
                    string regCmd = "regedit /s \"C:\\WinboxResources\\hide_touch.reg\"";
                    baseSetupLog($"hide touchscreen visualization");
                    baseSetup += $"\r\n" + regCmd;
                    regAppScriptFirstInitCmd("hide_touch", regCmd);
                }

                string bootresDllPath = Path.Combine(wimMountPath, "Windows\\Boot\\Resources\\bootres.dll");

                if (winBoxConfig.bootresRepacking_logoPath != null && winBoxConfig.CustomBootLogo_UseOnBootres != true)
                {
                    string logoPath = Path.Combine(resourcesDirectoryPath, winBoxConfig.bootresRepacking_logoPath);
                    if (File.Exists(logoPath))
                    {
                        BootresPatcher.PatchBootres(bootresDllPath, logoPath);
                    }
                }

                if (customBootLogo)
                {
                    string logoPath = Path.Combine(resourcesDirectoryPath, winBoxConfig.CustomBootLogo);
                    if (File.Exists(logoPath))
                    {
                        if (winBoxConfig.CustomBootLogo_UseOnBootres == true)
                        {
                            BootresPatcher.PatchBootres(bootresDllPath, logoPath);
                        }

                        await UnpackBlob("HackBGRT.zip");

                        string splashBootLogoPath = Path.Combine(WinboxResourcesPath, "HackBGRT-2.5.2", "splash.bmp");
                        ImageConverter.ConvertToBmp_54_24(logoPath, splashBootLogoPath);
                        await copyToDebugFile("logo.bmp", splashBootLogoPath);

                        string configBootLogoPath = Program.ResourcePath(Path.Combine("resources", winBoxConfig.CustomBootLogo_centering == true ? "hackBGRT_centering.txt" : "hackBGRT.txt"));
                        await Program.CopyFileAsync(configBootLogoPath, Path.Combine(WinboxResourcesPath, "HackBGRT-2.5.2", "config.txt"));

                        string hackBGRT = "cd C:\\WinboxResources\\HackBGRT-2.5.2\r\nC:\\WinboxResources\\HackBGRT-2.5.2\\setup.exe batch install allow-secure-boot allow-bitlocker allow-bad-loader enable-overwrite enable-bcdedit";
                        baseSetupLog($"apply HackBGRT");
                        baseSetup += "\r\n" + hackBGRT;

                        regAppScriptFirstInitCmd("hackBGRT", hackBGRT);
                    }
                }

                if (Program.isTweakEnabled(winBoxConfig, "Integrate PSTools"))
                {
                    await UnpackBlob("PSTools.zip", "executable");
                }

                baseSetup += "\r\ncd C:\\";
                applicationScript += "\r\ncd C:\\";

                if (winBoxConfig.PostInstall_reg != null)
                {
                    string regPath = Path.Combine(resourcesDirectoryPath, winBoxConfig.PostInstall_reg);
                    if (File.Exists(regPath))
                    {
                        await Program.CopyFileAsync(regPath, Path.Combine(WinboxResourcesPath, "postinstall.reg"));
                        baseSetupLog($"run postinstall.reg");
                        baseSetup += $"\r\nregedit /s \"C:\\WinboxResources\\postinstall.reg\"";
                    }
                }

                if (winBoxConfig.PostInstall_bat != null)
                {
                    string batPath = Path.Combine(resourcesDirectoryPath, winBoxConfig.PostInstall_bat);
                    if (File.Exists(batPath))
                    {
                        await Program.CopyFileAsync(batPath, Path.Combine(WinboxResourcesPath, "postinstall.bat"));
                        baseSetupLog($"run postinstall.bat");
                        baseSetup += $"\r\ncall \"C:\\WinboxResources\\postinstall.bat\"";
                    }
                }

                if (winBoxConfig.PostInstall_user_reg != null)
                {
                    string regPath = Path.Combine(resourcesDirectoryPath, winBoxConfig.PostInstall_user_reg);
                    if (File.Exists(regPath))
                    {
                        await Program.CopyFileAsync(regPath, Path.Combine(WinboxResourcesPath, "postinstall_user.reg"));
                        regAppScriptFirstInitCmd("postinstall_reg", $"regedit /s \"C:\\WinboxResources\\postinstall_user.reg\"");
                    }
                }

                if (winBoxConfig.PostInstall_user_bat != null)
                {
                    string batPath = Path.Combine(resourcesDirectoryPath, winBoxConfig.PostInstall_user_bat);
                    if (File.Exists(batPath))
                    {
                        await Program.CopyFileAsync(batPath, Path.Combine(WinboxResourcesPath, "postinstall_user.bat"));
                        regAppScriptFirstInitCmd("postinstall_bat", $"call \"C:\\WinboxResources\\postinstall_user.bat\"");
                    }
                }

                if (winBoxConfig.AddVirtualDisplay == true)
                {
                    await UnpackBlob("usbmmidd_v2.zip");
                    await CopyResource("usbmmidd_v2\\install_driver.bat");
                    await CopyResource("usbmmidd_v2\\add_display.bat");
                    await WriteHiddenBatExecuter(Path.Combine(WinboxResourcesPath, "run_add_display_hidden.vbs"), "C:\\WinboxResources\\usbmmidd_v2\\add_display.bat", null);
                    string regStr = $"\r\nreg add \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\WUDF\\Services\\usbmmIdd\\Parameters\\Monitors\" /v 0 /t REG_SZ /d \"{winBoxConfig.VirtualDisplayWidth},{winBoxConfig.VirtualDisplayHeight}\" /f";
                    string installDriver = $"\r\ncall C:\\WinboxResources\\usbmmidd_v2\\install_driver.bat";
                    string addDisplay = $"\r\ncall C:\\WinboxResources\\usbmmidd_v2\\add_display.bat";
                    applicationScript += $"\r\ntimeout /t 2";
                    applicationScript += regStr;
                    applicationScript += installDriver;
                    applicationScript += regStr;
                    applicationScript += addDisplay;
                }

                if (useWinboxService)
                {
                    //await Program.CopyFilesRecursivelyAsync(AppDomain.CurrentDomain.BaseDirectory, Path.Combine(WinboxResourcesPath, "winbox_maker"));
                }

                if (winBoxConfig.UseEmbeddedDisplay == true)
                {
                    //applicationScript += $"\r\nstart /B \"\" C:\\WinboxResources\\winbox_maker\\WinBox-Maker.exe";
                }

                if (winBoxConfig.firstBootAction == FirstBootActionEnum.generalize ||
                    (initViaVmMode && winBoxConfig.img_shutdownAfterInstall == true && winBoxConfig.img_generalizeAfterInstall == true))
                {
                    string architecture = winBoxConfig.Architecture;
                    if (architecture == "x64") architecture = "amd64";

                    string sysprep_unattend = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<unattend xmlns=""urn:schemas-microsoft-com:unattend"">
    <settings pass=""oobeSystem"">
        <component name=""Microsoft-Windows-Shell-Setup"" processorArchitecture=""{architecture}"" publicKeyToken=""31bf3856ad364e35"" language=""neutral"" versionScope=""nonSxS"" xmlns:wcm=""http://schemas.microsoft.com/WMIConfig/2002/State"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
            <OOBE>
                <HideEULAPage>true</HideEULAPage>
                <ProtectYourPC>3</ProtectYourPC>
                <SkipMachineOOBE>true</SkipMachineOOBE>
                <SkipUserOOBE>true</SkipUserOOBE>
            </OOBE>
        </component>
    </settings>
</unattend>";

                    await File.WriteAllTextAsync(Path.Combine(WinboxResourcesPath, "sysprep_unattend.xml"), sysprep_unattend);
                    await writeDebugFile("sysprep_unattend.xml", sysprep_unattend, false);
                }

                string sysprepCmd = "C:\\Windows\\System32\\Sysprep\\sysprep.exe /quiet /generalize /oobe /shutdown /unattend:C:\\WinboxResources\\sysprep_unattend.xml";

                if (initViaVmMode && winBoxConfig.img_shutdownAfterInstall == true)
                {
                    string firstBootShutdown = "\r\npause";

                    if (winBoxConfig.img_generalizeAfterInstall == true)
                    {
                        firstBootShutdown = sysprepCmd + firstBootShutdown;
                    }
                    else
                    {
                        firstBootShutdown = "shutdown /s /t 0" + firstBootShutdown;
                    }

                    if (winBoxConfig.img_runningPostinstallOnFirstRealStartup == true)
                    {
                        string[] deleteAfterVm =
                        {
                            "C:\\WinboxResources\\firstInit1.installed",
                            "C:\\WinboxResources\\firstInit2.installed",
                            "C:\\WinboxResources\\hackBGRT.installed",
                            "C:\\WinboxResources\\postinstall_reg.installed",
                            "C:\\WinboxResources\\postinstall_bat.installed"
                        };

                        foreach (string path in deleteAfterVm)
                        {
                            firstBootShutdown = $"del /F /Q \"{path}\"\r\n" + firstBootShutdown;
                        }
                    }

                    regAppScriptFirstInitCmd("firstBootShutdown", firstBootShutdown, true);
                }

                switch (winBoxConfig.firstBootAction)
                {
                    case FirstBootActionEnum.reboot:
                        regAppScriptFirstInitCmd("firstBootAction", "shutdown /r /t 0\r\npause", true);
                        break;

                    case FirstBootActionEnum.shutdown:
                        regAppScriptFirstInitCmd("firstBootAction", "shutdown /s /t 0\r\npause", true);
                        break;

                    case FirstBootActionEnum.hibernate:
                        regAppScriptFirstInitCmd("firstBootAction", "shutdown /h /t 0\r\npause", true);
                        break;

                    case FirstBootActionEnum.reboot_to_desktop:
                        regAppScriptFirstInitCmd("firstBootAction", reboot_to_desktop_cmd, true);
                        break;

                    case FirstBootActionEnum.generalize:
                        regAppScriptFirstInitCmd("firstBootAction", sysprepCmd + "\r\npause", true);
                        break;
                }

                baseSetupLog($"unload DEFAULT_USER");

                baseSetup += $"\r\n";
                baseSetup += @$"reg unload HKLM\DEFAULT_USER

echo SetupComplete - creating a user >> C:\WinboxResources\setup.log
net user winbox /add

echo SetupComplete - PasswordExpires=False >> C:\WinboxResources\setup.log
wmic useraccount where ""Name='winbox'"" set PasswordExpires=False

echo SetupComplete - making the user an administrator >> C:\WinboxResources\setup.log
net localgroup Administrators winbox /add";

                baseSetupLog($"end");

                await writeDebugFile("UpdateSystemSettings", updateSystemSettings);
                await writeDebugFile("SetupComplete", baseSetup);

                await File.WriteAllTextAsync(Path.Combine(WinboxResourcesPath, "UpdateSystemSettings.bat"), updateSystemSettings);
                await File.WriteAllTextAsync(Path.Combine(WindowsScriptsPath, "SetupComplete.cmd"), baseSetup);
            }

            // ------------------------------------ copy program files
            if (!manual)
            {
                string programPath = Path.Combine(resourcesDirectoryPath, "program");
                if (Directory.Exists(programPath))
                {
                    await Program.CopyFilesRecursivelyAsync(programPath, Path.Combine(wimMountPath, "WinboxProgram"));
                }

                if (Directory.Exists(tempProgramPath))
                {
                    await Program.CopyFilesRecursivelyAsync(tempProgramPath, Path.Combine(wimMountPath, "WinboxProgram"));
                }
            }

            // ------------------------------------ copy files

            if (manual)
            {
                if (winBoxConfig.manual_setup_complete != null)
                {
                    string batPath = Path.Combine(resourcesDirectoryPath, winBoxConfig.manual_setup_complete);
                    if (File.Exists(batPath))
                    {
                        Directory.CreateDirectory(WindowsScriptsPath);
                        await Program.CopyFileAsync(batPath, Path.Combine(WindowsScriptsPath, "SetupComplete.cmd"));
                    }
                }

                if (winBoxConfig.manual_setup_error != null)
                {
                    string batPath = Path.Combine(resourcesDirectoryPath, winBoxConfig.manual_setup_error);
                    if (File.Exists(batPath))
                    {
                        Directory.CreateDirectory(WindowsScriptsPath);
                        await Program.CopyFileAsync(batPath, Path.Combine(WindowsScriptsPath, "ErrorHandler.cmd"));
                    }
                }

                if (winBoxConfig.manual_setup_sysunattend != null)
                {
                    string xmlPath = Path.Combine(resourcesDirectoryPath, winBoxConfig.manual_setup_sysunattend);
                    if (File.Exists(xmlPath))
                    {
                        await Program.CopyFileAsync(xmlPath, Path.Combine(wimMountPath, "Windows", "Panther", "unattend.xml"));
                    }
                }
            }

            string filesPath = Path.Combine(resourcesDirectoryPath, "files");
            if (Directory.Exists(filesPath))
            {
                await Program.CopyFilesRecursivelyAsync(filesPath, wimMountPath);
            }

            filesPath = Path.Combine(tempDirectoryPath, "files");
            if (Directory.Exists(filesPath))
            {
                await Program.CopyFilesRecursivelyAsync(filesPath, wimMountPath);
            }

            await addAdFiles(wimMountPath, newWindowsDescription, winBoxConfig.aaf_readme_system, winBoxConfig.aaf_info_system);

            // ------------------------------------ setup application autorun
            if (!manual) {
                string? command = null;
                switch (winBoxConfig.ProgramType)
                {
                    case ProgramTypeEnum.ExecutableFile:
                        {
                            applicationScript += "\r\ncd C:\\WinboxProgram";
                            string execFilePath = @$"C:\WinboxProgram\{winBoxConfig.ProgramName}";
                            string extension = Path.GetExtension(winBoxConfig.ProgramName);
                            if (extension != null && (extension.Equals(".bat", StringComparison.OrdinalIgnoreCase) ||
                                extension.Equals(".cmd", StringComparison.OrdinalIgnoreCase)))
                            {
                                //await WriteHiddenBatExecuter(Path.Combine(WinboxResourcesPath, "run_user_script_hidden.vbs"), execFilePath, winBoxConfig.ProgramArgs);
                                //command = "wscript \"C:\\WinboxResources\\run_user_script_hidden.vbs\"";
                                command = "call \"" + execFilePath + "\"";
                            }
                            else
                            {
                                command = "start \"\" /wait \"" + execFilePath + "\"";
                            }

                            if (winBoxConfig.ProgramArgs != null && winBoxConfig.ProgramArgs.Length > 0)
                            {
                                command += " " + winBoxConfig.ProgramArgs;
                            }
                        }
                        break;

                    case ProgramTypeEnum.RawCommand:
                        if (winBoxConfig.RawCommand != null) {
                            applicationScript += "\r\ncd C:\\WinboxProgram";
                            command = winBoxConfig.RawCommand;
                        }
                        break;

                    case ProgramTypeEnum.WebSite:
                        if (winBoxConfig.WebSite != null && !winBoxConfig.WebSite.Contains("\""))
                        {
                            string execFilePath = @"C:\WinboxResources\run_edge.bat";
                            string batFile = $@"@echo off

set ""edgePath1=C:\WinboxResources\edge\msedge.exe""
set ""edgePath2=C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe""
set ""edgePath3=C:\Program Files\Microsoft\Edge\Application\msedge.exe""
set ""edgePath4=C:\Program Files (x86)\Microsoft\Edge Beta\Application\msedge.exe""
set ""edgePath5=C:\Program Files\Microsoft\Edge Beta\Application\msedge.exe""

set ""msedgePath=""
if exist ""%edgePath1%"" (
    set ""msedgePath=%edgePath1%""
) else (
    if exist ""%edgePath2%"" (
        set ""msedgePath=%edgePath2%""
    ) else (
        if exist ""%edgePath3%"" (
            set ""msedgePath=%edgePath3%""
        ) else (
            if exist ""%edgePath4%"" (
                set ""msedgePath=%edgePath4%""
            ) else (
                if exist ""%edgePath5%"" (
                    set ""msedgePath=%edgePath5%""
                )
            )
        )
    )
)

if ""%msedgePath%""=="""" (
    powershell -Command ""Add-Type -AssemblyName System.Windows.Forms; [System.Windows.Forms.MessageBox]::Show('Microsoft Edge not found. please check the winbox configuration','Winbox Broken', [System.Windows.Forms.MessageBoxButtons]::OK, [System.Windows.Forms.MessageBoxIcon]::Error)""
    exit /b
)

start "" /wait ""%msedgePath%"" --kiosk ""{winBoxConfig.WebSite}"" --edge-kiosk-type=fullscreen --kiosk-idle-timeout-minutes={winBoxConfig.WebSessionTimeout} --no-first-run";

                            await writeDebugFile("RunEdge", applicationScript);
                            await File.WriteAllTextAsync(Path.Combine(WinboxResourcesPath, "run_edge.bat"), batFile);

                            //await WriteHiddenBatExecuter(Path.Combine(WinboxResourcesPath, "run_edge_script_hidden.vbs"), execFilePath, null);
                            //command = "wscript \"C:\\WinboxResources\\run_edge_script_hidden.vbs\"";
                            command = "call \"" + execFilePath + "\"";
                        }
                        break;

                    case ProgramTypeEnum.None:
                        break;
                }

                {
                    string? before_app_logo = null;
                    string baseCmd = $@"powershell -ExecutionPolicy Bypass -File ""C:\WinboxResources\show_image.ps1"" ";
                    if (winBoxConfig.CustomBootLogo_UseLogoBeforeApp == true)
                    {
                        if (winBoxConfig.CustomBootLogo != null)
                        {
                            string logoPath = Path.Combine(resourcesDirectoryPath, winBoxConfig.CustomBootLogo);
                            if (File.Exists(logoPath))
                            {
                                await CopyResource("show_image.ps1");
                                string beforeAppLogoPath = Path.Combine(WinboxResourcesPath, "before_app.bmp");
                                ImageConverter.ConvertToBmp_54_24(logoPath, beforeAppLogoPath);
                                await copyToDebugFile("before_app.bmp", beforeAppLogoPath);
                            }
                            before_app_logo = baseCmd + $@"-path ""C:\WinboxResources\before_app.bmp"" -stretch None -offsetX 0 -offsetY {(winBoxConfig.CustomBootLogo_centering == true ? "0" : "-200")}";
                        }
                    }
                    else if (winBoxConfig.logoBeforeApp != null)
                    {
                        await CopyResource("show_image.ps1");
                        string filename = "before_app" + Path.GetExtension(winBoxConfig.logoBeforeApp);
                        string beforeAppLogoPath = Path.Combine(WinboxResourcesPath, filename);
                        File.Copy(winBoxConfig.logoBeforeApp, beforeAppLogoPath, true);
                        await copyToDebugFile(filename, beforeAppLogoPath);
                        before_app_logo = baseCmd + $@"-path ""C:\WinboxResources\{filename}"" -stretch {winBoxConfig.logoBeforeApp_stretch.ToString()}";
                    }

                    if (before_app_logo != null)
                    {
                        await File.WriteAllTextAsync(Path.Combine(WinboxResourcesPath, "before_app_logo.bat"), before_app_logo);
                        await WriteHiddenBatExecuter(Path.Combine(WinboxResourcesPath, "run_before_app_logo_hidden.vbs"), @"C:\WinboxResources\before_app_logo.bat", null);

                        applicationScript += "\r\n";
                        applicationScript += @"set SHOW_IMAGE_FLAG_FILE=C:\WinboxResources\show_image.flag" + "\r\n";
                        applicationScript += @"del /f ""%SHOW_IMAGE_FLAG_FILE%""" + "\r\n";
                        applicationScript += "wscript \"C:\\WinboxResources\\run_before_app_logo_hidden.vbs\"" + "\r\n";

                        if (winBoxConfig.wait_before_app_logo == true)
                        {
                            applicationScript += @":wait_show_image
if exist ""%SHOW_IMAGE_FLAG_FILE%"" goto continue_show_image
timeout /t 0 /nobreak >nul
goto wait_show_image

:continue_show_image
del /f ""%SHOW_IMAGE_FLAG_FILE%""";
                        }

                        applicationScript += "\r\n";
                    }
                }

                applicationScript += "\r\n:restart_app";

                if (winBoxConfig.appdelay_time == true)
                {
                    applicationScript += $"\r\ntimeout /t {winBoxConfig.appdelay_time_value} /nobreak";
                }

                if (winBoxConfig.appdelay_internet == true)
                {
                    applicationScript += $"\r\n" + $@":wait_internet
ping -n 1 ""{winBoxConfig.appdelay_internet_checkurl}"" >nul 2>&1
if errorlevel 1 (
    timeout /t {winBoxConfig.appdelay_internet_requestdelay} >nul
    goto wait_internet
)" + $"\r\n";
                }

                if (command != null)
                {
                    applicationScript += "\r\n" + command;
                }

                if (winBoxConfig.appcrash_time == true)
                {
                    applicationScript += $"\r\ntimeout /t {winBoxConfig.appcrash_time_value} /nobreak";
                }

                switch (winBoxConfig.actionAtEndOfApplication)
                {
                    case ActionAtEndOfApplication.none:
                        break;

                    case ActionAtEndOfApplication.restart_app:
                        applicationScript += "\r\ngoto restart_app";
                        break;

                    case ActionAtEndOfApplication.reboot_computer:
                        applicationScript += "\r\nshutdown /r /t 0";
                        break;

                    case ActionAtEndOfApplication.shutdown_computer:
                        applicationScript += "\r\nshutdown /s /t 0";
                        break;

                    case ActionAtEndOfApplication.execute_command:
                        applicationScript += winBoxConfig.actionAtEndOfApplication_command;
                        break;
                }

                Program.CreateDirectory(Path.Combine(
                    wimMountPath,
                    "ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\StartUp"
                ));

                await writeDebugFile("AppScript", applicationScript);
                await File.WriteAllTextAsync(Path.Combine(WinboxResourcesPath, "app_script.bat"), applicationScript);

                if (winBoxConfig.LaunchMode == ProgramLaunchModeEnum.afterDesktop)
                {
                    await WriteHiddenBatExecuter(
                        Path.Combine(
                            wimMountPath,
                            "ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\StartUp\\run_app_script_hidden.vbs"
                        ),
                        @"C:\WinboxResources\app_script.bat",
                        null
                    );
                }
                else
                {
                    await WriteApiScript("reboot_to_desktop.bat", reboot_to_desktop_cmd);

                    string customShell = "wscript \"C:\\WinboxResources\\run_app_script_hidden.vbs\"";

                    await File.WriteAllTextAsync(Path.Combine(WinboxResourcesPath, "return_the_shell.bat"), $"reg add \"HKLM\\Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\" /v Shell /t REG_SZ /d \"{customShell.Replace("\"", "\\\"")}\" /f");

                    await WriteHiddenBatExecuter(
                        Path.Combine(
                            wimMountPath,
                            "ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\StartUp\\run_return_the_shell_hidden.vbs"
                        ),
                        @"C:\WinboxResources\return_the_shell.bat",
                        null
                    );

                    await WriteHiddenBatExecuter(Path.Combine(WinboxResourcesPath, "run_app_script_hidden.vbs"), @"C:\WinboxResources\app_script.bat", null);
                    await RegChanger.RegMod("SOFTWARE", "Microsoft\\Windows NT\\CurrentVersion\\Winlogon", "Shell", Program.EscapeForRegFile(customShell));
                }

                await WriteApiScript("reboot_to_recovery.bat", "reagentc /boottore\r\nshutdown /r /t 0");
                await WriteApiScript("reboot_to_advanced_options.bat", "shutdown /r /o /f /t 0");
            }

            // ------------------------------------ apple reg
            if (modSystemReg)
            {
                if (winBoxConfig.onbuild_reg != null)
                {
                    string oldRegPath = Path.Combine(resourcesDirectoryPath, winBoxConfig.onbuild_reg);
                    string newRegPath = Path.Combine(tempDirectoryPath, "modified_reg.reg");
                    await RegPatcher.regPatcher(oldRegPath, newRegPath);
                    copyToDebugFile("modified_reg.txt", newRegPath);
                    await Program.ExecuteAsync("reg.exe", $"import \"{newRegPath}\"", null, debugFolder);
                    File.Delete(newRegPath);
                }

                await RegChanger.umountReg();
                //await Program.ExecuteAsync("reg.exe", $"unload HKLM\\WINBOX_SYSTEM");
            }

            // ------------------------------------ install drivers & setup features

            async Task addUserDrivers(string baseDir)
            {
                string driversPath = Path.Combine(baseDir, "drivers");
                if (Program.IsDirectoryNotEmpty(driversPath))
                {
                    processName("Installing user drivers");
                    processValue(55);
                    await Program.ExecuteAsync("dism.exe", $"/image:\"{wimMountPath}\" /add-driver /driver:\"{driversPath}\" /recurse /forceunsigned", null, debugFolder);
                }
            }

            async Task addCabMsu(string baseDir)
            {
                string packagesPath = Path.Combine(baseDir, "packages");
                if (Program.IsDirectoryNotEmpty(packagesPath))
                {
                    processName("Installing .cab/.msu packages");
                    processValue(56);
                    await Program.ExecuteAsync("dism.exe", $"/image:\"{wimMountPath}\" /add-package /PackagePath:\"{packagesPath}\"", null, debugFolder);
                }
            }

            await addUserDrivers(resourcesDirectoryPath);
            await addUserDrivers(tempDirectoryPath);
            await addCabMsu(resourcesDirectoryPath);
            await addCabMsu(tempDirectoryPath);

            if (winBoxConfig.forceIot == true)
            {
                processName("Change edition");
                processValue(57);
                await Program.ExecuteAsync("dism.exe", $"/image:\"{wimMountPath}\" /Set-Edition:IoTEnterprise /accepteula", null, debugFolder);
            }

            if (winBoxConfig.customdism_enabled == true)
            {
                processName("Applying custom dism commands");
                processValue(58);
                foreach (string command in splitRickTextboxLinesWithoutEmptyLines(winBoxConfig.customdism_commands ?? ""))
                {
                    await Program.ExecuteAsync("dism.exe", $"/image:\"{wimMountPath}\" {command}", baseDirectoryPath, debugFolder);
                }
            }

            processName("Enabling necessary windows components");
            processValue(59);
            foreach (string feature in getEnableFeatures())
            {
                await Program.ExecuteAsync("dism.exe", $"/image:\"{wimMountPath}\" /Enable-Feature /all /FeatureName:\"{feature}\"", baseDirectoryPath, debugFolder);
            }

            if (!manual)
            {
                processName("disabling unnecessary Windows components");
                processValue(60);
                await Program.ExecuteAsync("dism.exe", $"/image:\"{wimMountPath}\" /disable-feature /remove /featurename:Windows-Defender", null, debugFolder); //it will probably only work for Windows server
                await Program.ExecuteAsync("dism.exe", $"/image:\"{wimMountPath}\" /disable-feature /remove /featurename:Windows-Defender-GUI", null, debugFolder);

                processName("OEM key applying");
                processValue(61);
                if (winBoxConfig.oemkey_dism == true && winBoxConfig.isValidOemKey())
                {
                    await Program.ExecuteAsync("dism.exe", $"/image:\"{wimMountPath}\" /Set-ProductKey:\"{winBoxConfig.OemKey}\"", null, debugFolder);
                }
            }

            // ------------------------------------ removing excess

            processName("Deleting unnecessary content");
            processValue(62);

            string[]? fullPackagesNames = null;
            string[]? fullProvisionedPackagesNames = null;

            string RemoveDism_log = "";

            async Task execDismCmd(string name, int type)
            {
                switch (type)
                {
                    case 0:
                        {
                            bool removeFlag = true; //remove default
                            if (name.StartsWith("!"))
                            {
                                name = name.Substring(1);
                                removeFlag = false;
                            }
                            string cmd = $"/image:\"{wimMountPath}\" /disable-feature /featurename:\"{name}\"";
                            if (removeFlag)
                            {
                                cmd += " /Remove";
                            }
                            await Program.ExecuteAsync("dism.exe", cmd, null, debugFolder);
                            break;
                        }

                    case 1:
                        await Program.ExecuteAsync("dism.exe", $"/image:\"{wimMountPath}\" /Remove-Package /PackageName:\"{name}\"", null, debugFolder);
                        break;

                    case 2:
                        await Program.ExecuteAsync("dism.exe", $"/image:\"{wimMountPath}\" /Remove-ProvisionedAppxPackage /PackageName:\"{name}\"", null, debugFolder);
                        break;
                }
            }

            async Task<string[]> getLocalFullPackagesNames(string name, bool provisionPackage=false)
            {
                if (name.StartsWith("*"))
                {
                    name = name.Substring(1);

                    string[]? packagesNames = null;
                    if (provisionPackage)
                    {
                        packagesNames = fullProvisionedPackagesNames;
                    }
                    else
                    {
                        packagesNames = fullPackagesNames;
                    }

                    if (packagesNames == null)
                    {
                        RemoveDism_log += $"get image packages names: provisionPackage: {provisionPackage}\r\n";
                        packagesNames = await getImagePackagesList(provisionPackage);
                        if (provisionPackage)
                        {
                            fullProvisionedPackagesNames = packagesNames;
                        }
                        else
                        {
                            fullPackagesNames = packagesNames;
                        }
                    }

                    RemoveDism_log += $"find full packages names: {name} | provisionPackage: {provisionPackage} | ({string.Join(",", packagesNames)})\r\n";
                    return getFullPackagesNames(packagesNames, name);
                }

                RemoveDism_log += $"stub full packages names: {name} | provisionPackage: {provisionPackage}\r\n";
                return [name];
            }

            async Task executeDismPackageDelete(string name, bool provisionPackage = false)
            {
                RemoveDism_log += $"multi delete start: {name} | provisionPackage: {provisionPackage}\r\n";
                string[] packagesToDelete = await getLocalFullPackagesNames(name, provisionPackage);
                foreach (string packageName in packagesToDelete) {
                    RemoveDism_log += $"mdelete: {packageName}\r\n";
                    await execDismCmd(packageName, provisionPackage ? 2 : 1);
                }
                RemoveDism_log += $"multi delete end\r\n";
            }

            foreach (string name in splitRickTextboxLinesWithoutEmptyLines(winBoxConfig.delete_dism_universal ?? ""))
            {
                RemoveDism_log += $"universal delete request: {name}\r\n";
                await execDismCmd(name, 0);
                await executeDismPackageDelete(name, false);
                await executeDismPackageDelete(name, true);
            }

            foreach (string name in splitRickTextboxLinesWithoutEmptyLines(winBoxConfig.delete_dism ?? ""))
            {
                RemoveDism_log += $"disable-feature request: {name}\r\n";
                await execDismCmd(name, 0);
            }

            foreach (string name in splitRickTextboxLinesWithoutEmptyLines(winBoxConfig.delete_dism_remove_package ?? ""))
            {
                RemoveDism_log += $"Remove-Package request: {name}\r\n";
                await executeDismPackageDelete(name, false);
            }

            foreach (string name in splitRickTextboxLinesWithoutEmptyLines(winBoxConfig.delete_dism_remove_appx_package ?? ""))
            {
                RemoveDism_log += $"Remove-ProvisionedAppxPackage request: {name}\r\n";
                await executeDismPackageDelete(name, true);
            }

            if (!manual)
            {
                if (Program.isTweakEnabled(winBoxConfig, "completely remove explorer.exe"))
                {
                    await removeSystemObject("Windows\\explorer.exe");
                }

                if (Program.isTweakEnabled(winBoxConfig, "completely remove system audio/images"))
                {
                    await removeSystemObject("Windows\\Web");
                    await removeSystemObject("Windows\\Media");
                }

                if (Program.isTweakEnabled(winBoxConfig, "removal of the subsystem SysWOW64"))
                {
                    await removeSystemObject("Windows\\SysWOW64");
                }

                if (Program.isTweakEnabled(winBoxConfig, "removing Windows/System apps (breaks the default shell)"))
                {
                    await removeSystemObject("Windows\\SystemApps");
                    await removeSystemObject("Program Files\\WindowsApps");
                }

                if (Program.isTweakEnabled(winBoxConfig, "remove windows defender files"))
                {
                    await removeSystemObject("Program Files (x86)\\Windows Defender");
                    await removeSystemObject("Program Files (x86)\\Windows Defender Advanced Threat Protection");
                    await removeSystemObject("Program Files\\Windows Defender");
                    await removeSystemObject("Program Files\\Windows Defender Advanced Threat Protection");
                    await removeSystemObject("ProgramData\\Microsoft\\Windows Defender");
                }

                if (Program.isTweakEnabled(winBoxConfig, "remove OneDrive")) {
                    await removeSystemObject("Windows\\System32\\Tasks\\Microsoft\\OneDrive");
                    await removeSystemObject("Windows\\System32\\OneDriveSetup.exe");
                    await removeSystemObject("Windows\\SysWOW64\\OneDriveSetup.exe");
                }
            }

            foreach (string path in splitRickTextboxLinesWithoutEmptyLines(winBoxConfig.delete_paths ?? ""))
            {
                await removeSystemObject(path);
            }

            /*
            foreach (string path in getSchtasksDeletePaths())
            {
                await removeSystemObject("!" + path, "Windows\\System32\\Tasks");
            }
            */

            await writeDebugFile("RemovePaths", RemovePaths_log);
            await writeDebugFile("RemoveDism", RemoveDism_log);

            // ------------------------------------ save & export

            if (winBoxConfig.winmounted_breakbefore == true) breakpointStop("win-mounted", false);
            if (winBoxConfig.winmountedEnabled == true)
            {
                processValue(63);
                processName("Executing a win-mounted event");
                await Program.executeBuildEvent(baseDirectoryPath, winBoxConfig.winmountedEvent);
            }
            if (winBoxConfig.winmounted_breakafter == true) breakpointStop("win-mounted", true);

            processName("Unmounting and save install.wim");
            processValue(70);
            await umountDism(true);

            if (imgExportPath != null)
            {
                processName("Generating an .img image of a partition");
                processValue(75);
                await ExportImg(newWimPath, imgExportPath);
            }

            return true;
        }
        private async Task CompleteExport(Action<string> processName, Action<int> processValue, string exportPath)
        {
            if (winBoxConfig.postbuild_breakbefore == true) breakpointStop("post-build", false);
            if (winBoxConfig.postbuildEnabled == true)
            {
                processValue(98);
                processName("Executing a post-build event");
                await Program.executeBuildEvent(baseDirectoryPath, winBoxConfig.postbuildEvent, $"\"{exportPath}\"");
            }
            if (winBoxConfig.postbuild_breakafter == true) breakpointStop("post-build", true);

            processValue(99);
            await RemoveTemp(processName);

            processName("Completed!");
            processValue(100);
            await Task.Delay(2000);
        }

        async Task<int> getWindowsSetupWimSlot(string wimPath)
        {
            string[] outputLines = await getFromDismWithOffset(
                $"/English /Get-WimInfo /WimFile:\"{wimPath}\"",
                "Name : Microsoft Windows Setup",
                "Index",
                -1
            );

            if (outputLines.Length > 0 && int.TryParse(outputLines[0], out int value))
            {
                return value;
            }

            return 1;
        }

        async Task modUnpackedIso(Action<string> processName, Action<int> processValue, string unpackIsoPath, WindowsDescription newWindowsDescription)
        {
            bool modAllow = winBoxConfig.manual_setup != true || winBoxConfig.installermod_manual_allow == true;

            // modify BCD in installer iso
            if (modAllow)
                await winBoxConfig.installer_winPE_mod.modMountedIso(unpackIsoPath);

            // unpack winPE
            string bootWimPath = Path.Combine(unpackIsoPath, "sources\\boot.wim");
            bool mountBootWim = modAllow || needMountInstallerBoot();
            if (mountBootWim) {
                int wimSetupSlot = await getWindowsSetupWimSlot(bootWimPath);
                await mountDism(bootWimPath, wimWinPeMountPath, wimSetupSlot);

                if (winBoxConfig.installerMountedEarly_breakbefore == true) breakpointStop("installer-mounted", false);
                if (winBoxConfig.installerMountedEarlyEnabled == true)
                {
                    processValue(83);
                    processName("Executing a installer-mounted event");
                    await Program.executeBuildEvent(baseDirectoryPath, winBoxConfig.installerMountedEarlyEvent);
                }
                if (winBoxConfig.installerMountedEarly_breakafter == true) breakpointStop("installer-mounted", true);
            }

            if (mountBootWim)
            {
                // optional add adverting files to boot.wim
                await addAdFiles(wimWinPeMountPath, newWindowsDescription, winBoxConfig.aaf_readme_boot, winBoxConfig.aaf_info_boot);

                // add user files to boot.wim
                string filesPath = Path.Combine(resourcesDirectoryPath, "boot_files");
                if (Directory.Exists(filesPath))
                {
                    await Program.CopyFilesRecursivelyAsync(filesPath, wimWinPeMountPath);
                }

                filesPath = Path.Combine(tempDirectoryPath, "boot_files");
                if (Directory.Exists(filesPath))
                {
                    await Program.CopyFilesRecursivelyAsync(filesPath, wimWinPeMountPath);
                }

                // umount & save boot.wim
                await umountDism(true, wimWinPeMountPath);
            }

            // modify BCD in installer wim
            if (modAllow)
                await winBoxConfig.installer_winPE_mod.modMountedWim(wimWinPeMountPath);

            // add winbox maker installer bypass
            if (modAllow && winBoxConfig.install_bypass == true)
            {
                string winPEsetup = @"reg add ""HKEY_LOCAL_MACHINE\SYSTEM\Setup\LabConfig"" /v BypassTPMCheck /t REG_DWORD /d 1 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\Setup\LabConfig"" /v BypassSecureBootCheck /t REG_DWORD /d 1 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\Setup\LabConfig"" /v BypassSecureBoot /t REG_DWORD /d 1 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\Setup\LabConfig"" /v BypassRAMCheck /t REG_DWORD /d 1 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\Setup\LabConfig"" /v BypassStorageCheck /t REG_DWORD /d 1 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\Setup\LabConfig"" /v BypassCPUCheck /t REG_DWORD /d 1 /f
reg add ""HKEY_LOCAL_MACHINE\SYSTEM\Setup\MoSetup"" /v AllowUpgradesWithUnsupportedTPMOrCPU /t REG_DWORD /d 1 /f";

                // add WinboxMaker_winPE_setup.bat
                string winPEsetupName = "WinboxMaker_winPE_setup.bat";
                await File.WriteAllTextAsync(Path.Combine(wimWinPeMountPath, winPEsetupName), winPEsetup);
                await File.WriteAllTextAsync(Path.Combine(unpackIsoPath, winPEsetupName), winPEsetup);

                // winPE setup autoexec
                string winPEcmdPath = Path.Combine(wimWinPeMountPath, "Windows\\System32\\startnet.cmd");
                if (File.Exists(winPEcmdPath))
                {
                    string winPEsetupExec = @$"call ""X:\{winPEsetupName}""";
                    await File.WriteAllTextAsync(winPEcmdPath, winPEsetupExec + "\r\n" + File.ReadAllText(winPEcmdPath) + "\r\n" + winPEsetupExec);
                }
            }

            // optional add adverting files to installer iso
            await addAdFiles(unpackIsoPath, newWindowsDescription, winBoxConfig.aaf_readme_iso, winBoxConfig.aaf_info_iso);

            // add user files to installer iso
            string isoFilesPath = Path.Combine(resourcesDirectoryPath, "iso_files");
            if (Directory.Exists(isoFilesPath))
            {
                await Program.CopyFilesRecursivelyAsync(isoFilesPath, unpackIsoPath);
            }

            isoFilesPath = Path.Combine(tempDirectoryPath, "iso_files");
            if (Directory.Exists(isoFilesPath))
            {
                await Program.CopyFilesRecursivelyAsync(isoFilesPath, unpackIsoPath);
            }
        }

        public async Task<bool> BuildEsdAsync(Action<string> processName, Action<int> processValue, string exportPath, WindowsDescription newWindowsDescription, bool showComplete = true)
        {

            string new_install_wim = Path.Combine(tempDirectoryPath, "new_install.wim");
            if (!await MakeModWim(processName, processValue, newWindowsDescription, new_install_wim, null))
            {
                return false;
            }

            processName("Converting install.wim to install.esd");
            processValue(77);
            await Program.ExecuteAsync("dism.exe", @$"/Export-Image /SourceImageFile:""{new_install_wim}"" /All /DestinationImageFile:""{exportPath}"" /Compress:recovery /CheckIntegrity", null, debugFolder);

            processName("Deleting install.wim");
            processValue(79);
            await Task.Run(() =>
            {
                File.Delete(new_install_wim);
            });

            if (showComplete)
            {
                await CompleteExport(processName, processValue, exportPath);
            }

            return true;
        }

        public async Task<bool> BuildIsoAsync(Action<string> processName, Action<int> processValue, string exportPath, WindowsDescription newWindowsDescription, bool showComplete = true, bool initViaVmMode = false)
        {
            string? baseWindowsImageFullPath = await getWindowsImagePath();
            if (baseWindowsImageFullPath == null)
            {
                Program.Error(Program.isoError);
                return false;
            }

            processName("Unpacking the iso");
            string[] unpackBlacklist = { "sources\\install.wim", "sources\\install.esd" };

            bool failed = false;
            if (await Program.UnpackUdfIso(baseWindowsImageFullPath, unpackIsoPath, processValue, unpackBlacklist))
            {
                if (!await BuildEsdAsync(processName, processValue, Path.Combine(unpackIsoPath, "sources\\install.esd"), newWindowsDescription, false))
                {
                    showComplete = false;
                    failed = true;
                    goto end;
                }
            }
            else
            {
                if (!await MakeModWim(processName, processValue, newWindowsDescription, Path.Combine(unpackIsoPath, "sources\\install.wim"), null, initViaVmMode))
                {
                    showComplete = false;
                    failed = true;
                    goto end;
                }
            }

            processName("ISO modification");
            processValue(80);

            bool manual = winBoxConfig.manual_setup == true;

            if (!manual) {
                if (winBoxConfig.oemkey_installer == true && winBoxConfig.isValidOemKey())
                {
                    await File.WriteAllTextAsync(Path.Combine(unpackIsoPath, "Sources\\PID.txt"), $"[PID]\nValue={winBoxConfig.OemKey}");
                }
            }

            await modUnpackedIso(processName, processValue, unpackIsoPath, newWindowsDescription);

            processName("ISO modification");
            processValue(83);

            if (manual)
            {
                if (winBoxConfig.manual_setup_autounattend != null)
                {
                    string xmlPath = Path.Combine(resourcesDirectoryPath, winBoxConfig.manual_setup_autounattend);
                    if (File.Exists(xmlPath))
                    {
                        await Program.CopyFileAsync(xmlPath, Path.Combine(unpackIsoPath, "autounattend.xml"));
                    }
                }
            }

            processName("Building an ISO image");
            processValue(85);
            //await Program.ExecuteAsync(Program.oscdimgPath, $"-m -u2 -b\"{Path.Combine(unpackIsoPath, "boot\\etfsboot.com")}\" \"{unpackIsoPath}\" \"{exportPath}\"");
            await Program.ExecuteAsync(Program.oscdimgPath, $"-m -o -u2 -udfver102 -bootdata:2#p0,e,b\"{Path.Combine(unpackIsoPath, "boot\\etfsboot.com")}\"#pEF,e,b\"{Path.Combine(unpackIsoPath, "efi\\microsoft\\boot\\efisys.bin")}\" \"{unpackIsoPath}\" \"{exportPath}\"", null, debugFolder);

            end:
            processName("Deleting unpacked ISO files");
            processValue(90);
            await Task.Run(() =>
            {
                Directory.Delete(unpackIsoPath, true);
            });

            if (showComplete)
            {
                await CompleteExport(processName, processValue, exportPath);
            }

            return !failed;
        }

        public async Task BuildWimAsync(Action<string> processName, Action<int> processValue, string exportPath, WindowsDescription newWindowsDescription)
        {
            if (await MakeModWim(processName, processValue, newWindowsDescription, exportPath, null))
            {
                await CompleteExport(processName, processValue, exportPath);
            }
        }

        public async Task __BuildImgAsync(Action<string> processName, Action<int> processValue, string exportPath, WindowsDescription newWindowsDescription)
        {
            await MakeModWim(processName, processValue, newWindowsDescription, newWimFile, exportPath);

            processName("Deleting temp install.wim");
            processValue(90);
            await Task.Run(() =>
            {
                File.Delete(newWimFile);
            });

            await CompleteExport(processName, processValue, exportPath);
        }

        public async Task<bool> BuildImgAsync(Action<string> processName, Action<int> processValue, string exportPath, WindowsDescription newWindowsDescription, bool useUefi=false, bool showComplete=true)
        {
            string tempIsoPath = Path.Combine(tempDirectoryPath, "temp.iso");
            bool successfully = true;

            if (await BuildIsoAsync(processName, processValue, tempIsoPath, newWindowsDescription, false, true))
            {
                processName("Launching a virtual machine");
                processValue(95);
                await InstallToImg(tempIsoPath, exportPath, useUefi);
            }
            else
            {
                showComplete = false;
                successfully = false;
            }

            processName("Deleting temp temp.iso");
            processValue(97);
            await Task.Run(() =>
            {
                File.Delete(tempIsoPath);
            });

            if (showComplete)
            {
                await CompleteExport(processName, processValue, exportPath);
            }

            return successfully && File.Exists(exportPath);
        }

        async Task CaptureFfu(string imgPath, string ffuOutput)
        {
            string args = @$"/Capture-Ffu /CaptureDrive:""{imgPath}"" /ImageFile:""{ffuOutput}"" /Name:""{winBoxConfig.WinboxName}"" /Description:""{winBoxConfig.WinboxDescription}""";
            await Program.ExecuteAsync("dism.exe", args, null, debugFolder);
        }

        public async Task BuildFfuAsync(Action<string> processName, Action<int> processValue, string exportPath, WindowsDescription newWindowsDescription, bool useUefi = false)
        {
            string tempImgPath = Path.Combine(tempDirectoryPath, "temp.img");

            bool showComplete = false;
            if (await BuildImgAsync(processName, processValue, tempImgPath, newWindowsDescription, useUefi, false))
            {
                processName("Launching a virtual machine");
                processValue(95);
                await CaptureFfu(tempImgPath, exportPath);
            }
            else
            {
                showComplete = false;
            }

            processName("Deleting temp temp.img");
            processValue(97);
            await Task.Run(() =>
            {
                File.Delete(tempImgPath);
            });

            if (showComplete)
            {
                await CompleteExport(processName, processValue, exportPath);
            }
        }

        public bool canExport()
        {
            bool canExport = true;
            if (winBoxConfig.BaseWindowsImage == null || winBoxConfig.BaseWindowsVersion == null)
            {
                canExport = false;
            }

            if (canExport && winBoxConfig.manual_setup != true)
            {
                switch (winBoxConfig.ProgramType)
                {
                    case ProgramTypeEnum.ExecutableFile:
                        if (winBoxConfig.ProgramName == null || winBoxConfig.ProgramName.Length == 0)
                        {
                            canExport = false;
                        }
                        break;

                    case ProgramTypeEnum.RawCommand:
                        if (winBoxConfig.RawCommand == null || winBoxConfig.RawCommand.Length == 0)
                        {
                            canExport = false;
                        }
                        break;

                    case ProgramTypeEnum.WebSite:
                        if (winBoxConfig.WebSite == null || winBoxConfig.WebSite.Length == 0)
                        {
                            canExport = false;
                        }
                        break;

                    case ProgramTypeEnum.None:
                        break;
                }
            }

            if (canExport)
            {
                bool exists = false;
                WindowsDescription[] localWindowsDescriptions = GetWindowsDescriptions();
                foreach (WindowsDescription item in localWindowsDescriptions)
                {
                    if (item.name == winBoxConfig.BaseWindowsVersion)
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                {
                    canExport = false;
                }
            }

            return canExport;
        }

        public async Task<bool> debugBuildProgramsAsync(Action<string> processName, Action<int> processValue)
        {
            if (Directory.Exists(debugBuildProgramsPath))
            {
                Directory.Delete(debugBuildProgramsPath, true);
            }

            processValue(30);
            processName("copying user program files (for debugging)");
            string programPath = Path.Combine(resourcesDirectoryPath, "program");
            if (Directory.Exists(programPath))
            {
                await Program.CopyFilesRecursivelyAsync(programPath, debugBuildProgramsPath);
            }

            processValue(70);
            processName("Compiling a user project (for debugging)");
            int index = 1;
            foreach (BuildItem buildItem in winBoxConfig.BuildItems)
            {
                if (!await BuildUserProject(index, buildItem, true))
                {
                    Program.Error("couldn't build a custom project. the paths to the required build system may not be configured in the winbox maker settings");
                    return false;
                }
                index++;
            }

            return true;
        }
    }
}
