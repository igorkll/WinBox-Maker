using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinBox_Maker
{
    internal class RegChanger
    {
        public static async Task mountRegRaw(string name, string path, string? windowsMountPath = null)
        {
            if (windowsMountPath == null) windowsMountPath = Program.winBoxProject.wimMountPath;
            await Program.ExecuteAsync("reg.exe", $"load HKLM\\{name} \"{Path.Combine(windowsMountPath, path)}\"", null, Program.winBoxProject.debugFolder);
        }

        public static async Task umountRegRaw(string name)
        {
            await Program.ExecuteAsync("reg.exe", $"unload HKLM\\{name}", null, Program.winBoxProject.debugFolder);
        }

        public static async Task mountReg(string hive = "SOFTWARE", string tag = "", string? windowsMountPath=null)
        {
            if (tag.Length > 0) tag = "_" + tag;
            await mountRegRaw($"WINBOX{tag}_{hive}", $"Windows\\System32\\config\\{hive}", windowsMountPath);
        }

        public static async Task umountReg(string hive = "SOFTWARE", string tag = "")
        {
            if (tag.Length > 0) tag = "_" + tag;
            await umountRegRaw($"WINBOX{tag}_{hive}");
        }

        public static async Task RegMod(string baseTree, string path, string key, string value, string tag = "")
        {
            if (tag.Length > 0) tag = "_" + tag;

            path = Program.ReplaceAndPrependBackslash(path);
            string tempRegPath = Path.Combine(Program.winBoxProject.tempDirectoryPath, "temp.reg");
            string regMod = $@"Windows Registry Editor Version 5.00

[HKEY_LOCAL_MACHINE\WINBOX{tag}_{baseTree}{path}]
""{key}""={value}
";
            await File.WriteAllTextAsync(tempRegPath, regMod);
            await Program.ExecuteAsync("reg.exe", $"import \"{tempRegPath}\"", null, Program.winBoxProject.debugFolder);
            File.Delete(tempRegPath);
        }

        public static async Task RegModFromFile(string regPath)
        {
            string newRegPath = Path.Combine(Program.winBoxProject.tempDirectoryPath, "modified_reg.reg");
            await RegPatcher.regPatcher(regPath, newRegPath);
            await Program.winBoxProject.copyToDebugFile($"modified_reg_{Program.CalculateMD5(regPath)}.txt", newRegPath, true);
            await Program.ExecuteAsync("reg.exe", $"import \"{newRegPath}\"", null, Program.winBoxProject.debugFolder);
            File.Delete(newRegPath);
        }

        public static async Task RegModFromRamFile(string fileString)
        {
            string regPath = Path.Combine(Program.winBoxProject.tempDirectoryPath, "base_reg.reg");
            await File.WriteAllTextAsync(regPath, fileString);
            await Program.winBoxProject.copyToDebugFile($"base_reg_{Program.CalculateMD5(fileString)}.txt", regPath, true);
            await RegModFromFile(regPath);
            File.Delete(regPath);
        }
    }
}
