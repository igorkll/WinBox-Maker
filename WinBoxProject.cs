using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WinBox_Maker
{
    public class WinBoxProject
    {
        public WinBoxConfig winBoxConfig;
        string wnbFilePath;
        string baseDirectoryPath;
        string buildDirectoryPath;
        string resourceDirectoryPath;
        string name;
        string? err;

        public WinBoxProject(string wnbFilePath)
        {
            winBoxConfig = new WinBoxConfig();
            this.wnbFilePath = wnbFilePath;
            baseDirectoryPath = Path.GetDirectoryName(wnbFilePath);
            buildDirectoryPath = Path.Combine(baseDirectoryPath, "winbox_build");
            resourceDirectoryPath = Path.Combine(baseDirectoryPath, "winbox_resource");
            name = Path.GetFileName(baseDirectoryPath);

            if (File.Exists(wnbFilePath))
            {
                winBoxConfig = WinBoxConfig.Load(wnbFilePath);
                if (winBoxConfig == null)
                {
                    err = "failed to load .wnb config";
                    return;
                }
            }
            else
            {
                winBoxConfig.Save(wnbFilePath);
            }
        }

        public string? GetName()
        {
            return name;
        }

        public string? GetError()
        {
            return err;
        }

        public void SaveConfig()
        {
            winBoxConfig.Save(wnbFilePath);
        }
    }
}
