using ManagedWimLib;
using System.Runtime.InteropServices;

namespace WinBox_Maker
{
    internal static class Program
    {
        public static string version = "WinBox-Maker 0.0.0";
        public static Form openProjectForm;
        static bool isClosingProgrammatically = false;

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            switch (RuntimeInformation.ProcessArchitecture)
            {
                case Architecture.X86:
                    Wim.GlobalInit("winbox/libwim_x86/libwim-15.dll", InitFlags.None);
                    break;

                case Architecture.X64:
                    Wim.GlobalInit("winbox/libwim_x64/libwim-15.dll", InitFlags.None);
                    break;

                case Architecture.Arm64:
                    Wim.GlobalInit("winbox/libwim_arm64/libwim-15.dll", InitFlags.None);
                    break;

                default:
                    MessageBox.Show("failed to find native libwim-15.dll for you CPU", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                    break;
            }

            openProjectForm = new OpenProjectForm();
            Application.Run(openProjectForm);
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

        public static async Task CopyFileAsync(string sourceFilePath, string destinationFilePath, ProgressBar progressBar)
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

                    progressBar.Value = (int)((bytesCopied * 100) / totalBytes);
                }
            }
        }
    }
}