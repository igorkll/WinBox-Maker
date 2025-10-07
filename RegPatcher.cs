using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WinBox_Maker
{
    static public class RegPatcher
    {
        static string[][] replaceHives = [
            ["HKEY_LOCAL_MACHINE\\SOFTWARE\\", "HKEY_LOCAL_MACHINE\\WINBOX_SOFTWARE\\"]
        ];

        static public async Task regPatcher(string regPath, string newRegPath)
        {
            using (var regFile = new FileStream(regPath, FileMode.Open, FileAccess.Read))
            using (var newRegFile = new FileStream(newRegPath, FileMode.OpenOrCreate, FileAccess.Write))
            using (var reader = new StreamReader(regFile, Encoding.UTF8))
            using (var writer = new StreamWriter(newRegFile, new UTF8Encoding(false)))
            {
                writer.WriteLine(reader.ReadLine()); //skip first line

                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    bool allowed = false;
                    if (line.StartsWith("[", StringComparison.OrdinalIgnoreCase)) {
                        foreach (string[] replace in replaceHives)
                        {
                            string startPrefix = "[";
                            string fromReplace = $"{startPrefix}{replace[0]}";
                            if (line.StartsWith(fromReplace, StringComparison.OrdinalIgnoreCase))
                            {
                                line = startPrefix + replace[1] + line.Substring(fromReplace.Length);
                                allowed = true;
                            }
                            else
                            {
                                startPrefix = "[-";
                                fromReplace = $"{startPrefix}{replace[0]}";
                                if (line.StartsWith(fromReplace, StringComparison.OrdinalIgnoreCase))
                                {
                                    line = startPrefix + replace[1] + line.Substring(fromReplace.Length);
                                    allowed = true;
                                }
                            }
                        }
                    }

                    if (allowed)
                    {
                        writer.WriteLine(line);
                    }
                }
            }
        }
    }
}
