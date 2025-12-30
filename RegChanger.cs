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
        public static async Task mountReg(string hive = "SOFTWARE", string tag = "", string? windowsMountPath=null)
        {
            if (tag.Length > 0) tag = "_" + tag;
            if (windowsMountPath == null) windowsMountPath = Program.winBoxProject.wimMountPath;
            await Program.ExecuteAsync("reg.exe", $"load HKLM\\WINBOX{tag}_{hive} \"{Path.Combine(windowsMountPath, $"Windows\\System32\\config\\{hive}")}\"", null, Program.winBoxProject.debugFolder);
        }

        public static async Task umountReg(string hive = "SOFTWARE", string tag = "")
        {
            if (tag.Length > 0) tag = "_" + tag;
            await Program.ExecuteAsync("reg.exe", $"unload HKLM\\WINBOX{tag}_{hive}", null, Program.winBoxProject.debugFolder);
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
            await Program.winBoxProject.copyToDebugFile($"modified_reg_{Program.CalculateMD5(regPath)}.txt", newRegPath);
            await Program.ExecuteAsync("reg.exe", $"import \"{newRegPath}\"", null, Program.winBoxProject.debugFolder);
            File.Delete(newRegPath);
        }
    }
}
