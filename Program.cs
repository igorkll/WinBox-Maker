using DiscUtils;
using DiscUtils.Udf;
using ManagedWimLib;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
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


        public const string version = "WinBox-Maker 0.1.0";
        public const string logichubUrl = "https://igorkll.github.io/logichub/index.html";
        public static string? oscdimgPath;
        public static Form openProjectForm;
        static bool isClosingProgrammatically = false;

        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            InitLibwim();
            InitOscdimg();

            if (args.Length > 0)
            {
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

        static void consoleConvert(string path, string? output, List<string> flags)
        {
            WinBoxProject winBoxProject = new WinBoxProject(path);
            string? err = winBoxProject.GetError();
            if (err != null)
            {
                Console.Error.WriteLine(err);
                return;
            }

            WinboxConsoleExporter winboxConsoleExporter = new WinboxConsoleExporter(winBoxProject);
            if (flags.Contains("i"))
            {
                winboxConsoleExporter.ExportIsoInstaller(output);
            }
            else if (flags.Contains("w"))
            {
                winboxConsoleExporter.ExportInstallWim(output);
            }
            else if (flags.Contains("r"))
            {
                winboxConsoleExporter.ExportImgPartition(output);
            }
            else
            {
                Console.Error.WriteLine("specify one of the keys to set the output format");
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
            oscdimgPath = Path.Combine(oscdimgPath, "oscdimg.exe");
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
                if (!File.Exists(libPath)) {
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

        public static async Task ExecuteAsync(string exec, string args)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = exec;
                process.StartInfo.Arguments = args;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                await process.WaitForExitAsync();
            }
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
            Directory.CreateDirectory(targetDir);

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
                File.Delete(destFile);
            }

            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.None)) {
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
                if (!blacklist.Contains(file.FullName)) {
                    string outputPath = Path.Combine(outputDirectory, file.FullName);
                    Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

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
                Directory.CreateDirectory(Path.Combine(outputDirectory, dir.FullName));
                await RecursiveUnpack(cd, dir, outputDirectory, processValue, globalUsedSpace, copied, blacklist);
            }
        }

        public static async Task UnpackUdfIso(string isoPath, string outputDirectory, Action<int> processValue, string[] blacklist)
        {
            using (FileStream isoStream = File.Open(isoPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                UdfReader cd = new UdfReader(isoStream);
                await RecursiveUnpack(cd, cd.Root, outputDirectory, processValue, await RecursiveGetUsedSpace(cd, cd.Root, blacklist), 0, blacklist);
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
    }
}