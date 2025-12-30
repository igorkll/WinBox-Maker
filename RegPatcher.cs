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
            ["HKEY_LOCAL_MACHINE\\SOFTWARE\\", "HKEY_LOCAL_MACHINE\\WINBOX_SOFTWARE\\"],
            ["HKEY_USERS\\DEFAULT_USER\\", "HKEY_LOCAL_MACHINE\\WINBOX_DEFAULT_USER_TEMPLATE\\"],
            ["HKEY_USERS\\.DEFAULT\\", "HKEY_LOCAL_MACHINE\\WINBOX_DOT_DEFAULT_USER\\"],

            ["HKEY_LOCAL_MACHINE\\SOFTWARE]", "HKEY_LOCAL_MACHINE\\WINBOX_SOFTWARE]"],
            ["HKEY_USERS\\DEFAULT_USER]", "HKEY_LOCAL_MACHINE\\WINBOX_DEFAULT_USER_TEMPLATE]"],
            ["HKEY_USERS\\.DEFAULT]", "HKEY_LOCAL_MACHINE\\WINBOX_DOT_DEFAULT_USER]"]
        ];

        static public async Task regPatcher(string regPath, string newRegPath)
        {
            using (var regFile = new FileStream(regPath, FileMode.Open, FileAccess.Read))
            using (var newRegFile = new FileStream(newRegPath, FileMode.OpenOrCreate, FileAccess.Write))
            using (var reader = new StreamReader(regFile, Encoding.UTF8))
            using (var writer = new StreamWriter(newRegFile, new UTF8Encoding(false)))
            {
                writer.WriteLine("Windows Registry Editor Version 5.00");

                bool allowed = false;
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (line.StartsWith("[", StringComparison.OrdinalIgnoreCase)) {
                        allowed = false;
                        foreach (string[] replace in replaceHives)
                        {
                            string startPrefix = "[";
                            string fromReplace = $"{startPrefix}{replace[0]}";
                            if (line.StartsWith(fromReplace, StringComparison.OrdinalIgnoreCase))
                            {
                                line = "\n" + startPrefix + replace[1] + line.Substring(fromReplace.Length);
                                allowed = true;
                            }
                            else
                            {
                                startPrefix = "[-";
                                fromReplace = $"{startPrefix}{replace[0]}";
                                if (line.StartsWith(fromReplace, StringComparison.OrdinalIgnoreCase))
                                {
                                    line = "\n" + startPrefix + replace[1] + line.Substring(fromReplace.Length);
                                    allowed = true;
                                }
                            }
                            if (allowed) break;
                        }
                    }

                    if (allowed)
                    {
                        writer.WriteLine(line);
                    }
                }
            }
        }

        static string[] ExpandHives(string[] allowedHives)
        {
            var result = new List<string>();
            foreach (var hive in allowedHives)
            {
                result.Add(hive + "\\");
                result.Add(hive + "]");
            }
            return result.ToArray();
        }

        static string CleanRegLine(string line)
        {
            if (line.EndsWith("]"))
                line = line[..^1];

            if (line.StartsWith("[-"))
                line = line[2..];
            else if (line.StartsWith("["))
                line = line[1..];

            return line;
        }

        public static (string Name, string Type, string Value)? ParseRegLine(string line)
        {
            line = line.Trim();

            if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";"))
                return null;

            string pattern = "^(@|\"([^\"]+)\")=(.*)$";
            var match = Regex.Match(line, pattern);

            if (!match.Success)
                return null;

            string name = match.Groups[1].Value == "@" ? "(Default)" : match.Groups[2].Value;
            string rawValue = match.Groups[3].Value.Trim();
            string type;
            string value;

            if (rawValue.StartsWith("dword:", StringComparison.OrdinalIgnoreCase))
            {
                type = "REG_DWORD";
                value = rawValue.Substring(6);
            }
            else if (rawValue.StartsWith("hex", StringComparison.OrdinalIgnoreCase))
            {
                type = "REG_BINARY_OR_OTHER";
                value = rawValue;
            }
            else
            {
                type = "REG_SZ";
                if (rawValue.StartsWith("\"") && rawValue.EndsWith("\""))
                    rawValue = rawValue[1..^1];
                value = rawValue;
            }

            return (name, type, value);
        }

        static public async Task<string> regToCommands(string regData, string[] _allowedHives)
        {
            string commands = "";
            string[] allowedHives = ExpandHives(_allowedHives);

            using (var reader = new StringReader(regData))
            {
                bool allowed = false;
                bool removeKey = false;
                string? hive = null;

                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (line.StartsWith("[", StringComparison.OrdinalIgnoreCase))
                    {
                        allowed = false;
                        foreach (string allowedHive in allowedHives)
                        {
                            string startPrefix = "[";
                            string fromReplace = $"{startPrefix}{allowedHive}";
                            if (line.StartsWith(fromReplace, StringComparison.OrdinalIgnoreCase))
                            {
                                allowed = true;
                                removeKey = false;
                            }
                            else
                            {
                                startPrefix = "[-";
                                fromReplace = $"{startPrefix}{allowedHive}";
                                if (line.StartsWith(fromReplace, StringComparison.OrdinalIgnoreCase))
                                {
                                    allowed = true;
                                    removeKey = true;
                                }
                            }
                            if (allowed)
                            {
                                hive = CleanRegLine(line);
                                break;
                            }
                        }
                    }

                    if (allowed)
                    {
                        var parsed = ParseRegLine(line);
                        if (parsed != null)
                        {
                            string name = parsed.Value.Name;
                            string type = parsed.Value.Type;
                            string value = parsed.Value.Value;

                            string cmd;
                            if (removeKey)
                            {
                                if (name == "(Default)")
                                    cmd = $"reg delete \"{hive}\" /ve /f";
                                else
                                    cmd = $"reg delete \"{hive}\" /v \"{name}\" /f";
                            }
                            else
                            {
                                if (name == "(Default)")
                                    cmd = $"reg add \"{hive}\" /ve /t {type} /d \"{value}\" /f";
                                else
                                    cmd = $"reg add \"{hive}\" /v \"{name}\" /t {type} /d \"{value}\" /f";
                            }

                            commands += cmd + "\n";
                        }
                    }
                }
            }

            return commands;
        }
    }
}
