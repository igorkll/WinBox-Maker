using ManagedWimLib;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

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


        public const string version = "WinBox-Maker 1.0";
        public const string logichubUrl = "https://igorkll.github.io/logichub/index.html";
        public static Form openProjectForm;
        static bool isClosingProgrammatically = false;

        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            InitLibwim();

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
    }
}