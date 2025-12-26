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

    public enum WinboxRecoveryLogoType
    {
        DefaultLogo,
        CustomLogo,
        NoLogo
    }

    public class WinPeModifications
    {
        public bool? enabled { get; set; }

        public bool? applyBaseSystemBCD { get; set; }
        public bool? remove_cmd_exe { get; set; }
        public bool? app_override { get; set; }
        public bool? app_lowlevel { get; set; }
        public AppOverrideType? app_override_type { get; set; }
        public string? app_custom_cmdline { get; set; }
        public WinboxRecoveryLogoType? winboxRecoveryLogoType { get; set; }
        public string? customRecoveryLogoPath { get; set; }

        public string? recovery_title { get; set; }
        public string? recovery_dataPaths { get; set; }
        public bool? recovery_allowFactoryReset { get; set; }
        public bool? recovery_allowFlashWim { get; set; }
        public string? recovery_wimName { get; set; }
        public bool? recovery_allowFlashImg { get; set; }
        public string? recovery_imgName { get; set; }
        public bool? recovery_allowFlashFfu { get; set; }
        public string? recovery_ffuName { get; set; }
        public bool? recovery_allowFlashWithoutFactoryReset { get; set; }
        public bool? recovery_allowFlashWithFactoryReset { get; set; }
        public bool? recovery_textOnInfoPage_en { get; set; }
        public string? recovery_textOnInfoPage { get; set; }

        // initFor
        // 0 - installer
        // 1 - recovery
        // 2 - other
        public void initDefaults(int initFor = 0)
        {
            if (enabled == null) enabled = true;
            if (applyBaseSystemBCD == null) applyBaseSystemBCD = true;
            if (remove_cmd_exe == null) remove_cmd_exe = initFor == 1;
            if (app_override == null) app_override = false;
            if (app_lowlevel == null) app_lowlevel = initFor == 1;
            if (app_override_type == null) app_override_type = AppOverrideType.WinboxMakerRecovery;
            if (app_custom_cmdline == null) app_custom_cmdline = "my_app_example.exe --argument";
            if (winboxRecoveryLogoType == null) winboxRecoveryLogoType = WinboxRecoveryLogoType.DefaultLogo;
            if (customRecoveryLogoPath == null) customRecoveryLogoPath = "";

            if (recovery_title == null) recovery_title = "Winbox maker recovery";
            if (recovery_dataPaths == null) recovery_dataPaths = "Users\\winbox\\AppData\\Roaming\\MY_APP_DATA_EXAMPLE\nOTHER_DATA_FOLDER_IN_C_DRIVE\nUsers\\winbox\\desktop\\FILE_ON_DESKTOP_EXAMPLE";
            if (recovery_allowFactoryReset == null) recovery_allowFactoryReset = true;
            if (recovery_allowFlashWim == null) recovery_allowFlashWim = true;
            if (recovery_wimName == null) recovery_wimName = "firmware.wim";
            if (recovery_allowFlashImg == null) recovery_allowFlashImg = true;
            if (recovery_imgName == null) recovery_imgName = "firmware.img";
            if (recovery_allowFlashFfu == null) recovery_allowFlashFfu = true;
            if (recovery_ffuName == null) recovery_ffuName = "firmware.ffu";
            if (recovery_allowFlashWithoutFactoryReset == null) recovery_allowFlashWithoutFactoryReset = true;
            if (recovery_allowFlashWithFactoryReset == null) recovery_allowFlashWithFactoryReset = true;
            if (recovery_textOnInfoPage_en == null) recovery_textOnInfoPage_en = false;
            if (recovery_textOnInfoPage == null) recovery_textOnInfoPage = "you can download device firmware on\nhttps://example.com";
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
            // copy recovery exe
            string recoveryDirectory = Path.Combine(mountedPath, "WinboxMakerRecovery");
            Program.CreateDirectory(recoveryDirectory);
            await Program.CopyFileAsync(Program.getBlobPath(Program.winBoxConfig, recoveryFileName), Path.Combine(recoveryDirectory, recoveryFileName));

            // copy recovery logo
            string? logoPath = null;
            switch (winboxRecoveryLogoType)
            {
                case WinboxRecoveryLogoType.DefaultLogo:
                    logoPath = Program.getBlobPath(Program.winBoxConfig, "WinboxMakerRecoveryLogo.bmp");
                    break;

                case WinboxRecoveryLogoType.CustomLogo:
                    if (customRecoveryLogoPath.Length > 0 && !customRecoveryLogoPath.Contains(".."))
                        logoPath = Path.Combine(Program.winBoxProject.resourcesDirectoryPath, customRecoveryLogoPath);
                    break;
            }

            if (logoPath != null)
                ImageConverter.ConvertToBmp_54_24(logoPath, Path.Combine(recoveryDirectory, "logo.bmp"));

            // write recovery settings json

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

            string path_winpeshl_ini = Path.Combine(mountedPath, "Windows\\System32\\winpeshl.ini");
            string path_winpeshl_exe = Path.Combine(mountedPath, "Windows\\System32\\winpeshl.exe");

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

                await Program.WriteFileAsync(path_winpeshl_ini, @$"[LaunchApps]
%SYSTEMDRIVE%{Program.ReplaceAndPrependBackslash(cmdline)}");

                if (app_lowlevel == true && cmdline.Length > 0 && !cmdline.Contains(".."))
                {
                    await Program.CopyFileAsync(Path.Combine(mountedPath, cmdline), path_winpeshl_exe);
                }
            }

            if (remove_cmd_exe == true)
            {
                File.Delete(Path.Combine(mountedPath, "Windows\\System32\\cmd.exe"));
            }
        }
    }
}
