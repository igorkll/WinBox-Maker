using DiscUtils;
using DiscUtils.Udf;
using ManagedWimLib;
using Microsoft.VisualBasic;
using Microsoft.WindowsAPICodePack.Taskbar;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WinBox_Maker
{
    internal static class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        public const int version_major = 1;
        public const int version_minor = 8;
        public const int version_patch = 1;
        public static string version_str = $"{version_major}.{version_minor}.{version_patch}";
        public static int version_num = (version_major * 10000) + (version_minor * 100) + version_patch;

        static bool is_beta_version = true;
        public static string version = $"WinBox-Maker {version_str}" + (is_beta_version ? " BETA" : "");
        public const string buildEventsWarning = "WARNING!!! THIS PROJECT USES BUILD EVENTS, WHICH MEANS THAT COMMANDS FROM THE EVENTS TAB WILL BE EXECUTED DURING THE BUILD PROCESS!!!";
        public static string isoError = "failed to load the iso file, make sure the path is correct";
        public const string logichubUrl = "https://igorkll.github.io/logichub/index.html";
        public static string? oscdimgPath;
        public static string? z7Path;
        public static string? mainTweakPath;
        public static OpenProjectForm openProjectForm;
        static bool isClosingProgrammatically = false;
        public static WinboxMakerConfig? winboxSettings;
        public static WinBoxConfig? winBoxConfig;
        public static WinBoxProject? winBoxProject;
        static bool consoleExporter = false;

        public static string? appdataPath;
        public static string? downloadCachePath;
        public static string? downloadImagesPath;
        public static string? appconfigPath;

        public static string imageFilter = "Image Files (*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tiff)|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tiff";
        public static string batFilter = "Bat scripts (*.bat;*.cmd)|*.bat;*.cmd|All files (*.*)|*.*";
        public static string xmlFilter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
        public static string wimFilter = "WIM Files (*.wim)|*.wim|All Files (*.*)|*.*";

        public static string[] powerSchemes = new string[]
        {
            "SCHEME_CURRENT", //Default
            "381b4222-f694-41f0-9685-ff5bb260df2e", // Balanced
            "8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c", // High performance
            "a1841308-3541-4fab-bc81-f71556f20b4a" // Power saver
        };

        public static string[] default_keyboard_filter_blockList = new string[]
        {
            "Alt+F4",
            "Alt+Space",
            "Alt+Tab",
            "Alt+Win",
            "Application",
            "BrowserBack",
            "BrowserFavorites",
            "BrowserForward",
            "BrowserHome",
            "BrowserRefresh",
            "BrowserSearch",
            "BrowserStop",
            "Ctrl+Alt+Del",
            "Ctrl+Esc",
            "Ctrl+F4",
            "Ctrl+Tab",
            "Ctrl+Win",
            "Ctrl+Win+F",
            "F21",
            "LaunchApp1",
            "LaunchApp2",
            "LaunchMail",
            "LaunchMediaSelect",
            "LShift+LAlt+NumLock",
            "LShift+LAlt+PrintScrn",
            "Shift+Ctrl+Esc",
            "Shift+Win",
            "Windows"
        };


        static string getAppdataSubdirectory(string subdirectory)
        {
            string path = Path.Combine(appdataPath, subdirectory);
            CreateDirectory(path);
            return path;
        }

        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            InitLibwim();
            InitOscdimg();

            z7Path = ResourcePath("7z.exe");
            mainTweakPath = ResourcePath("resources/tweak.reg");

            string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            appdataPath = Path.Combine(localAppDataPath, "Winbox-Maker");
            CreateDirectory(appdataPath);
            appconfigPath = Path.Combine(appdataPath, "config.json");
            downloadCachePath = getAppdataSubdirectory("DownloadCache");
            downloadImagesPath = getAppdataSubdirectory("DownloadImages");

            winboxSettings = WinboxMakerConfig.Load();
            if (winboxSettings != null) winboxSettings.initDefault();

            if (args.Length > 0)
            {
                consoleExporter = true;

                List<string> flags = new List<string>();
                List<string> arguments = new List<string>();
                foreach (var arg in args)
                {
                    if (arg.StartsWith("-") || arg.StartsWith("/"))
                    {
                        flags.Add(arg.Substring(1));
                    }
                    else
                    {
                        arguments.Add(arg);
                    }
                }

                string? inputPath = null;
                if (arguments.Count > 0)
                {
                    inputPath = arguments[0];
                }

                string? outputPath = null;
                if (arguments.Count > 1)
                {
                    outputPath = arguments[1];
                }

                if (inputPath != null && File.Exists(inputPath))
                {
                    consoleConvert(inputPath, outputPath, flags);
                }
                else if (inputPath != null && Directory.Exists(inputPath))
                {
                    string[] files = Directory.GetFiles(inputPath, "*.wnb");
                    if (files.Length > 0)
                    {
                        foreach (string file in files)
                        {
                            consoleConvert(file, outputPath, flags);
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine("there are no *.wnb files in the specified directory");
                    }
                }
                else
                {
                    Console.Error.WriteLine("the input path is not a *.wnb file or a directory containing *.wnb files");
                }

                return;
            }

            IntPtr consoleWindow = GetConsoleWindow();
            ShowWindow(consoleWindow, SW_HIDE);
            openProjectForm = new OpenProjectForm();
            Application.Run(openProjectForm);
        }

        public static string[] FormatExclamationMark(string[] input, bool enable)
        {
            string[] result = new string[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                string s = input[i];

                if (enable)
                {
                    result[i] = s.StartsWith("!") ? s : "!" + s;
                }
                else
                {
                    result[i] = s.StartsWith("!") ? s.Substring(1) : s;
                }
            }

            return result;
        }


        public static bool hasDirectoryNotEmpty(string path)
        {
            if (!Directory.Exists(path))
                return false;

            if (Directory.EnumerateFiles(path).GetEnumerator().MoveNext())
                return true;

            if (Directory.EnumerateDirectories(path).GetEnumerator().MoveNext())
                return true;

            return false;
        }

        public static void Error(string err)
        {
            if (consoleExporter)
            {
                Console.Error.WriteLine(err);
            }
            else
            {
                openProjectForm.editorForm.taskbarManager.SetProgressState(TaskbarProgressBarState.Error, openProjectForm.editorForm.Handle);
                openProjectForm.editorForm.taskbarManager.SetProgressValue(100, 100, openProjectForm.editorForm.Handle);
                MessageBox.Show(err, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void ReplaceAll(string[] items, string oldValue, string newValue)
        {
            for (int index = 0; index < items.Length; index++)
                if (items[index] == oldValue)
                    items[index] = newValue;
        }

        public static void ReplaceAll(List<string> items, string oldValue, string newValue)
        {
            for (int index = 0; index < items.Count; index++)
                if (items[index] == oldValue)
                    items[index] = newValue;
        }

        static void consoleConvert(string path, string? output, List<string> flags)
        {
            path = Path.GetFullPath(path);
            WinBoxProject winBoxProject = new WinBoxProject(path);

            string? err = winBoxProject.GetError();
            if (err != null)
            {
                Console.Error.WriteLine(err);
                return;
            }

            if (!winBoxProject.canExport())
            {
                Console.Error.WriteLine("export is not possible at the moment, the configuration is set incorrectly");
                return;
            }

            if (flags.Contains("i"))
            {
                WinboxConsoleExporter winboxConsoleExporter = new WinboxConsoleExporter(winBoxProject);
                winboxConsoleExporter.ExportIsoInstaller(output);
            }
            else if (flags.Contains("w"))
            {
                WinboxConsoleExporter winboxConsoleExporter = new WinboxConsoleExporter(winBoxProject);
                winboxConsoleExporter.ExportInstallWim(output);
            }
            else if (flags.Contains("d"))
            {
                WinboxConsoleExporter winboxConsoleExporter = new WinboxConsoleExporter(winBoxProject);
                winboxConsoleExporter.ExportInstallEsd(output);
            }
            else if (flags.Contains("r"))
            {
                WinboxConsoleExporter winboxConsoleExporter = new WinboxConsoleExporter(winBoxProject);
                winboxConsoleExporter.ExportImg(output);
            }
            else if (flags.Contains("e"))
            {
                WinboxConsoleExporter winboxConsoleExporter = new WinboxConsoleExporter(winBoxProject);
                winboxConsoleExporter.ExportImg(output, true);
            }
            else
            {
                Console.Error.WriteLine("specify one of the keys to set the output format");
            }
        }

        public static void DelRange(List<string> list, string[] arr)
        {
            list.RemoveAll(s => arr.Contains(s));
        }

        public static bool AnyFileExists(string[] list)
        {
            foreach (string path in list)
            {
                if (File.Exists(path)) return true;
            }
            return false;
        }

        public static bool AllFileExists(string[] list)
        {
            foreach (string path in list)
            {
                if (!File.Exists(path)) return false;
            }
            return true;
        }

        public static void DeleteFiles(string[] list)
        {
            foreach (string path in list)
            {
                if (File.Exists(path)) File.Delete(path);
            }
        }

        static void InitOscdimg()
        {
            switch (RuntimeInformation.ProcessArchitecture)
            {
                case Architecture.X86:
                    oscdimgPath = "oscdimg-x86";
                    break;
                case Architecture.X64:
                    oscdimgPath = "oscdimg-amd64";
                    break;
                case Architecture.Arm:
                    oscdimgPath = "oscdimg-arm";
                    break;
                case Architecture.Arm64:
                    oscdimgPath = "oscdimg-arm64";
                    break;
                default:
                    throw new PlatformNotSupportedException("the program does not support your processor architecture");
            }
            oscdimgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, oscdimgPath, "oscdimg.exe");
        }

        static void InitLibwim()
        {
            string libBaseDir = AppDomain.CurrentDomain.BaseDirectory;
            string libDir = "runtimes";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                libDir = Path.Combine(libDir, "win-");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                libDir = Path.Combine(libDir, "linux-");
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                libDir = Path.Combine(libDir, "osx-");

            switch (RuntimeInformation.ProcessArchitecture)
            {
                case Architecture.X86:
                    libDir += "x86";
                    break;
                case Architecture.X64:
                    libDir += "x64";
                    break;
                case Architecture.Arm:
                    libDir += "arm";
                    break;
                case Architecture.Arm64:
                    libDir += "arm64";
                    break;
            }
            libDir = Path.Combine(libDir, "native");

            string libPath = null;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                libPath = Path.Combine(libBaseDir, libDir, "libwim-15.dll");
                if (!File.Exists(libPath))
                {
                    libPath = Path.Combine(libBaseDir, "libwim-15.dll");
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                libPath = Path.Combine(libBaseDir, libDir, "libwim.so");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                libPath = Path.Combine(libBaseDir, libDir, "libwim.dylib");
            }

            if (libPath == null)
                throw new PlatformNotSupportedException($"Unable to find native library.");
            if (!File.Exists(libPath))
                throw new PlatformNotSupportedException($"Unable to find native library [{libPath}].");

            Wim.GlobalInit(libPath, InitFlags.None);
        }

        public static void SwitchForm(Form self, Form form)
        {
            form.Show();

            if (form != openProjectForm)
            {
                form.FormClosed += (s, args) =>
                {
                    if (!isClosingProgrammatically)
                    {
                        openProjectForm.Close();
                    }
                };
            }

            if (self == openProjectForm)
            {
                self.Hide();
            }
            else
            {
                isClosingProgrammatically = true;
                self.Close();
                isClosingProgrammatically = false;
            }
        }

        public static bool IsDirectoryEmpty(string path)
        {
            string[] files = Directory.GetFiles(path);
            string[] directories = Directory.GetDirectories(path);

            return files.Length == 0 && directories.Length == 0;
        }

        public static bool IsPathInsideDirectory(string path, string directory)
        {
            string fullPath = Path.GetFullPath(path);
            string fullDirectory = Path.GetFullPath(directory);

            if (!fullDirectory.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                fullDirectory += Path.DirectorySeparatorChar;
            }

            return fullPath.StartsWith(fullDirectory, StringComparison.OrdinalIgnoreCase);
        }

        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static async Task CopyFileAsync(string sourceFilePath, string destinationFilePath, Action<int> progressBar)
        {
            long totalBytes = new FileInfo(sourceFilePath).Length;
            long bytesCopied = 0;

            using (FileStream sourceStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (FileStream destinationStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = new byte[81920];
                int bytesRead;

                while ((bytesRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await destinationStream.WriteAsync(buffer, 0, bytesRead);
                    bytesCopied += bytesRead;

                    progressBar((int)((bytesCopied * 100) / totalBytes));
                }
            }
        }

        public static void OpenWebPage(string url)
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        public static void Execute(string exec, string args)
        {
            Process process = new Process();
            process.StartInfo.FileName = exec;
            process.StartInfo.Arguments = args;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
        }

        public static async Task<string> ExecuteAsync(string exec, string args, string? workingDirectory = null, string? outputPath = null)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = exec;
                process.StartInfo.Arguments = args;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                if (workingDirectory != null)
                    process.StartInfo.WorkingDirectory = workingDirectory;

                if (outputPath != null && Directory.Exists(outputPath))
                {
                    outputPath = Path.Combine(outputPath, "logs", exec + "_" + CalculateMD5(args) + ".txt");
                }

                if (outputPath != null)
                {
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;

                    CreateDirectory(Path.GetDirectoryName(outputPath));

                    var outputLines = new List<string>();
                    string outputText = "";

                    process.OutputDataReceived += (s, e) => {
                        if (e.Data != null) {
                            outputLines.Add("[OUT] " + e.Data);
                            outputText += e.Data + "\n\r";
                        }
                    };
                    process.ErrorDataReceived += (s, e) => { if (e.Data != null) outputLines.Add("[ERR] " + e.Data); };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    await process.WaitForExitAsync();

                    using FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None);
                    using StreamWriter writer = new StreamWriter(fs, Encoding.UTF8);
                    writer.WriteLine("[EXEC] " + exec);
                    writer.WriteLine("[ARGS] " + args);
                    writer.WriteLine("[DIR ] " + workingDirectory);
                    foreach (string line in outputLines)
                    {
                        writer.WriteLine(line);
                    }
                    writer.Flush();

                    return outputText; // вернет результат ТОЛЬКО если идет запись в лог
                }
                else
                {
                    process.Start();
                    await process.WaitForExitAsync();
                }
            }

            return "";
        }

        public static string ConvertToPowerShellFormat(string input)
        {
            StringBuilder result = new StringBuilder();
            bool inQuotes = false;
            string currentArg = "";

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                if (c == '\"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ' ' && !inQuotes)
                {
                    if (currentArg.Length > 0)
                    {
                        result.Append($"\"{currentArg}\", ");
                        currentArg = "";
                    }
                }
                else
                {
                    currentArg += c;
                }
            }

            if (currentArg.Length > 0)
            {
                result.Append($"\"{currentArg}\"");
            }

            return result.ToString();
        }

        public static async Task CopyFilesRecursivelyAsync(string sourceDir, string targetDir)
        {
            CreateDirectory(targetDir);

            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(targetDir, fileName);
                await CopyFileAsync(file, destFile);
            }

            foreach (string directory in Directory.GetDirectories(sourceDir))
            {
                string newTargetDir = Path.Combine(targetDir, Path.GetFileName(directory));
                await CopyFilesRecursivelyAsync(directory, newTargetDir);
            }
        }

        public static async Task CopyFileAsync(string sourceFile, string destFile)
        {
            FileAttributes? oldFileAttributes = null;

            if (File.Exists(destFile))
            {
                oldFileAttributes = File.GetAttributes(destFile);
                if ((oldFileAttributes & FileAttributes.ReadOnly) != 0)
                    File.SetAttributes(destFile, (FileAttributes)(oldFileAttributes & ~FileAttributes.ReadOnly));

                File.Delete(destFile);
            }

            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                using (FileStream destinationStream = new FileStream(destFile, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await sourceStream.CopyToAsync(destinationStream);
                }
            }

            if (oldFileAttributes != null)
            {
                File.SetAttributes(destFile, (FileAttributes)oldFileAttributes);
            }
        }

        public static async Task WriteFileAsync(string destFile, string text)
        {
            FileAttributes? oldFileAttributes = null;

            if (File.Exists(destFile))
            {
                oldFileAttributes = File.GetAttributes(destFile);
                if ((oldFileAttributes & FileAttributes.ReadOnly) != 0)
                    File.SetAttributes(destFile, (FileAttributes)(oldFileAttributes & ~FileAttributes.ReadOnly));

                File.Delete(destFile);
            }

            using (FileStream destinationStream = new FileStream(destFile, FileMode.Create, FileAccess.Write, FileShare.None))
            using (StreamWriter writer = new StreamWriter(destinationStream))
            {
                await writer.WriteAsync(text);
            }

            if (oldFileAttributes != null)
            {
                File.SetAttributes(destFile, (FileAttributes)oldFileAttributes);
            }
        }


        public static void SetAttributesRecursive(string path, FileAttributes attributes)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException(path);

            // Изменяем атрибуты самой папки
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            dirInfo.Attributes = attributes;

            // Изменяем атрибуты всех файлов внутри
            foreach (var file in Directory.GetFiles(path, "*", SearchOption.AllDirectories))
            {
                File.SetAttributes(file, attributes);
            }

            // Изменяем атрибуты всех вложенных папок
            foreach (var dir in Directory.GetDirectories(path, "*", SearchOption.AllDirectories))
            {
                DirectoryInfo subDirInfo = new DirectoryInfo(dir);
                subDirInfo.Attributes = attributes;
            }
        }

        private static async Task<long> RecursiveGetUsedSpace(UdfReader cd, DiscDirectoryInfo currentDir, string[] blacklist)
        {
            long usedSpace = 0;

            foreach (DiscFileInfo file in currentDir.GetFiles())
            {
                if (!blacklist.Contains(file.FullName))
                {
                    usedSpace += file.Length;
                }
            }

            foreach (DiscDirectoryInfo dir in currentDir.GetDirectories())
            {
                usedSpace += await RecursiveGetUsedSpace(cd, dir, blacklist);
            }

            return usedSpace;
        }

        private static async Task RecursiveUnpack(UdfReader cd, DiscDirectoryInfo currentDir, string outputDirectory, Action<int> processValue, long globalUsedSpace, long copied, string[] blacklist)
        {
            foreach (DiscFileInfo file in currentDir.GetFiles())
            {
                if (!blacklist.Contains(file.FullName))
                {
                    string outputPath = Path.Combine(outputDirectory, file.FullName);
                    CreateDirectory(Path.GetDirectoryName(outputPath));

                    using (var wimFile = cd.OpenFile(file.FullName, FileMode.Open, FileAccess.Read))
                    {
                        using (FileStream outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                        {
                            byte[] buffer = new byte[1024 * 64 * 4];
                            int bytesRead;

                            while ((bytesRead = await wimFile.ReadAsync(buffer, 0, buffer.Length)) > 0)
                            {
                                await outputStream.WriteAsync(buffer, 0, bytesRead);
                                copied += bytesRead;
                                processValue((int)((copied * 100) / globalUsedSpace));
                            }
                        }
                    }
                }
            }

            foreach (DiscDirectoryInfo dir in currentDir.GetDirectories())
            {
                CreateDirectory(Path.Combine(outputDirectory, dir.FullName));
                await RecursiveUnpack(cd, dir, outputDirectory, processValue, globalUsedSpace, copied, blacklist);
            }
        }

        public static async Task<bool> UnpackUdfIso(string isoPath, string outputDirectory, Action<int> processValue, string[] blacklist)
        {
            using (FileStream isoStream = File.Open(isoPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                UdfReader cd = new UdfReader(isoStream);
                await RecursiveUnpack(cd, cd.Root, outputDirectory, processValue, await RecursiveGetUsedSpace(cd, cd.Root, blacklist), 0, blacklist);
                return cd.Exists(@"sources\install.esd");
            }
        }

        public static string EscapeForRegFile(string input)
        {
            return "\"" + input.Replace("\\", "\\\\")
                        .Replace("\"", "\\\"") + "\"";
        }

        public static string ResourcePath(string path)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        }
        public static bool IsDirectoryNotEmpty(string path)
        {
            if (Directory.Exists(path))
            {
                return Directory.GetFiles(path).Length > 0 || Directory.GetDirectories(path).Length > 0;
            }
            return false;
        }

        public static bool isTweakEnabled(WinBoxConfig winBoxConfig, String checkTweak)
        {
            if (winBoxConfig.TweakList != null)
            {
                foreach (String tweak in winBoxConfig.TweakList)
                {
                    if (tweak == checkTweak)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void setTweakEnabled(WinBoxConfig winBoxConfig, String setTweak, bool state)
        {
            if (winBoxConfig.TweakList == null)
            {
                winBoxConfig.TweakList = [];
            }

            if (state)
            {
                winBoxConfig.TweakList.Add(setTweak);
            }
            else
            {
                winBoxConfig.TweakList.RemoveAll(s => s == setTweak);
            }
        }

        public static bool isCheckEnabled(List<string>? checkList, string checkCheck)
        {
            if (checkList == null)
                return false;

            foreach (var check in checkList)
            {
                if (check == checkCheck)
                    return true;
            }

            return false;
        }

        public static void setCheckEnabled(List<string>? checkList, string setCheck, bool state)
        {
            if (checkList == null)
                return;

            if (state)
            {
                if (!checkList.Contains(setCheck))
                    checkList.Add(setCheck);
            }
            else
            {
                checkList.RemoveAll(s => s == setCheck);
            }
        }

        public static string? getBlobPath(WinBoxConfig winBoxConfig, string blobname)
        {
            string blobPath = ResourcePath(Path.Combine("blobs", winBoxConfig.Architecture, blobname));
            if (File.Exists(blobPath))
            {
                return blobPath;
            }

            blobPath = ResourcePath(Path.Combine("blobs", "def", blobname));
            if (File.Exists(blobPath))
            {
                return blobPath;
            }

            return null;
        }

        public static string? getBlobPathFromArch(WinBoxConfig winBoxConfig, string blobname, string arch)
        {
            string blobPath = ResourcePath(Path.Combine("blobs", arch, blobname));
            if (File.Exists(blobPath))
            {
                return blobPath;
            }

            blobPath = ResourcePath(Path.Combine("blobs", "def", blobname));
            if (File.Exists(blobPath))
            {
                return blobPath;
            }

            return null;
        }

        public async static Task executeBuildEvent(string directory, string buildEvent, string? args = null)
        {
            string buildEventFilePath = Path.Combine(directory, "winbox_temp", "build_event.bat");

            await File.WriteAllTextAsync(buildEventFilePath, buildEvent);

            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/C winbox_temp\\build_event.bat {args}";
            process.StartInfo.WorkingDirectory = directory;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            await process.WaitForExitAsync();

            File.Delete(buildEventFilePath);
        }

        public static string CalculateMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        public static string ReplaceAndPrependBackslash(string input)
        {
            string modified = input.Replace('/', '\\');

            if (!modified.StartsWith("\\"))
            {
                modified = "\\" + modified;
            }

            return modified;
        }

        static public async Task downloadFile(string url, string path, Action<int>? processValue = null)
        {
            var tcs = new TaskCompletionSource<bool>();
            bool successfully = false;

            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += (sender, e) =>
                {
                    if (processValue != null)
                    {
                        processValue(e.ProgressPercentage);
                    }
                };

                wc.DownloadFileCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        tcs.SetException(e.Error);
                    }
                    else
                    {
                        successfully = true;
                        tcs.SetResult(true);
                    }
                };

                try
                {
                    wc.DownloadFileAsync(new Uri(url), path);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }

            await tcs.Task;

            if (successfully) File.WriteAllText(getDownloadTriggerFilePath(path), "");
        }

        static public bool isFileDownloaded(string path)
        {
            return File.Exists(path) && File.Exists(getDownloadTriggerFilePath(path));
        }

        static public string getDownloadTriggerFilePath(string path)
        {
            return Path.Combine(Path.GetDirectoryName(path), Path.GetFileName(path) + ".downloaded");
        }

        static public string getTimestamp()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        static public void appendLog(string filePath, string textToAppend)
        {
            Program.CreateDirectory(Path.GetDirectoryName(filePath)!);
            File.AppendAllText(filePath, $"[{getTimestamp()}] {textToAppend}\r\n");
        }
    }
}