namespace WinBox_Maker
{
    internal static class Program
    {
        static public string version = "WinBox-Maker 0.0.0";

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Mainform());
        }
    }
}