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
        const int totalLength = 50;
        const int maxValue = 100;
        bool needNewLine = false;

        public WinboxConsoleExporter(WinBoxProject winBoxProject)
        {
            this.winBoxProject = winBoxProject;
            if (winBoxProject.NeedLoadWindows())
            {
                winBoxProject.LoadWindowsImageAsync(UpdateProcessName, UpdateProcessValue).Wait();
            }
            NewLine();
        }

        public void NewLine()
        {
            if (needNewLine)
            {
                Console.WriteLine("");
                needNewLine = false;
            }
        }

        private void UpdateProcessName(string text)
        {
            NewLine();
            Console.WriteLine($"> {text}");
        }

        private void UpdateProcessValue(int Value)
        {
            int filledLength = (int)((Value / (double)maxValue) * totalLength);
            string progressBar = "[" + new string('#', filledLength) + new string(' ', totalLength - filledLength) + "]";
            Console.Write($"\r{progressBar} {Value}%   ");
            needNewLine = true;
        }

        private string getExportPath(string? path, string ext, string? appendName)
        {
            appendName = appendName ?? "";
            if (path == null)
            {
                path = Path.Combine(winBoxProject.buildDirectoryPath, $"{winBoxProject.winBoxConfig.WinboxName}{appendName}.{ext}");
            }
            else if (Directory.Exists(path))
            {
                path = Path.Combine(path, $"{winBoxProject.winBoxConfig.WinboxName}{appendName}.{ext}");
            }
            else if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(winBoxProject.buildDirectoryPath, path);
            }
            return path;
        }

        public void ExportIsoInstaller(string? path)
        {
            path = getExportPath(path, "iso", null);
            Console.WriteLine($">> exporting iso installer from {winBoxProject.GetName()} to: {path}");
            WindowsDescription windowsDescription = new WindowsDescription
            {
                name = winBoxProject.winBoxConfig.WinboxName,
                description = winBoxProject.winBoxConfig.WinboxDescription
            };
            winBoxProject.BuildIsoAsync(UpdateProcessName, UpdateProcessValue, path, windowsDescription).Wait();
            NewLine();
        }

        public void ExportInstallWim(string? path)
        {
            path = getExportPath(path, "wim", null);
            Console.WriteLine($">> exporting install.wim from {winBoxProject.GetName()} to: {path}");
            WindowsDescription windowsDescription = new WindowsDescription
            {
                name = winBoxProject.winBoxConfig.WinboxName,
                description = winBoxProject.winBoxConfig.WinboxDescription
            };
            winBoxProject.BuildWimAsync(UpdateProcessName, UpdateProcessValue, path, windowsDescription).Wait();
            NewLine();
        }

        public void ExportImgPartition(string? path)
        {
            path = getExportPath(path, "img", " (partition)");
            Console.WriteLine($">> exporting img partition from {winBoxProject.GetName()} to: {path}");
            WindowsDescription windowsDescription = new WindowsDescription
            {
                name = winBoxProject.winBoxConfig.WinboxName,
                description = winBoxProject.winBoxConfig.WinboxDescription
            };
            winBoxProject.BuildImgPartitionAsync(UpdateProcessName, UpdateProcessValue, path, windowsDescription).Wait();
            NewLine();
        }
    }
}
