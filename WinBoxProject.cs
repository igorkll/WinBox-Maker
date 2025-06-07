using DiscUtils.Raw;
using DiscUtils.Udf;
using ManagedWimLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WinBox_Maker
{
    public class WinBoxProject
    {
        const string resourcesDirectoryName = "winbox_resources";
        const string bigResourcesDirectoryName = "winbox_bigResources";
        public WinBoxConfig winBoxConfig;
        string wnbFilePath;
        public string baseDirectoryPath;
        public string buildDirectoryPath;
        string resourcesDirectoryPath;
        string bigResourcesDirectoryPath;
        string tempDirectoryPath;
        string unpackedWimFile;
        string newWimFile;
        string wimMountPath;
        string name;
        string? err;

        public WinBoxProject(string wnbFilePath)
        {
            winBoxConfig = new WinBoxConfig();
            this.wnbFilePath = wnbFilePath;
            baseDirectoryPath = Path.GetDirectoryName(wnbFilePath) ?? "";
            buildDirectoryPath = Path.Combine(baseDirectoryPath, "winbox_build");
            resourcesDirectoryPath = Path.Combine(baseDirectoryPath, resourcesDirectoryName);
            bigResourcesDirectoryPath = Path.Combine(baseDirectoryPath, bigResourcesDirectoryName);
            tempDirectoryPath = Path.Combine(baseDirectoryPath, "winbox_temp");
            unpackedWimFile = Path.Combine(tempDirectoryPath, "base_install.wim");
            newWimFile = Path.Combine(tempDirectoryPath, "new_install.wim");
            wimMountPath = Path.Combine(tempDirectoryPath, "wim_mount");
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

            Program.CreateDirectory(buildDirectoryPath);
            Program.CreateDirectory(resourcesDirectoryPath);
            Program.CreateDirectory(bigResourcesDirectoryPath);
            Program.CreateDirectory(tempDirectoryPath);
            Program.CreateDirectory(wimMountPath);

            string gitignorePath = Path.Combine(baseDirectoryPath, ".gitignore");
            if (!File.Exists(gitignorePath)) {
                File.WriteAllText(gitignorePath, $"## WinBox-Maker\n\nwinbox_build\nwinbox_temp\nwinbox_bigResources\n");
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

        public async Task<string?> SelectResourceAsync(Action<string> processName, Action<int> processValue, string filter)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = resourcesDirectoryPath;
                openFileDialog.Filter = filter;
                openFileDialog.Title = "Select Resource";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    string fileName = Path.GetFileName(filePath);
                    if (Program.IsPathInsideDirectory(filePath, resourcesDirectoryPath))
                    {
                        return Path.Combine(resourcesDirectoryName, fileName);
                    }
                    else if (Program.IsPathInsideDirectory(filePath, bigResourcesDirectoryPath))
                    {
                        return Path.Combine(bigResourcesDirectoryName, fileName);
                    }

                    DialogResult result = MessageBox.Show("the file is not located in the project's resource directory, if you use it like this, then the project config will have the absolute path to the file, which will make it impossible to build on another computer. do you want to copy the file so that you don't have to use an absolute path?", "copy the file?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        string projectFolderToCopy;
                        FileInfo fileInfo = new FileInfo(filePath);
                        if (fileInfo.Length > (1024 * 1024 * 1024))
                        {
                            projectFolderToCopy = bigResourcesDirectoryName;
                        }
                        else
                        {
                            projectFolderToCopy = resourcesDirectoryName;
                        }

                        processName("Copying a resource file");
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

            using (Wim wimHandle = Wim.OpenWim(unpackedWimFile, OpenFlags.None))
            {
                WimInfo wimInfo = wimHandle.GetWimInfo();
                for (int i = 1; i <= wimInfo.ImageCount; i++)
                {
                    //Console.WriteLine($"Image Index: {i}");
                    //Console.WriteLine($"Name: {wimHandle.GetImageName(i)}");
                    //Console.WriteLine($"Description: {wimHandle.GetImageDescription(i)}");
                    WindowsDescription windowVersion = new WindowsDescription {
                        name = wimHandle.GetImageName(i) ?? "failed to read windows name",
                        description = wimHandle.GetImageDescription(i) ?? "failed to read windows description"
                    };
                    windowsVersions.Add(windowVersion);
                }
            }

            return windowsVersions.ToArray();
        }

        public async Task MakeModWim(Action<string> processName, Action<int> processValue, WindowsDescription newWindowsDescription, string newWimPath, string? imgPartitionPath)
        {
            //processName.Text = "Copying an install.wim file";
            //await Program.CopyFileAsync(unpackedWimFile, newWimPath, processValue);

            processName("Modification of install.wim");
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

            processName("Mounting install.wim");
            processValue(30);
            await Program.ExecuteAsync("dism.exe", $"/Mount-Wim /WimFile:\"{newWimPath}\" /index:1 /MountDir:\"{wimMountPath}\"");

            return;

            processName("Modification of the system files");
            processValue(50);
            await Program.ExecuteAsync("dism.exe", $"reg load HKLM\\TempHive \"{Path.Combine(wimMountPath, "Windows\\System32\\config\\SYSTEM")}\"");
            await Program.ExecuteAsync("dism.exe", $"reg import reg\\skip_oobe.reg");
            await Program.ExecuteAsync("dism.exe", $"reg import reg\\disable_telemetry.reg");
            await Program.ExecuteAsync("dism.exe", $"reg import reg\\disable_defender.reg");
            await Program.ExecuteAsync("dism.exe", $"reg import reg\\disable_autoupdate.reg");
            await Program.ExecuteAsync("dism.exe", $"reg import reg\\disable_checkdisk.reg");
            await Program.ExecuteAsync("dism.exe", $"reg import reg\\disable_powerdown_checks.reg");
            await Program.ExecuteAsync("dism.exe", $"reg import reg\\disable_bootmanager.reg");
            await Program.ExecuteAsync("dism.exe", $"reg unload HKLM\\TempHive");

            if (imgPartitionPath != null)
            {
                processName("Generating an .img image of a partition");
                processValue(60);
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

            processValue(100);
        }

        public async Task BuildIsoAsync(Action<string> processName, Action<int> processValue, string exportPath, WindowsDescription newWindowsDescription)
        {
            if (winBoxConfig.BaseWindowsImage == null || winBoxConfig.BaseWindowsVersion == null) return;
            string baseWindowsImageFullPath = GetAbsoluteResourcePath(winBoxConfig.BaseWindowsImage);

            await MakeModWim(processName, processValue, newWindowsDescription, newWimFile, null);

            /*
            processName.Text = "Copying an image file";
            await Program.CopyFileAsync(baseWindowsImageFullPath, exportPath, processValue);

            processName.Text = "Packaging of the modified install.wim in the installation image";
            using (FileStream isoStream = File.Open(exportPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                UdfReader cd = new UdfReader(isoStream);
                using (var wimFile = new FileStream(unpackedWimFile, FileMode.Open, FileAccess.Read))
                {
                    long totalBytes = wimFile.Length;
                    long bytesCopied = 0;

                    using (var outputStream = cd.OpenFile(@"sources\install.wim", FileMode.Open, FileAccess.Write))
                    {
                        byte[] buffer = new byte[81920];
                        int bytesRead;

                        while ((bytesRead = await wimFile.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await outputStream.WriteAsync(buffer, 0, bytesRead);
                            bytesCopied += bytesRead;

                            processValue.Value = (int)((bytesCopied * 100) / totalBytes);
                        }
                    }
                }
            }
            */

            File.Delete(newWimFile);
        }

        public async Task BuildWimAsync(Action<string> processName, Action<int> processValue, string exportPath, WindowsDescription newWindowsDescription)
        {
            if (winBoxConfig.BaseWindowsImage == null || winBoxConfig.BaseWindowsVersion == null) return;

            await MakeModWim(processName, processValue, newWindowsDescription, exportPath, null);
        }

        public async Task BuildImgPartitionAsync(Action<string> processName, Action<int> processValue, string exportPath, WindowsDescription newWindowsDescription)
        {
            if (winBoxConfig.BaseWindowsImage == null || winBoxConfig.BaseWindowsVersion == null) return;

            await MakeModWim(processName, processValue, newWindowsDescription, newWimFile, exportPath);

            File.Delete(newWimFile);
        }
    }
}
