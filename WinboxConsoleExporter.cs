using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace WinBox_Maker
{
    public class WinboxConsoleExporter
    {
        WinBoxProject winBoxProject;
        const int totalLength = 15;
        const int maxValue = 100;

        public WinboxConsoleExporter(WinBoxProject winBoxProject)
        {
            this.winBoxProject = winBoxProject;
            if (winBoxProject.NeedLoadWindows())
            {
                winBoxProject.LoadWindowsImageAsync(UpdateProcessName, UpdateProcessValue).Wait();
            }
        }

        private void UpdateProcessName(string text)
        {
            Console.WriteLine($"> {text}");
        }

        private void UpdateProcessValue(int Value)
        {
            int filledLength = (int)((Value / (double)maxValue) * totalLength);
            string progressBar = "[" + new string('#', filledLength) + new string(' ', totalLength - filledLength) + "]";
            Console.Write($"\r{progressBar} {Value}%");
        }

        public void ExportIsoInstaller(string? path)
        {
            path = path ?? Path.Combine(winBoxProject.buildDirectoryPath, $"{winBoxProject.winBoxConfig.WinboxName}.iso");
            WindowsDescription windowsDescription = new WindowsDescription
            {
                name = winBoxProject.winBoxConfig.WinboxName,
                description = winBoxProject.winBoxConfig.WinboxDescription
            };
            winBoxProject.BuildIsoAsync(UpdateProcessName, UpdateProcessValue, path, windowsDescription).Wait();
        }

        public void ExportInstallWim(string? path)
        {
            path = path ?? Path.Combine(winBoxProject.buildDirectoryPath, $"{winBoxProject.winBoxConfig.WinboxName}.wim");
            WindowsDescription windowsDescription = new WindowsDescription
            {
                name = winBoxProject.winBoxConfig.WinboxName,
                description = winBoxProject.winBoxConfig.WinboxDescription
            };
            winBoxProject.BuildWimAsync(UpdateProcessName, UpdateProcessValue, path, windowsDescription).Wait();
        }
    }
}
