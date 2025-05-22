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
    }
}