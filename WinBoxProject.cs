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
        WinBoxConfig winBoxConfig;
        string wnbFilePath;
        string baseDirectoryPath;
        string buildDirectoryPath;
        string resourceDirectoryPath;
        string? err;

        public WinBoxProject(string wnbFilePath)
        {
            this.wnbFilePath = wnbFilePath;
            baseDirectoryPath = Path.GetDirectoryName(wnbFilePath);
            buildDirectoryPath = Path.Combine(baseDirectoryPath, "winbox_build");
            resourceDirectoryPath = Path.Combine(baseDirectoryPath, "winbox_resource");

            WinBoxConfig? config = WinBoxConfig.Load(wnbFilePath);
            if (config == null)
            {
                MessageBox.Show(err, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                err = "failed to load .wnb config";
            }
            else
            {
                MessageBox.Show("test", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                winBoxConfig = config;
            }
        }

        public string? GetError()
        {
            return err;
        }
    }
}
