using DiscUtils.Raw;
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
        public string? path_cargo { get; set; }
        public string? path_qemu_folder { get; set; }

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

        string? checkMsbuild(char disk)
        {
            string[] versions = { "2022", "2019", "2017", "2015", "2013", "2012" };
            string[] editions = { "Community", "Professional", "Enterprise", "Ultimate" };

            foreach (string version in versions)
            {
                foreach (string edition in editions)
                {
                    string path = $"{disk}:\\Program Files\\Microsoft Visual Studio\\{version}\\{edition}\\MSBuild\\Current\\Bin\\MSBuild.exe";
                    if (File.Exists(path)) return path;

                    path = $"{disk}:\\Program Files (x86)\\Microsoft Visual Studio\\{version}\\{edition}\\MSBuild\\Current\\Bin\\MSBuild.exe";
                    if (File.Exists(path)) return path;
                }
            }

            return null;
        }

        string? checkQemu(char disk)
        {
            bool checkQemuPath(string path)
            {
                return Directory.Exists(path) && File.Exists(Path.Combine(path, "qemu-img.exe"));
            }

            string path = $"{disk}:\\Program Files (x86)\\qemu";
            if (checkQemuPath(path)) return path;

            path = $"{disk}:\\Program Files\\qemu";
            if (checkQemuPath(path)) return path;

            return null;
        }

        string? checkCmake(char disk)
        {
            string path = $"{disk}:\\Program Files\\CMake\\bin\\cmake.exe";
            if (File.Exists(path)) return path;

            path = $"{disk}:\\Program Files (x86)\\CMake\\bin\\cmake.exe";
            if (File.Exists(path)) return path;

            return null;
        }

        string? findAny(Func<char, string?> check)
        {
            string? path = check('C');
            if (path != null) return path;

            path = check('D');
            if (path != null) return path;

            for (char letter = 'A'; letter <= 'Z'; letter++)
            {
                if (letter != 'C' && letter != 'D')
                {
                    path = check(letter);
                    if (path != null) return path;
                }
            }

            return null;
        }

        public void AutoDetect(bool forceSave=false)
        {
            string? old_path_msbuild = path_msbuild;
            string? old_path_cmake = path_cmake;
            string? old_path_pip = path_pip;
            string? old_path_cargo = path_cargo;
            string? old_path_qemu_folder = path_qemu_folder;

            if (!File.Exists(path_msbuild)) path_msbuild = null;
            if (!File.Exists(path_cmake)) path_cmake = null;
            if (!File.Exists(path_pip)) path_pip = null;
            if (!File.Exists(path_cargo)) path_cargo = null;
            if (!File.Exists(path_qemu_folder)) path_qemu_folder = null;

            path_msbuild = path_msbuild ?? FindProgram("msbuild.exe") ?? findAny(checkMsbuild);
            path_cmake = path_cmake ?? FindProgram("cmake.exe") ?? findAny(checkCmake);
            path_pip = path_pip ?? FindProgram("pip.exe");
            path_cargo = path_cargo ?? FindProgram("cargo.exe");
            if (path_qemu_folder == null)
            {
                string? qemuExe = FindProgram("qemu-img.exe");
                if (qemuExe != null)
                {
                    path_qemu_folder = Path.GetDirectoryName(qemuExe);
                }
            }
            if (path_qemu_folder == null)
            {
                path_qemu_folder = findAny(checkQemu);
            }

            if (forceSave ||
                path_msbuild != old_path_msbuild ||
                path_cmake != old_path_cmake ||
                path_pip != old_path_pip ||
                path_cargo != old_path_cargo ||
                path_qemu_folder != old_path_qemu_folder) {
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
