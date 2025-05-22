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
        string bigResourceDirectoryPath;
        string tempDirectoryPath;
        string name;
        string? err;

        public WinBoxProject(string wnbFilePath)
        {
            winBoxConfig = new WinBoxConfig();
            this.wnbFilePath = wnbFilePath;
            baseDirectoryPath = Path.GetDirectoryName(wnbFilePath) ?? "";
            buildDirectoryPath = Path.Combine(baseDirectoryPath, "winbox_build");
            resourceDirectoryPath = Path.Combine(baseDirectoryPath, "winbox_resources");
            bigResourceDirectoryPath = Path.Combine(baseDirectoryPath, "winbox_bigResources");
            tempDirectoryPath = Path.Combine(baseDirectoryPath, "winbox_temp");
            name = Path.GetFileName(baseDirectoryPath);

            if (File.Exists(wnbFilePath))
            {
                WinBoxConfig? config = WinBoxConfig.Load(wnbFilePath);
                if (config == null)
                {
                    err = "failed to load .wnb config";
                    return;
                }
                winBoxConfig = config;
            }
            else
            {
                winBoxConfig.Save(wnbFilePath);
            }

            Program.CreateDirectory(buildDirectoryPath);
            Program.CreateDirectory(resourceDirectoryPath);
            Program.CreateDirectory(bigResourceDirectoryPath);
            Program.CreateDirectory(tempDirectoryPath);

            string gitignorePath = Path.Combine(baseDirectoryPath, ".gitignore");
            if (!File.Exists(gitignorePath)) {
                File.WriteAllText(gitignorePath, $"## WinBox-Maker\n\nwinbox_build\nwinbox_temp\nwinbox_bigResources\n");
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

        public string? SelectResource(string filter)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = resourceDirectoryPath;
                openFileDialog.Filter = filter;
                openFileDialog.Title = "Select Resource";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    string fileName = Path.GetFileName(filePath);
                    if (!Program.IsPathInsideDirectory(filePath, resourceDirectoryPath))
                    {
                        DialogResult result = MessageBox.Show("The file is not located in the project's resources folder, so you need to copy it to use it. Continue?", "copy the file?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            File.Copy(filePath, Path.Combine(resourceDirectoryPath, fileName));
                            MessageBox.Show("the file has been copied to the project folder", null, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    return fileName;
                }
            }

            return null;
        }
    }
}
