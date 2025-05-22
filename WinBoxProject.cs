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
        WinBoxConfig? winBoxConfig;
        string wnbFilePath;
        string baseDirectoryPath;
        string buildDirectoryPath;
        string resourceDirectoryPath;
        string name;
        string? err;

        public WinBoxProject(string wnbFilePath)
        {
            WinBoxConfig? config;
            if (File.Exists(wnbFilePath))
            {
                config = WinBoxConfig.Load(wnbFilePath);
                if (config == null)
                {
                    err = "failed to load .wnb config";
                    return;
                }
            }
            else
            {
                config = new WinBoxConfig();
                config.Save(wnbFilePath);
            }

            winBoxConfig = config;
            this.wnbFilePath = wnbFilePath;
            baseDirectoryPath = Path.GetDirectoryName(wnbFilePath);
            buildDirectoryPath = Path.Combine(baseDirectoryPath, "winbox_build");
            resourceDirectoryPath = Path.Combine(baseDirectoryPath, "winbox_resource");
            name = Path.GetFileName(baseDirectoryPath);
        }

        public string? GetName()
        {
            return name;
        }

        public string? GetError()
        {
            return err;
        }
    }
}
