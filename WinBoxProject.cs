using DiscUtils.Raw;
using DiscUtils.Udf;
using DiscUtils.Vfs;
using ManagedWimLib;
using Microsoft.VisualBasic.ApplicationServices;
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
using System.Text;
using System.Text.Json;
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
        public string sourcesDirectoryPath;
        string tempDirectoryPath;
        string unpackedWimFile;
        string wimInfoFile;
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
            wimInfoFile = Path.Combine(tempDirectoryPath, "installWimInfo.json");
            newWimFile = Path.Combine(tempDirectoryPath, "new_install.wim");
            wimMountPath = Path.Combine(tempDirectoryPath, "wim_mount");
            unpackIsoPath = Path.Combine(tempDirectoryPath, "iso_unpack");
            sourcesDirectoryPath = Path.Combine(resourcesDirectoryPath, "sources");
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
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "nvidia_drivers"));
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "amd_drivers"));
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "packages"));
            Program.CreateDirectory(Path.Combine(resourcesDirectoryPath, "cursor"));
            Program.CreateDirectory(sourcesDirectoryPath);

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

                if (!File.Exists(downloadPath))
                {
                    processName("Downloading a windows image by URL");
                    await Program.downloadFile(winBoxConfig.BaseWindowsImage, downloadPath, processValue);
                }

                if (File.Exists(downloadPath))
                {
                    return downloadPath;
                }
            }
            else
            {
                string path = Path.Combine(baseDirectoryPath, winBoxConfig.BaseWindowsImage);
                if (File.Exists(path))
                {
                    return path;
                }
            }

            return null;
        }

        public bool NeedLoadWindows()
        {
            return winBoxConfig.BaseWindowsImage != null && !File.Exists(wimInfoFile);
        }

        public async Task ExtractInstallWim(Action<string> processName, Action<int> processValue)
        {
            string? baseWindowsImageFullPath = await getWindowsImagePath(processName, processValue);
            if (baseWindowsImageFullPath == null) return;

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
                }
            }

            processValue(50);
            await DeleteInstallWim(processName);
        }

        public void UnloadWindowsImage()
        {
            if (File.Exists(wimInfoFile))
            {
                File.Delete(wimInfoFile);
            }
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

        public async Task CopyBlob(string name)
        {
            string? path = Program.getBlobPath(winBoxConfig, name);
            if (path != null)
            {
                await Program.CopyFileAsync(path, Path.Combine(wimMountPath, "WinboxResources", name));
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

        public async Task UnpackBlob(string name)
        {
            string? path = Program.getBlobPath(winBoxConfig, name);
            if (path != null)
            {
                await Task.Run(() =>
                {
                    try
                    {
                        ZipFile.ExtractToDirectory(path, Path.Combine(wimMountPath, "WinboxResources"));
                    }
                    catch (Exception ex)
                    {
                    }
                });
            }
        }

        private async Task RemoveTempFolder(string folder)
        {
            string tempDriversPath = Path.Combine(tempDirectoryPath, folder);
            if (Directory.Exists(tempDriversPath))
            {
                Directory.Delete(tempDriversPath, true);
            }
        }

        private async Task RemoveTemp(Action<string> processName) {
            processName("Cleaning temporary files");
            await RemoveTempFolder("drivers");
            await RemoveTempFolder("program");
            await RemoveTempFolder("files");
            await RemoveTempFolder("packages");
            await RemoveTempFolder("nvidia_drivers");
            await RemoveTempFolder("amd_drivers");
        }

        public async Task BuildCMakeProject(BuildItem buildItem, string cmakeFolder, string output)
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

            await Program.ExecuteAsync(Program.winboxSettings.path_cmake, $"-A \"{architecture}\" -DCMAKE_BUILD_TYPE=\"{configuration}\" \"{cmakeFolder}\"", buildDir);
            await Program.ExecuteAsync(Program.winboxSettings.path_cmake, $"--build . --config \"{configuration}\"", buildDir);
            await Program.CopyFilesRecursivelyAsync(Path.Combine(buildDir, configuration), output);

            Directory.Delete(buildDir, true);
        }

        public async Task BuildCargoProject(BuildItem buildItem, string cargoFolder, string output)
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

            await Program.ExecuteAsync(Program.winboxSettings.path_cargo, $"build --release --target=\"{target}\" --target-dir=\"{buildDir}\"", cargoFolder);
            await Program.CopyFileAsync(Path.Combine(buildDir, target, "release", exeName), Path.Combine(output, exeName));

            Directory.Delete(buildDir, true);
        }

        public async Task RunCustomBuildSystem(BuildItem buildItem, string output)
        {
            string tempBatFilePath = Path.Combine(tempDirectoryPath, "custom_build.bat");
            await File.WriteAllTextAsync(tempBatFilePath, buildItem.custom_command);
            await Program.ExecuteAsync(tempBatFilePath, $"\"{buildItem.custom_path}\" \"{output}\" \"{winBoxConfig.Architecture}\"", buildItem.custom_path);
            File.Delete(tempBatFilePath);
        }

        public async Task<bool> BuildUserProject(BuildItem buildItem)
        {
            string outputDir = Path.Combine(tempDirectoryPath, "program");
            if (buildItem.subdirectory_enabled)
            {
                if (buildItem.subdirectory.Contains("..")) return false;
                outputDir = Path.Combine(tempDirectoryPath, "program", buildItem.subdirectory ?? "");
            }
            else
            {
                outputDir = Path.Combine(tempDirectoryPath, "program");
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
                            $"\"{Path.Combine(sourcesDirectoryPath, buildItem.msbuild_path)}\" /p:Configuration=\"{buildItem.msbuild_configuration}\" /p:Platform=\"{architecture}\" /p:OutputPath=\"{outputDir}\" /p:OutDir=\"{outputDir}\"");
                        return true;
                    }
                    break;

                case BuildItemType.cmake:
                    if (Program.winboxSettings.path_cmake != null)
                    {
                        await BuildCMakeProject(buildItem, Path.GetDirectoryName(Path.Combine(sourcesDirectoryPath, buildItem.cmake_path)), outputDir);
                        return true;
                    }
                    break;

                case BuildItemType.cargo:
                    if (Program.winboxSettings.path_cargo != null)
                    {
                        await BuildCargoProject(buildItem, Path.GetDirectoryName(Path.Combine(sourcesDirectoryPath, buildItem.cargo_path)), outputDir);
                        return true;
                    }
                    break;

                case BuildItemType.custom:
                    await RunCustomBuildSystem(buildItem, outputDir);
                    return true;
            }

            return false;
        }

        public async Task DownloadFile(DownloadItem downloadItem, Action<int> processValue)
        {
            if (downloadItem.path.Contains("..")) return;

            bool needDelete = false;
            string downloadPath;
            if (downloadItem.cache == true)
            {
                downloadPath = Path.Combine(Program.downloadCachePath, Program.CalculateMD5(downloadItem.url));
                if (!File.Exists(downloadPath))
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

            string outputPath = Path.Combine(baseDirectoryPath, downloadItem.path);

            if (downloadItem.unpack == true)
            {
                Directory.CreateDirectory(outputPath);
                await Program.ExecuteAsync(Program.z7Path, @$"x ""{downloadPath}"" -o""{outputPath}""");
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

        public async Task InstallToImg(string isoPath, string imgPath)
        {
            string emuName = null;
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
            string qemuParameters = $"-drive file=\"{imgPath}\",format=raw -cdrom \"{isoPath}\" -boot d -m 1024 -smp 2";

            await File.WriteAllTextAsync(Path.Combine(tempDirectoryPath, "debug_qemu.txt"), $"\"{qemuPath}\" {qemuParameters}");

            await Program.ExecuteAsync(Path.Combine(Program.winboxSettings.path_qemu_folder, "qemu-img.exe"), $"create -f raw \"{imgPath}\" 20G");
            await Program.ExecuteAsync(qemuPath, qemuParameters);
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

        public async Task<bool> MakeModWim(Action<string> processName, Action<int> processValue, WindowsDescription newWindowsDescription, string newWimPath, string? imgExportPath)
        {
            if (winBoxConfig.prebuildEnabled == true)
            {
                processValue(2);
                processName("Executing a pre-build event");
                await Program.executeBuildEvent(baseDirectoryPath, winBoxConfig.prebuildEvent);
            }

            processValue(5);
            await RemoveTemp(processName);

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
                foreach (BuildItem buildItem in winBoxConfig.BuildItems)
                {
                    if (!await BuildUserProject(buildItem))
                    {
                        Program.Error("couldn't build a custom project. the paths to the required build system may not be configured in the winbox maker settings");
                        return false;
                    }
                }
            }

            // ------------------------------------ creating a new install.wim file for subsequent modification

            await ExtractInstallWim(processName, processValue);

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

            async Task addGpuDrivers(string baseDir)
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
                        await Program.ExecuteAsync(Program.z7Path, @$"x ""{file}"" -o""{path}""");
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
                        await Program.ExecuteAsync(Program.z7Path, @$"x ""{file}"" -o""{path}""");
                        number++;
                    }
                }
            }

            await addGpuDrivers(resourcesDirectoryPath);
            await addGpuDrivers(tempDirectoryPath);

            await Program.ExecuteAsync("reg.exe", $"import \"{Program.ResourcePath(Path.Combine("reg", "tweak.reg"))}\"");

            /*
            string lockScreenAppPath = Path.Combine(wimMountPath, "Windows\\SystemApps\\Microsoft.LockApp_cw5n1h2txyewy");
            if (Directory.Exists(lockScreenAppPath))
            {
                Directory.Delete(lockScreenAppPath, true);
            }
            */

            // ------------------------------------ system init
            string baseSetup = $@"@echo off

powercfg -h off
powercfg -change -standby-timeout-ac 0
powercfg -change -standby-timeout-dc 0
powercfg -change -hibernate-timeout-ac 0
powercfg -change -hibernate-timeout-dc 0
powercfg -change -monitor-timeout-ac {winBoxConfig.ScreenTimeout}
powercfg -change -monitor-timeout-dc {winBoxConfig.ScreenTimeout}
powercfg -setacvalueindex SCHEME_CURRENT SUB_BUTTONS LIDSWITCH 0
powercfg -setdcvalueindex SCHEME_CURRENT SUB_BUTTONS LIDSWITCH 0
powercfg -s SCHEME_CURRENT

dism /online /enable-feature /all /featurename:Client-EmbeddedLogon
dism /online /enable-feature /all /featurename:Client-DeviceLockdown
dism /online /enable-feature /all /featurename:Client-KeyboardFilter

sc config edgeupdate start= disabled
sc config edgeupdatem start= disabled
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

call ""C:\WinboxResources\UpdateSystemSettings.bat""
schtasks /create /tn ""winbox_UpdateSystemSettings"" /tr ""C:\WinboxResources\UpdateSystemSettings.bat"" /sc onlogon /rl highest /ru ""SYSTEM""

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

reg add ""HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Embedded\EmbeddedLogon"" /v HideAutoLogonUI /t REG_DWORD /d 1 /f
reg add ""HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Embedded\EmbeddedLogon"" /v HideFirstLogonAnimation /t REG_DWORD /d 1 /f
reg add ""HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Embedded\EmbeddedLogon"" /v BrandingNeutral /t REG_DWORD /d 1 /f
reg add ""HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Embedded\EmbeddedLogon"" /v NoLockScreen /t REG_DWORD /d 1 /f
reg add ""HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Embedded\EmbeddedLogon"" /v AnimationDisabled /t REG_DWORD /d 1 /f
reg add ""HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Embedded\EmbeddedLogon"" /v UIVerbosityLevel /t REG_DWORD /d 1 /f

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
reg add ""HKEY_LOCAL_MACHINE\DEFAULT_USER\Software\NVIDIA Corporation\Global\NVTweak"" /v OverlayHook /t REG_DWORD /d 0 /f";

            string updateSystemSettings = $@"@echo off

reg add ""HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\SessionData"" /v AllowLockScreen /t REG_DWORD /d 0 /f

reagentc.exe /disable
netsh advfirewall set allprofiles state off

bcdedit /set {{bootmgr}} displaybootmenu no
bcdedit /set {{bootmgr}} timeout 0
bcdedit /set {{current}} bootstatuspolicy ignoreallfailures
bcdedit /set {{current}} recoveryenabled no
bcdedit /set {{current}} loadoptions DISABLE_INTEGRITY_CHECKS
bcdedit /set {{current}} NOINTEGRITYCHECKS ON
bcdedit /set {{current}} TESTSIGNING ON

bcdedit /set {{bootmgr}} bootstatuspolicy ignoreallfailures
bcdedit /set {{bootmgr}} recoveryenabled no
bcdedit /set {{bootmgr}} loadoptions DISABLE_INTEGRITY_CHECKS
bcdedit /set {{bootmgr}} NOINTEGRITYCHECKS ON
bcdedit /set {{bootmgr}} TESTSIGNING ON

bcdedit /set {{current}} displaybootmenu no
bcdedit /set {{current}} timeout 0

bcdedit /set {{default}} displaybootmenu no
bcdedit /set {{default}} timeout 0
bcdedit /set {{default}} bootstatuspolicy ignoreallfailures
bcdedit /set {{default}} recoveryenabled no
bcdedit /set {{default}} loadoptions DISABLE_INTEGRITY_CHECKS
bcdedit /set {{default}} NOINTEGRITYCHECKS ON
bcdedit /set {{default}} TESTSIGNING ON";

            string applicationScript = $@"@echo off";

            bool customBootLogo = winBoxConfig.CustomBootLogo != null && !winBoxConfig.CustomBootLogo.Contains("\"");
            string cursorPath = Path.Combine(resourcesDirectoryPath, "cursor");
            bool customCursor = Directory.Exists(cursorPath) && !Program.IsDirectoryEmpty(cursorPath);
            bool useWinboxService = winBoxConfig.UseEmbeddedDisplay == true;

            if (!Program.isTweakEnabled(winBoxConfig, "Do not disable hotkeys by changing the layout"))
            {
                baseSetup += "\r\n";
                baseSetup += $@"reg add ""HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Keyboard Layout"" /v ""Scancode Map"" /t REG_BINARY /d 000000000000000030000000000021e000006ce000006de0000011e000006be000003b0000004400000057000000580000006400000065000000660000006700000068000000690000006a0000003c0000006b0000006c0000006d0000006e0000006f0000003d0000003e0000003f0000004000000041000000420000004300000013e0000014e0000012e00000380000005be000005ee0000037e0000038e000005ce000005fe0000063e000006ae0000066e0000069e0000032e0000067e0000065e0000068e000000000 /f";
            }

            if (winBoxConfig.UseOemKey == true && winBoxConfig.OemKey != null && !winBoxConfig.OemKey.Contains("\""))
            {
                baseSetup += $"\r\ncscript /B \"%windir%\\system32\\slmgr.vbs\" /ipk \"{winBoxConfig.OemKey}\"\ncscript /B \"%windir%\\system32\\slmgr.vbs\" /ato";
            }

            if (Program.isTweakEnabled(winBoxConfig, "Disable boot circle"))
            {
                updateSystemSettings += $"\r\nbcdedit /set {{globalsettings}} custom:16000069 true";
            }

            if (Program.isTweakEnabled(winBoxConfig, "Disable boot logo"))
            {
                updateSystemSettings += $"\r\nbcdedit /set {{globalsettings}} custom:16000067 true";
            }

            if (Program.isTweakEnabled(winBoxConfig, "Disable boot messages"))
            {
                updateSystemSettings += $"\r\nbcdedit /set {{globalsettings}} custom:16000068 true";
            }

            void regRedist(string name)
            {
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

            if (Program.isTweakEnabled(winBoxConfig, "Integrate net 4.8.1"))
            {
                await CopyBlob("net481.exe");
                baseSetup += $"\r\nstart /wait C:\\WinboxResources\\net481.exe /q";
            }

            if (Program.isTweakEnabled(winBoxConfig, "Integrate net 4.7.2") || customBootLogo)
            {
                await CopyBlob("net472.exe");
                baseSetup += $"\r\nstart /wait C:\\WinboxResources\\net472.exe /q";
            }

            if (Program.isTweakEnabled(winBoxConfig, "Integrate net 8.0.17") || useWinboxService)
            {
                await CopyBlob("net8017.exe");
                baseSetup += $"\r\nstart /wait C:\\WinboxResources\\net8017.exe /quiet /norestart";
            }

            if (Program.isTweakEnabled(winBoxConfig, "Integrate net 9.0.6"))
            {
                await CopyBlob("net906.exe");
                baseSetup += $"\r\nstart /wait C:\\WinboxResources\\net906.exe /quiet /norestart";
            }

            if (Program.isTweakEnabled(winBoxConfig, "Integrate app runtime 1.7.3"))
            {
                await CopyBlob("appruntime173.exe");
                applicationScript += $"\r\nIF NOT EXIST \"C:\\WinboxResources\\appruntime173.installed\" (";
                applicationScript += $"\r\nC:\\WinboxResources\\appruntime173.exe";
                applicationScript += $"\r\necho. > \"C:\\WinboxResources\\appruntime173.installed\"";
                applicationScript += $"\r\n)";
            }

            if (Program.isTweakEnabled(winBoxConfig, "Integrate microsoft edge") || winBoxConfig.ProgramType == ProgramTypeEnum.WebSite)
            {
                await CopyBlob("MicrosoftEdge.msi");
                baseSetup += $"\r\nstart /wait msiexec /i C:\\WinboxResources\\MicrosoftEdge.msi /quiet /norestart";
            }

            if (Program.isTweakEnabled(winBoxConfig, "Hide Cursor"))
            {
                await CopyResource("empty.cur");
                await CopyResource("hide_cursor.reg");
                baseSetup += $"\r\nregedit /s \"C:\\WinboxResources\\hide_cursor.reg\"";
                await OverwriteSystemCursorEmpty(Path.Combine(wimMountPath, "Windows", "Cursors"));
            }
            else if (customCursor)
            {
                await Program.CopyFilesRecursivelyAsync(cursorPath, Path.Combine(WinboxResourcesPath, "cursor"));
                await CopyResource("custom_cursor.reg");
                baseSetup += $"\r\nregedit /s \"C:\\WinboxResources\\custom_cursor.reg\"";
                await OverwriteSystemCursorEmpty(Path.Combine(wimMountPath, "Windows", "Cursors"));
            }

            if (customBootLogo)
            {
                string logoPath = Path.Combine(resourcesDirectoryPath, winBoxConfig.CustomBootLogo);
                if (File.Exists(logoPath))
                {
                    await UnpackBlob("HackBGRT.zip");

                    string debugBootLogoPath = Path.Combine(tempDirectoryPath, "debug_logo.bmp");
                    string splashBootLogoPath = Path.Combine(WinboxResourcesPath, "HackBGRT-2.5.2", "splash.bmp");
                    ImageConverter.ConvertToBmp_54_24(logoPath, debugBootLogoPath);
                    await Program.CopyFileAsync(debugBootLogoPath, splashBootLogoPath);

                    string configBootLogoPath = Program.ResourcePath(Path.Combine("resources", winBoxConfig.CustomBootLogo_centering == true ? "hackBGRT_centering.txt" : "hackBGRT.txt"));
                    await Program.CopyFileAsync(configBootLogoPath, Path.Combine(WinboxResourcesPath, "HackBGRT-2.5.2", "config.txt"));

                    string hackBGRT = "\r\ncd C:\\WinboxResources\\HackBGRT-2.5.2\r\nC:\\WinboxResources\\HackBGRT-2.5.2\\setup.exe batch install allow-secure-boot allow-bitlocker allow-bad-loader enable-overwrite enable-bcdedit";
                    baseSetup += hackBGRT;

                    applicationScript += $"\r\nIF NOT EXIST \"C:\\WinboxResources\\hackBGRT.installed\" (";
                    applicationScript += hackBGRT;
                    applicationScript += $"\r\necho. > \"C:\\WinboxResources\\hackBGRT.installed\"";
                    applicationScript += $"\r\n)";
                }
            }

            baseSetup += "\r\ncd C:\\";
            applicationScript += "\r\ncd C:\\";

            if (winBoxConfig.PostInstall_reg != null)
            {
                string regPath = Path.Combine(resourcesDirectoryPath, winBoxConfig.PostInstall_reg);
                if (File.Exists(regPath))
                {
                    await Program.CopyFileAsync(regPath, Path.Combine(WinboxResourcesPath, "postinstall.reg"));
                    baseSetup += $"\r\nregedit /s \"C:\\WinboxResources\\postinstall.reg\"";
                }
            }

            if (winBoxConfig.PostInstall_bat != null)
            {
                string batPath = Path.Combine(resourcesDirectoryPath, winBoxConfig.PostInstall_bat);
                if (File.Exists(batPath))
                {
                    await Program.CopyFileAsync(batPath, Path.Combine(WinboxResourcesPath, "postinstall.bat"));
                    baseSetup += $"\r\ncall \"C:\\WinboxResources\\postinstall.bat\"";
                }
            }

            if (winBoxConfig.PostInstall_user_reg != null)
            {
                string regPath = Path.Combine(resourcesDirectoryPath, winBoxConfig.PostInstall_user_reg);
                if (File.Exists(regPath))
                {
                    await Program.CopyFileAsync(regPath, Path.Combine(WinboxResourcesPath, "postinstall_user.reg"));
                    applicationScript += $"\r\nIF NOT EXIST \"C:\\WinboxResources\\postinstall_reg.installed\" (";
                    applicationScript += $"\r\nregedit /s \"C:\\WinboxResources\\postinstall_user.reg\"";
                    applicationScript += $"\r\necho. > \"C:\\WinboxResources\\postinstall_reg.installed\"";
                    applicationScript += $"\r\n)";
                }
            }

            if (winBoxConfig.PostInstall_user_bat != null)
            {
                string batPath = Path.Combine(resourcesDirectoryPath, winBoxConfig.PostInstall_user_bat);
                if (File.Exists(batPath))
                {
                    await Program.CopyFileAsync(batPath, Path.Combine(WinboxResourcesPath, "postinstall_user.bat"));
                    applicationScript += $"\r\nIF NOT EXIST \"C:\\WinboxResources\\postinstall_bat.installed\" (";
                    applicationScript += $"\r\ncall \"C:\\WinboxResources\\postinstall_user.bat\"";
                    applicationScript += $"\r\necho. > \"C:\\WinboxResources\\postinstall_bat.installed\"";
                    applicationScript += $"\r\n)";
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
                baseSetup += installDriver;
                baseSetup += regStr;
                baseSetup += addDisplay;
            }

            if (useWinboxService)
            {
                //await Program.CopyFilesRecursivelyAsync(AppDomain.CurrentDomain.BaseDirectory, Path.Combine(WinboxResourcesPath, "winbox_maker"));
            }

            if (winBoxConfig.UseEmbeddedDisplay == true)
            {
                //applicationScript += $"\r\nstart /B \"\" C:\\WinboxResources\\winbox_maker\\WinBox-Maker.exe";
            }

            baseSetup += $"\r\n";
            baseSetup += @$"reg unload HKLM\DEFAULT_USER

net user winbox /add
net localgroup Administrators winbox /add";

            await File.WriteAllTextAsync(Path.Combine(tempDirectoryPath, "debug_UpdateSystemSettings.txt"), updateSystemSettings);
            await File.WriteAllTextAsync(Path.Combine(tempDirectoryPath, "debug_SetupComplete.txt"), baseSetup);

            await File.WriteAllTextAsync(Path.Combine(WinboxResourcesPath, "UpdateSystemSettings.bat"), updateSystemSettings);
            await File.WriteAllTextAsync(Path.Combine(WindowsScriptsPath, "SetupComplete.cmd"), baseSetup);

            // ------------------------------------ copy program files
            string programPath = Path.Combine(resourcesDirectoryPath, "program");
            if (Directory.Exists(programPath))
            {
                await Program.CopyFilesRecursivelyAsync(programPath, Path.Combine(wimMountPath, "WinboxProgram"));
            }

            if (Directory.Exists(tempProgramPath))
            {
                await Program.CopyFilesRecursivelyAsync(tempProgramPath, Path.Combine(wimMountPath, "WinboxProgram"));
            }

            // ------------------------------------ copy files
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

            await File.WriteAllTextAsync(Path.Combine(wimMountPath, "README.txt"), $"this image was created by the {Program.version} free software\r\nhttps://github.com/igorkll/WinBox-Maker");

            // ------------------------------------ setup application autorun
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

                case ProgramTypeEnum.None:
                    break;
            }

            if (command != null)
            {
                applicationScript += "\r\n" + command;
            }

            await File.WriteAllTextAsync(Path.Combine(tempDirectoryPath, "debug_AppScript.txt"), applicationScript);
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
                await WriteHiddenBatExecuter(Path.Combine(WinboxResourcesPath, "run_app_script_hidden.vbs"), @"C:\WinboxResources\app_script.bat", null);
                await RegMod("SOFTWARE", "Microsoft\\Windows NT\\CurrentVersion\\Winlogon", "Shell", Program.EscapeForRegFile("wscript \"C:\\WinboxResources\\run_app_script_hidden.vbs\""));
            }

            // ------------------------------------ save & export
            await Program.ExecuteAsync("reg.exe", $"unload HKLM\\WINBOX_SOFTWARE");
            //await Program.ExecuteAsync("reg.exe", $"unload HKLM\\WINBOX_SYSTEM");

            async Task addUserDrivers(string baseDir)
            {
                string driversPath = Path.Combine(baseDir, "drivers");
                if (Program.IsDirectoryNotEmpty(driversPath))
                {
                    processName("Installing user drivers");
                    processValue(55);
                    await Program.ExecuteAsync("dism.exe", $"/image:\"{wimMountPath}\" /add-driver /driver:\"{driversPath}\" /recurse /forceunsigned");
                }
            }

            async Task addCabMsu(string baseDir)
            {
                string packagesPath = Path.Combine(baseDir, "packages");
                if (Program.IsDirectoryNotEmpty(packagesPath))
                {
                    processName("Installing .cab/.msu packages");
                    processValue(58);
                    await Program.ExecuteAsync("dism.exe", $"/image:\"{wimMountPath}\" /add-package /PackagePath:\"{packagesPath}\"");
                }
            }

            await addUserDrivers(resourcesDirectoryPath);
            await addUserDrivers(tempDirectoryPath);
            await addCabMsu(resourcesDirectoryPath);
            await addCabMsu(tempDirectoryPath);

            processName("Enabling necessary windows components");
            processValue(60);
            await Program.ExecuteAsync("dism.exe", $"/image:\"{wimMountPath}\" /enable-feature /all /featurename:Client-EmbeddedLogon");
            await Program.ExecuteAsync("dism.exe", $"/image:\"{wimMountPath}\" /enable-feature /all /featurename:Client-DeviceLockdown");
            await Program.ExecuteAsync("dism.exe", $"/image:\"{wimMountPath}\" /enable-feature /all /featurename:Client-KeyboardFilter");

            if (winBoxConfig.winmountedEnabled == true)
            {
                processValue(63);
                processName("Executing a win-mounted event");
                await Program.executeBuildEvent(baseDirectoryPath, winBoxConfig.winmountedEvent);
            }

            processName("Unmounting and save install.wim");
            processValue(70);
            await Program.ExecuteAsync("dism.exe", $"/Unmount-Wim /MountDir:\"{wimMountPath}\" /commit");

            if (imgExportPath != null)
            {
                processName("Generating an .img image of a partition");
                processValue(65);
                await ExportImg(newWimPath, imgExportPath);
            }

            processValue(75);
            await RemoveTemp(processName);

            return true;
        }
        private async Task CompleteExport(Action<string> processName, Action<int> processValue, string exportPath)
        {
            if (winBoxConfig.postbuildEnabled == true)
            {
                processValue(99);
                processName("Executing a post-build event");
                await Program.executeBuildEvent(baseDirectoryPath, winBoxConfig.postbuildEvent, $"\"{exportPath}\"");
            }

            processName("Completed!");
            processValue(100);
            await Task.Delay(2000);
        }
 
        public async Task<bool> BuildIsoAsync(Action<string> processName, Action<int> processValue, string exportPath, WindowsDescription newWindowsDescription, bool showComplete=true)
        {
            string? baseWindowsImageFullPath = await getWindowsImagePath();
            if (baseWindowsImageFullPath == null) return false;

            processName("Unpacking the iso");
            string[] unpackBlacklist = { "sources\\install.wim" };
            await Program.UnpackUdfIso(baseWindowsImageFullPath, unpackIsoPath, processValue, unpackBlacklist);

            bool failed = false;
            if (!await MakeModWim(processName, processValue, newWindowsDescription, Path.Combine(unpackIsoPath, "sources\\install.wim"), null))
            {
                showComplete = false;
                failed = true;
                goto end;
            }

            processName("ISO modification");
            processValue(80);
            if (winBoxConfig.UseOemKey == true)
            {
                await File.WriteAllTextAsync(Path.Combine(unpackIsoPath, "Sources\\PID.txt"), $"[PID]\nValue={winBoxConfig.OemKey}");
            }

            processName("Building an ISO image");
            processValue(85);
            //await Program.ExecuteAsync(Program.oscdimgPath, $"-m -u2 -b\"{Path.Combine(unpackIsoPath, "boot\\etfsboot.com")}\" \"{unpackIsoPath}\" \"{exportPath}\"");
            await Program.ExecuteAsync(Program.oscdimgPath, $"-m -o -u2 -udfver102 -bootdata:2#p0,e,b\"{Path.Combine(unpackIsoPath, "boot\\etfsboot.com")}\"#pEF,e,b\"{Path.Combine(unpackIsoPath, "efi\\microsoft\\boot\\efisys.bin")}\" \"{unpackIsoPath}\" \"{exportPath}\"");

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

        public async Task BuildImgAsync(Action<string> processName, Action<int> processValue, string exportPath, WindowsDescription newWindowsDescription)
        {
            string tempIsoPath = Path.Combine(tempDirectoryPath, "temp.iso");

            bool showComplete = true;
            if (await BuildIsoAsync(processName, processValue, tempIsoPath, newWindowsDescription, false))
            {
                processName("Launching a virtual machine");
                processValue(95);
                await InstallToImg(tempIsoPath, exportPath);
            }
            else
            {
                showComplete = false;
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
        }

        public bool canExport()
        {
            bool canExport = true;
            if (winBoxConfig.BaseWindowsImage == null || winBoxConfig.BaseWindowsVersion == null)
            {
                canExport = false;
            }

            if (canExport)
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
    }
}
