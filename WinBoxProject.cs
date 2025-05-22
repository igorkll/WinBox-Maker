using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinBox_Maker
{
    internal class WinBoxProject
    {
        string wnbFilePath;
        string baseDirectoryPath;
        string buildDirectoryPath;
        string resourceDirectoryPath;

        public WinBoxProject(string wnbFilePath)
        {
            this.wnbFilePath = wnbFilePath;
            baseDirectoryPath = Path.GetDirectoryName(wnbFilePath);
            buildDirectoryPath = Path.Combine(baseDirectoryPath, "winbox_build");
            resourceDirectoryPath = Path.Combine(baseDirectoryPath, "winbox_resource");
        }
    }
}
