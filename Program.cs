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
            self.Dispose();
            form.Show();
        }
    }
}