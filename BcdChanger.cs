using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinBox_Maker
{
    public class BcdChanger
    {
        public static string getBcdeditSetup(string? store = null, WinPeModifications? winPeModifications = null)
        {
            string bcdeditSetup = "";

            string? storeCmd = "";
            if (store != null)
            {
                storeCmd = @$" /store ""{store}""";
                bcdeditSetup += $"// BCD Path: {store} \r\n";
            }

            void regBcdChange(string change, string? partition = null)
            {
                if (partition != null)
                {
                    bcdeditSetup += $"bcdedit{storeCmd} /set {{{partition}}} " + change + "\r\n";
                }
                else
                {
                    bcdeditSetup += $"bcdedit{storeCmd} /set {{globalsettings}} " + change + "\r\n";
                    bcdeditSetup += $"bcdedit{storeCmd} /set {{bootmgr}} " + change + "\r\n";
                    bcdeditSetup += $"bcdedit{storeCmd} /set {{current}} " + change + "\r\n";
                    bcdeditSetup += $"bcdedit{storeCmd} /set {{default}} " + change + "\r\n\r\n";
                }
            }

            if (Program.winBoxConfig.manual_setup != true && (winPeModifications == null))
            {
                regBcdChange("advancedoptions false");
                regBcdChange("optionsedit false");
                regBcdChange("recoveryenabled no"); //запрет автоматического входа в recovery

                regBcdChange("displaybootmenu no"); // нечего не показываем
                regBcdChange("timeout 0");
                regBcdChange("bootstatuspolicy ignoreallfailures");

                regBcdChange("hypervisorlaunchtype off"); // для embedded это мусор
                regBcdChange("vsmlaunchtype off");
                regBcdChange("disableelamdrivers yes");

                regBcdChange("loadoptions DISABLE_INTEGRITY_CHECKS"); //chatGPT сказал что это даже на embedded п@здец полный
                regBcdChange("NOINTEGRITYCHECKS ON");
                regBcdChange("TESTSIGNING ON");

                if (Program.isTweakEnabled(Program.winBoxConfig, "Disable boot circle"))
                {
                    regBcdChange("custom:16000069 true");
                }

                if (Program.isTweakEnabled(Program.winBoxConfig, "Disable boot logo"))
                {
                    regBcdChange("custom:16000067 true");
                }

                if (Program.isTweakEnabled(Program.winBoxConfig, "Disable boot messages"))
                {
                    regBcdChange("custom:16000068 true");
                }

                if (Program.isTweakEnabled(Program.winBoxConfig, "Disable all boot UI"))
                {
                    regBcdChange("bootuxdisabled on");
                }

                if (Program.isTweakEnabled(Program.winBoxConfig, "Hide bootmgr errors"))
                {
                    regBcdChange("noerrordisplay on");
                }
            }

            return bcdeditSetup;
        }

        public static async Task modifyBCD(string bcdPath, WinPeModifications? winPeModifications = null)
        {
            string bcdscriptName = $"modifyBCD_{Program.CalculateMD5(bcdPath)}";
            string bcdscriptPath = Path.Combine(Program.winBoxProject.tempDirectoryPath, $"{bcdscriptName}.bat");
            string bcdeditCommand = getBcdeditSetup(bcdPath, winPeModifications);

            await File.WriteAllTextAsync(bcdscriptPath, bcdeditCommand);
            await Program.winBoxProject.writeDebugFile(bcdscriptName, bcdeditCommand);
            await Program.ExecuteAsync("cmd.exe", $"/c \"{bcdscriptPath}\"", null, Program.winBoxProject.getDebugFilePath($"{bcdscriptName}_output"));
            File.Delete(bcdscriptPath);
        }

        public static async Task modifyWinBCD(string path, WinPeModifications? winPeModifications = null)
        {
            string bcdPath = Path.Combine(path, "boot\\bcd");
            if (File.Exists(bcdPath))
            {
                await modifyBCD(bcdPath, winPeModifications);
            }

            bcdPath = Path.Combine(path, "EFI\\Microsoft\\Boot\\BCD");
            if (File.Exists(bcdPath))
            {
                await modifyBCD(bcdPath, winPeModifications);
            }

            bcdPath = Path.Combine(path, "Windows\\Boot\\EFI\\BCD");
            if (File.Exists(bcdPath))
            {
                await modifyBCD(bcdPath, winPeModifications);
            }

            bcdPath = Path.Combine(path, "Windows\\System32\\Config\\BCD-Template");
            if (File.Exists(bcdPath))
            {
                await modifyBCD(bcdPath, winPeModifications);
            }
        }
    }
}
