using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WinBox_Maker
{
    public enum AppOverrideType
    {
        WinboxMakerRecovery,
        Custom
    }

    public class WinPeModifications
    {
        public bool? enabled { get; set; }

        public bool? applyBaseSystemBCD { get; set; }
        public bool? app_override { get; set; }
        public AppOverrideType? app_override_type { get; set; }
        public string? app_custom_cmdline { get; set; }

        public void initDefaults()
        {
            if (enabled == null) enabled = true;
            if (applyBaseSystemBCD == null) applyBaseSystemBCD = true;
            if (app_override == null) app_override = false;
            if (app_override_type == null) app_override_type = AppOverrideType.WinboxMakerRecovery;
            if (app_custom_cmdline == null) app_custom_cmdline = "my_app_example.exe --argument";
        }

        // ------------------------------

        public void openGui(string titleSuffix)
        {
            WinPeModificationsUI newForm = new WinPeModificationsUI(this, titleSuffix);
            newForm.ShowDialog();
        }

        public async Task modMountedIso(string mountedPath)
        {
            if (enabled != true) return;
            await BcdChanger.modifyWinBCD(mountedPath, this);
        }

        // -------------------------------------

        string recoveryFileName = "WinboxMakerRecovery.exe";

        async Task addWinboxMakerRecoveryFils(string mountedPath)
        {
            string recoveryDirectory = Path.Combine(mountedPath, "WinboxMakerRecovery");
            Program.CreateDirectory(recoveryDirectory);

            await Program.CopyFileAsync(Program.getBlobPath(Program.winBoxConfig, recoveryFileName), Path.Combine(recoveryDirectory, recoveryFileName));
        }

        public async Task modMountedWim(string mountedPath)
        {
            if (enabled != true) return;
            await BcdChanger.modifyWinBCD(mountedPath, this);

            /*
            // FUCKING WINDOWS!
            bool needMountReg = app_override == true;

            if (needMountReg) await RegChanger.mountReg("SYSTEM", "WINPE", mountedPath);

            if (app_override == true)
            {
                string cmdline = "";
                switch (app_override_type)
                {
                    case AppOverrideType.WinboxMakerRecovery:
                        await addWinboxMakerRecoveryFils(mountedPath);
                        cmdline = "X:\\WinboxMakerRecovery\\WinboxMakerRecovery.exe";
                        break;

                    case AppOverrideType.Custom:
                        cmdline = app_custom_cmdline;
                        break;
                }

                await RegChanger.RegMod("SYSTEM", "Setup", "SetupType", "dword:00000002", "WINPE");
                await RegChanger.RegMod("SYSTEM", "Setup", "CmdLine", Program.EscapeForRegFile(cmdline), "WINPE");
            }

            if (needMountReg) await RegChanger.umountReg("SYSTEM", "WINPE");
            */

            if (app_override == true)
            {
                string cmdline = "";
                switch (app_override_type)
                {
                    case AppOverrideType.WinboxMakerRecovery:
                        await addWinboxMakerRecoveryFils(mountedPath);
                        cmdline = "WinboxMakerRecovery\\WinboxMakerRecovery.exe";
                        break;

                    case AppOverrideType.Custom:
                        cmdline = app_custom_cmdline;
                        break;
                }

                await File.WriteAllTextAsync(Path.Combine(mountedPath, "Windows\\System32\\winpeshl.ini"), @$"[LaunchApps]
%SYSTEMDRIVE%{Program.ReplaceAndPrependBackslash(cmdline)}");
            }
        }
    }
}
