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
        public static async Task mountReg(string hive = "SOFTWARE", string tag = "")
        {
            if (tag.Length > 0) tag = "_" + tag;
            await Program.ExecuteAsync("reg.exe", $"load HKLM\\WINBOX{tag}_{hive} \"{Path.Combine(Program.winBoxProject.wimMountPath, $"Windows\\System32\\config\\{hive}")}\"", null, Program.winBoxProject.debugFolder);
        }

        public static async Task umountReg(string hive = "SOFTWARE", string tag = "")
        {
            if (tag.Length > 0) tag = "_" + tag;
            await Program.ExecuteAsync("reg.exe", $"unload HKLM\\WINBOX_{hive}", null, Program.winBoxProject.debugFolder);
        }
    }
}
