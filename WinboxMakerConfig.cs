using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WinBox_Maker
{
    public class WinboxMakerConfig
    {
        public string? path_msbuild { get; set; }
        public string? path_cmake { get; set; }
        public string? path_pip { get; set; }

        string? FindProgram(string name)
        {
            string pathVariable = Environment.GetEnvironmentVariable("PATH");
            string[] paths = pathVariable.Split(Path.PathSeparator);
            string programPath = null;

            foreach (string path in paths)
            {
                string fullPath = Path.Combine(path, name);
                if (File.Exists(fullPath))
                {
                    programPath = fullPath;
                    break;
                }
            }

            return programPath;
        }

        public void AutoDetect(bool forceSave=false)
        {
            string? old_path_msbuild = path_msbuild;
            string? old_path_cmake = path_cmake;
            string? old_path_pip = path_pip;

            if (!File.Exists(path_msbuild)) path_msbuild = null;
            if (!File.Exists(path_cmake)) path_cmake = null;
            if (!File.Exists(path_pip)) path_pip = null;

            path_msbuild = path_msbuild ?? FindProgram("msbuild.exe");
            path_cmake = path_cmake ?? FindProgram("cmake.exe");
            path_pip = path_pip ?? FindProgram("pip.exe");

            if (forceSave ||
                path_msbuild != old_path_msbuild ||
                path_cmake != old_path_cmake ||
                path_pip != old_path_pip) {
                Save();
            }
        }

        public void Save()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(Program.appconfigPath, json);
        }

        public static WinboxMakerConfig? Load()
        {
            WinboxMakerConfig? winBoxMakerConfig;

            try
            {
                string json = File.ReadAllText(Program.appconfigPath);
                winBoxMakerConfig = JsonSerializer.Deserialize<WinboxMakerConfig>(json);
                if (winBoxMakerConfig == null)
                {
                    winBoxMakerConfig = new WinboxMakerConfig();
                }
                winBoxMakerConfig.AutoDetect();
                return winBoxMakerConfig;
            }
            catch (Exception ex) { }

            winBoxMakerConfig = new WinboxMakerConfig();
            winBoxMakerConfig.AutoDetect(true);

            return winBoxMakerConfig;
        }
    }
}
