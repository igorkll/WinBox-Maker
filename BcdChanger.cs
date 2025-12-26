using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties;

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

            // если мы собираем базовую систему - применяем BCD базовой системы
            // так же применяем его если в настройках winPE modifications установлен флаг о том что нужно продублировать настройки BCD из базовой системы
            if (winPeModifications == null || winPeModifications.applyBaseSystemBCD == true)
            {
                // base system bcd modifications
                if (Program.winBoxConfig.manual_setup != true)
                {
                    bool AllowStartRecoveryFromBootloader = Program.winBoxConfig.AllowStartRecoveryFromBootloader == true && (Program.winBoxConfig.manual_setup != true || Program.winBoxConfig.recoverymod_manual_allow == true);

                    regBcdChange("advancedoptions false");
                    regBcdChange("optionsedit false");
                    regBcdChange($"recoveryenabled {(AllowStartRecoveryFromBootloader == true ? "yes" : "no")}"); //запрет автоматического входа в recovery

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

        public static async Task modifyWinBCD(string path, WinPeModifications? winPeModifications = null) // меняет BCD как для ISO winPE, так и для его boot.wim, так и для install.wim
        {
            // winPE iso (BIOS)
            string bcdPath = Path.Combine(path, "boot\\bcd");
            if (File.Exists(bcdPath)) await modifyBCD(bcdPath, winPeModifications);

            // winPE iso (UEFI)
            bcdPath = Path.Combine(path, "EFI\\Microsoft\\Boot\\BCD");
            if (File.Exists(bcdPath)) await modifyBCD(bcdPath, winPeModifications);

            // wim BCD template. используется при разворачивании образа с wim файла
            bcdPath = Path.Combine(path, "Windows\\System32\\Config\\BCD-Template");
            if (File.Exists(bcdPath)) await modifyBCD(bcdPath, winPeModifications);

            // вроде как используется для UEFI в wim или уже установленой системе в каких то случаях. хз, пусть будет
            bcdPath = Path.Combine(path, "Windows\\Boot\\EFI\\BCD");
            if (File.Exists(bcdPath)) await modifyBCD(bcdPath, winPeModifications);
        }
    }
}
