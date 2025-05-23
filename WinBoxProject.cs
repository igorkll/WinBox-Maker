using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DiscUtils.Udf;

namespace WinBox_Maker
{
    public class WinBoxProject
    {
        const string resourcesDirectoryName = "winbox_resources";
        const string bigResourcesDirectoryName = "winbox_bigResources";
        public WinBoxConfig winBoxConfig;
        string wnbFilePath;
        string baseDirectoryPath;
        string buildDirectoryPath;
        string resourcesDirectoryPath;
        string bigResourcesDirectoryPath;
        string tempDirectoryPath;
        string name;
        string? err;

        public WinBoxProject(string wnbFilePath)
        {
            winBoxConfig = new WinBoxConfig();
            this.wnbFilePath = wnbFilePath;
            baseDirectoryPath = Path.GetDirectoryName(wnbFilePath) ?? "";
            buildDirectoryPath = Path.Combine(baseDirectoryPath, "winbox_build");
            resourcesDirectoryPath = Path.Combine(baseDirectoryPath, resourcesDirectoryName);
            bigResourcesDirectoryPath = Path.Combine(baseDirectoryPath, bigResourcesDirectoryName);
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
            Program.CreateDirectory(resourcesDirectoryPath);
            Program.CreateDirectory(bigResourcesDirectoryPath);
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
                openFileDialog.InitialDirectory = resourcesDirectoryPath;
                openFileDialog.Filter = filter;
                openFileDialog.Title = "Select Resource";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    string fileName = Path.GetFileName(filePath);
                    if (Program.IsPathInsideDirectory(filePath, resourcesDirectoryPath))
                    {
                        return Path.Combine(resourcesDirectoryName, fileName);
                    }
                    else if (Program.IsPathInsideDirectory(filePath, bigResourcesDirectoryPath))
                    {
                        return Path.Combine(bigResourcesDirectoryName, fileName);
                    }

                    DialogResult result = MessageBox.Show("the file is not located in the project's resource directory, if you use it like this, then the project config will have the absolute path to the file, which will make it impossible to build on another computer. do you want to copy the file so that you don't have to use an absolute path?", "copy the file?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        string projectFolderToCopy;
                        FileInfo fileInfo = new FileInfo(filePath);
                        if (fileInfo.Length > (1024 * 1024 * 1024))
                        {
                            projectFolderToCopy = bigResourcesDirectoryName;
                        }
                        else
                        {
                            projectFolderToCopy = resourcesDirectoryName;
                        }

                        File.Copy(filePath, Path.Combine(baseDirectoryPath, projectFolderToCopy, fileName), true);
                        MessageBox.Show("the file has been copied to the project folder", null, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return Path.Combine(projectFolderToCopy, fileName);
                    }
                    else if (result == DialogResult.No)
                    {
                        return filePath;
                    }
                }
            }

            return null;
        }

        public string GetAbsoluteResourcePath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }
            else
            {
                return Path.Combine(baseDirectoryPath, path);
            }
        }

        public string[] GetWindowsVersionsInImage(string imagePath)
        {
            imagePath = GetAbsoluteResourcePath(imagePath);
            using (FileStream isoStream = File.Open(imagePath, FileMode.Open))
            {
                UdfReader cd = new UdfReader(isoStream);
                return cd.GetFileSystemEntries(@"");
            }
        }
    }
}
