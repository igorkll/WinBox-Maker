namespace WinBox_Maker
{
    internal static class Program
    {
        public static string version = "WinBox-Maker 0.0.0";

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new OpenProjectForm());
        }

        public static void SwitchForm(Form self, Form form)
        {
            self.Hide();
            form.Show();
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
    }
}