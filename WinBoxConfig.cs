using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WinBox_Maker
{
    public enum ProgramTypeEnum
    {
        ExecutableFile,
        RawCommand,
        WebSite,
        None
    }

    public enum ProgramLaunchModeEnum
    {
        insteadDesktop,
        afterDesktop
    }

    public enum FirstBootActionEnum
    {
        none,
        reboot,
        shutdown,
        hibernate,
        reboot_to_desktop,
        generalize
    }

    public enum ButtonAction
    {
        none,
        sleep,
        hibernation,
        shutdown,
        turn_off_display
    }

    public enum ActionAtEndOfApplication
    {
        invalid,
        none,
        restart_app,
        reboot_computer,
        shutdown_computer,
        execute_command
    }

    public enum StretchMode
    {
        None,
        Fill,
        Uniform,
        UniformToFill
    }

    public enum PowerScheme
    {
        Default,
        Balanced,
        High_Performance,
        Power_Saver
    }

    public enum RecoveryMenuAction
    {
        Delete,
        StayDefault,
        Replace
    }

    public class WinBoxConfig
    {
        //public List<string>? Resources { get; set; }
        public int? winboxMakerVersion { get; set; }
        public string? winboxMakerVersionStr { get; set; }

        public string? BaseWindowsImage { get; set; }
        public string? BaseWindowsVersion { get; set; }
        public string? WinboxName { get; set; }
        public string? WinboxDescription { get; set; }
        public string? OemKey { get; set; }
        public string? ProgramName { get; set; }
        public string? ProgramArgs { get; set; }
        public string? RawCommand { get; set; }
        public string? WebSite { get; set; }
        public string? PostInstall_bat { get; set; }
        public string? PostInstall_reg { get; set; }
        public string? PostInstall_user_bat { get; set; }
        public string? PostInstall_user_reg { get; set; }
        public string? onbuild_reg { get; set; }
        public int? WebSessionTimeout { get; set; }
        public int? ScreenTimeout { get; set; }
        public int? StandbyTimeout { get; set; }
        public int? HibernateTimeout { get; set; }
        public int? DiskTimeout { get; set; }
        public ButtonAction? action_powerButton { get; set; }
        public ButtonAction? action_sleepButton { get; set; }
        public ButtonAction? action_closingLaptop { get; set; }
        public int? ScreenTimeout_dc { get; set; }
        public int? StandbyTimeout_dc { get; set; }
        public int? HibernateTimeout_dc { get; set; }
        public int? DiskTimeout_dc { get; set; }
        public ButtonAction? action_powerButton_dc { get; set; }
        public ButtonAction? action_sleepButton_dc { get; set; }
        public ButtonAction? action_closingLaptop_dc { get; set; }
        public string? Architecture { get; set; }
        public ProgramTypeEnum? ProgramType { get; set; }
        public ProgramLaunchModeEnum? LaunchMode { get; set; }
        public List<string>? TweakList { get; set; }
        public int? TweakListLevel { get; set; }
        public string? CustomBootLogo { get; set; }
        public bool? AddVirtualDisplay { get; set; }
        public int? VirtualDisplayWidth { get; set; }
        public int? VirtualDisplayHeight { get; set; }
        public bool? UseEmbeddedDisplay { get; set; }
        public bool? CustomBootLogo_centering { get; set; }
        public bool? CustomBootLogo_UseLogoBeforeApp { get; set; }
        public bool? CustomBootLogo_UseOnBootres { get; set; }
        public string? pythonVersion { get; set; }
        public bool? downloadEnabled { get; set; }
        public bool? buildEnabled { get; set; }
        public List<DownloadItem>? DownloadItems { get; set; }
        public List<BuildItem>? BuildItems { get; set; }
        public List<TwoStrings>? keyboard_layouts { get; set; }
        public bool? keyboard_layouts_firstAdded { get; set; }
        public bool? forceIot { get; set; }
        public bool? dc_use { get; set; }
        public bool? enable_hibernation { get; set; }
        public bool? enable_hiberboot { get; set; }
        public bool? UseCustomDisplaySettings { get; set; }
        public bool? UseCustomDisplaySettings_scale { get; set; }
        public int? cds_width { get; set; }
        public int? cds_height { get; set; }
        public int? cds_bitDepth { get; set; }
        public int? cds_refreshRate { get; set; }
        public int? cds_scaling { get; set; }
        public int? cds_orientation { get; set; }
        public bool? cds_width_use { get; set; }
        public bool? cds_height_use { get; set; }
        public bool? cds_bitDepth_use { get; set; }
        public bool? cds_refreshRate_use { get; set; }
        public bool? cds_orientation_use { get; set; }
        public FirstBootActionEnum? firstBootAction { get; set; }
        public int? img_size { get; set; }
        public int? img_install_ram { get; set; }
        public int? img_install_cpu { get; set; }
        public bool? img_shutdownAfterInstall { get; set; }
        public bool? img_runningPostinstallOnFirstRealStartup { get; set; }
        public bool? img_generalizeAfterInstall { get; set; }
        public bool? computername_use { get; set; }
        public string? computername { get; set; }
        public ActionAtEndOfApplication? actionAtEndOfApplication { get; set; }
        public string? actionAtEndOfApplication_command { get; set; }
        public bool? appdelay_time { get; set; }
        public int? appdelay_time_value { get; set; }
        public bool? appdelay_internet { get; set; }
        public string? appdelay_internet_checkurl { get; set; }
        public int? appdelay_internet_requestdelay { get; set; }
        public bool? appcrash_time { get; set; }
        public int? appcrash_time_value { get; set; }
        public string? logoBeforeApp { get; set; }
        public StretchMode? logoBeforeApp_stretch { get; set; }
        public bool? wait_before_app_logo { get; set; }
        public string? delete_paths { get; set; }
        public string? delete_dism { get; set; }
        public string? delete_dism_universal { get; set; }
        public string? delete_dism_remove_package { get; set; }
        public string? delete_dism_remove_appx_package { get; set; }
        public bool? manual_setup { get; set; }
        public string? manual_setup_complete { get; set; }
        public string? manual_setup_error { get; set; }
        public string? manual_setup_autounattend { get; set; }
        public string? manual_setup_sysunattend { get; set; }
        public bool? aaf_readme_iso { get; set; }
        public bool? aaf_readme_system { get; set; }
        public bool? aaf_readme_boot { get; set; }
        public bool? aaf_readme_recovery { get; set; }
        public bool? aaf_info_iso { get; set; }
        public bool? aaf_info_system { get; set; }
        public bool? aaf_info_boot { get; set; }
        public bool? aaf_info_recovery { get; set; }
        public bool? oemkey_installer { get; set; }
        public bool? oemkey_dism { get; set; }
        public bool? oemkey_slmgr { get; set; }
        public bool? DynamicDaylightTimeDisabled { get; set; }
        public bool? DisableNtp { get; set; }
        public bool? RealTimeIsUniversal { get; set; }
        public string? services_stop { get; set; }
        public string? services_start { get; set; }
        public string? services_deleteFromList { get; set; }
        public bool? services_stopOnlyList { get; set; }
        public bool? services_startOnlyList { get; set; }
        public string? TimeZoneKeyName { get; set; }
        public bool? customdism_enabled { get; set; }
        public string? customdism_commands { get; set; }
        public string? customdism_features { get; set; }
        public PowerScheme? powerScheme { get; set; }
        public bool? recoverymod_manual_allow { get; set; }
        public bool? installermod_manual_allow { get; set; }
        public RecoveryMenuAction? recoveryMenuAction { get; set; }
        public string? ReplaceRecovery { get; set; }
        public bool? AllowStartRecoveryFromBootloader { get; set; }
        public bool? bsod_autoreboot { get; set; }
        public bool? bsod_disabledisplay { get; set; }
        public bool? ChangeTimezone { get; set; }
        public bool? EnableRecovery { get; set; }
        public string? bootresRepacking_logoPath { get; set; }


        public bool? prebuildEnabled { get; set; }
        public string? prebuildEvent { get; set; }
        public bool? prebuild_breakbefore { get; set; }
        public bool? prebuild_breakafter { get; set; }

        public bool? postbuildEnabled { get; set; }
        public string? postbuildEvent { get; set; }
        public bool? postbuild_breakbefore { get; set; }
        public bool? postbuild_breakafter { get; set; }
        
        public bool? winmountedEnabled { get; set; }
        public string? winmountedEvent { get; set; }
        public bool? winmounted_breakbefore { get; set; }
        public bool? winmounted_breakafter { get; set; }

        public bool? winmountedEarlyEnabled { get; set; }
        public string? winmountedEarlyEvent { get; set; }
        public bool? winmountedEarly_breakbefore { get; set; }
        public bool? winmountedEarly_breakafter { get; set; }

        public bool? recoveryMountedEarlyEnabled { get; set; }
        public string? recoveryMountedEarlyEvent { get; set; }
        public bool? recoveryMountedEarly_breakbefore { get; set; }
        public bool? recoveryMountedEarly_breakafter { get; set; }

        public bool? installerMountedEarlyEnabled { get; set; }
        public string? installerMountedEarlyEvent { get; set; }
        public bool? installerMountedEarly_breakbefore { get; set; }
        public bool? installerMountedEarly_breakafter { get; set; }


        public string? schtasks_stopOrDelete { get; set; }
        public string? schtasks_stopOrDelete_deleteFromList { get; set; }
        public bool? schtasks_stopOrDeleteOnlyFromList { get; set; }

        public WinPeModifications? recovery_winPE_mod { get; set; }
        public WinPeModifications? installer_winPE_mod { get; set; }

        public bool? install_bypass { get; set; }
        public bool? keyboard_filter_enabled { get; set; }
        public List<string>? keyboard_filter_blockList { get; set; }
        public int? keyboard_filter_BreakoutKeyScanCode { get; set; }
        public bool? keyboard_filter_DisableKeyboardFilterForAdministrators { get; set; }
        public bool? keyboard_filter_ForceOffAccessibility { get; set; }



        static string[] renameTweaks_from = [
            "removing UWP apps",
            "Do not disable hotkeys by changing the registry"
        ];

        static string[] renameTweaks_to = [
            "removing Windows/System apps (breaks the default shell)",
            "Do not disable hotkeys by keyboard filter"
        ];


        public WinBoxConfig() {
            InitDefaults();
        }

        static int actualTweakListLevel = 2;

        void InitDefaults()
        {
            //if (Resources == null) Resources = new List<string>();

            if (winboxMakerVersion == null) winboxMakerVersion = Program.version_num;
            if (winboxMakerVersionStr == null) winboxMakerVersionStr = Program.version_str;

            if (TweakList == null) TweakList = ["Integrate vc redist", "Disable all boot UI", "Hide bootmgr errors", "Disable boot circle", "Disable boot messages"];
            if (TweakListLevel == null) TweakListLevel = 0;
            if (TweakListLevel < 1)
            {
                TweakList.Add("Disable system integrity checks");
                TweakList.Add("Disable HyperV / VSM / ELAM");
            }
            if (TweakListLevel < 2)
            {
                TweakList.Add("make a quiet SPP");
            }

            if (keyboard_filter_blockList == null)
            {
                keyboard_filter_blockList = [
                    "Alt+F4",
                    "Alt+Space",
                    "Alt+Tab",
                    "Alt+Win",
                    "Application",
                    "BrowserBack",
                    "BrowserFavorites",
                    "BrowserForward",
                    "BrowserHome",
                    "BrowserRefresh",
                    "BrowserSearch",
                    "BrowserStop",
                    "Ctrl+Alt+Del",
                    "Ctrl+Esc",
                    "Ctrl+F4",
                    "Ctrl+Tab",
                    "Ctrl+Win",
                    "Ctrl+Win+F",
                    "F21",
                    "LaunchApp1",
                    "LaunchApp2",
                    "LaunchMail",
                    "LaunchMediaSelect",
                    "LShift+LAlt+NumLock",
                    "LShift+LAlt+PrintScrn",
                    "Shift+Ctrl+Esc",
                    "Shift+Win",
                    "Windows"
                ];
            }
            if (keyboard_filter_BreakoutKeyScanCode == null) keyboard_filter_BreakoutKeyScanCode = 0;
            if (keyboard_filter_DisableKeyboardFilterForAdministrators == null) keyboard_filter_DisableKeyboardFilterForAdministrators = false;
            if (keyboard_filter_ForceOffAccessibility == null) keyboard_filter_ForceOffAccessibility = true;

            if (WinboxName == null) WinboxName = "Winbox Name";
            if (WinboxDescription == null) WinboxDescription = "Winbox Description";
            if (OemKey == null) OemKey = "";
            if (ProgramArgs == null) ProgramArgs = "";
            if (RawCommand == null) RawCommand = "";
            if (WebSite == null) WebSite = "";
            if (WebSessionTimeout == null) WebSessionTimeout = 0;
            
            if (ScreenTimeout == null) ScreenTimeout = 0;
            if (StandbyTimeout == null) StandbyTimeout = 0;
            if (HibernateTimeout == null) HibernateTimeout = 0;
            if (DiskTimeout == null) DiskTimeout = 0;

            if (ScreenTimeout_dc == null) ScreenTimeout_dc = 0;
            if (StandbyTimeout_dc == null) StandbyTimeout_dc = 0;
            if (HibernateTimeout_dc == null) HibernateTimeout_dc = 0;
            if (DiskTimeout_dc == null) DiskTimeout_dc = 0;

            if (Architecture == null) Architecture = "x64";
            if (ProgramType == null) ProgramType = ProgramTypeEnum.ExecutableFile;
            if (LaunchMode == null) LaunchMode = ProgramLaunchModeEnum.insteadDesktop;
            if (AddVirtualDisplay == null) AddVirtualDisplay = false;
            if (VirtualDisplayWidth == null) VirtualDisplayWidth = 960;
            if (VirtualDisplayHeight == null) VirtualDisplayHeight = 640;
            if (UseEmbeddedDisplay == null) UseEmbeddedDisplay = false;
            if (CustomBootLogo_centering == null) CustomBootLogo_centering = true;
            if (CustomBootLogo_UseLogoBeforeApp == null) CustomBootLogo_UseLogoBeforeApp = false;
            if (CustomBootLogo_UseOnBootres == null) CustomBootLogo_UseOnBootres = false;
            if (pythonVersion == null) pythonVersion = null;
            if (downloadEnabled == null) downloadEnabled = false;
            if (buildEnabled == null) buildEnabled = false;
            if (DownloadItems == null) DownloadItems = new List<DownloadItem>();
            if (BuildItems == null) BuildItems = new List<BuildItem>();
            if (keyboard_layouts == null) keyboard_layouts = new List<TwoStrings>();
            if (keyboard_layouts_firstAdded == null) keyboard_layouts_firstAdded = false;
            if (forceIot == null) forceIot = false;
            if (dc_use == null) dc_use = false;
            if (enable_hibernation == null) enable_hibernation = false;
            if (enable_hiberboot == null) enable_hiberboot = false;
            if (UseCustomDisplaySettings == null) UseCustomDisplaySettings = false;
            if (UseCustomDisplaySettings_scale == null) UseCustomDisplaySettings_scale = false;
            if (cds_width == null) cds_width = 800;
            if (cds_height == null) cds_height = 600;
            if (cds_bitDepth == null) cds_bitDepth = 32;
            if (cds_refreshRate == null) cds_refreshRate = 60;
            if (cds_scaling == null) cds_scaling = 100;
            if (cds_orientation == null) cds_orientation = 0;
            if (firstBootAction == null) firstBootAction = FirstBootActionEnum.none;

            if (action_powerButton == null) action_powerButton = ButtonAction.shutdown;
            if (action_sleepButton == null) action_sleepButton = ButtonAction.sleep;
            if (action_closingLaptop == null) action_closingLaptop = ButtonAction.turn_off_display;

            if (action_powerButton_dc == null) action_powerButton_dc = ButtonAction.shutdown;
            if (action_sleepButton_dc == null) action_sleepButton_dc = ButtonAction.sleep;
            if (action_closingLaptop_dc == null) action_closingLaptop_dc = ButtonAction.turn_off_display;

            if (cds_width_use == null) cds_width_use = false;
            if (cds_height_use == null) cds_height_use = false;
            if (cds_bitDepth_use == null) cds_bitDepth_use = false;
            if (cds_refreshRate_use == null) cds_refreshRate_use = false;
            if (cds_orientation_use == null) cds_orientation_use = false;

            if (img_size == null) img_size = 1024 * 20;
            if (img_install_ram == null) img_install_ram = 1024 * 2;
            if (img_install_cpu == null) img_install_cpu = 2;
            if (img_shutdownAfterInstall == null) img_shutdownAfterInstall = true;
            if (img_runningPostinstallOnFirstRealStartup == null) img_runningPostinstallOnFirstRealStartup = true;
            if (img_generalizeAfterInstall == null) img_generalizeAfterInstall = true;

            if (computername_use == null) computername_use = false;
            if (computername == null) computername = "winbox-maker";

            if (actionAtEndOfApplication == null) actionAtEndOfApplication = ActionAtEndOfApplication.invalid;
            if (actionAtEndOfApplication_command == null) actionAtEndOfApplication_command = "";

            if (appdelay_time == null) appdelay_time = false;
            if (appdelay_time_value == null) appdelay_time_value = 0;

            if (appcrash_time == null) appcrash_time = false;
            if (appcrash_time_value == null) appcrash_time_value = 0;

            if (appdelay_internet == null) appdelay_internet = false;
            if (appdelay_internet_checkurl == null) appdelay_internet_checkurl = "google.com";
            if (appdelay_internet_requestdelay == null) appdelay_internet_requestdelay = 1;

            if (logoBeforeApp_stretch == null) logoBeforeApp_stretch = StretchMode.None;

            if (wait_before_app_logo == null) wait_before_app_logo = true;

            if (delete_paths == null) delete_paths = "";
            if (delete_dism == null) delete_dism = "";
            if (delete_dism_universal == null) delete_dism_universal = "";
            if (delete_dism_remove_package == null) delete_dism_remove_package = "";
            if (delete_dism_remove_appx_package == null) delete_dism_remove_appx_package = "";

            if (manual_setup == null) manual_setup = false;

            if (aaf_readme_iso == null) aaf_readme_iso = true;
            if (aaf_readme_system == null) aaf_readme_system = true;
            if (aaf_readme_boot == null) aaf_readme_boot = true;
            if (aaf_readme_recovery == null) aaf_readme_recovery = true;
            if (aaf_info_iso == null) aaf_info_iso = true;
            if (aaf_info_system == null) aaf_info_system = true;
            if (aaf_info_boot == null) aaf_info_boot = true;
            if (aaf_info_recovery == null) aaf_info_recovery = true;

            if (oemkey_installer == null) oemkey_installer = true;
            if (oemkey_dism == null) oemkey_dism = true;
            if (oemkey_slmgr == null) oemkey_slmgr = false;

            if (DynamicDaylightTimeDisabled == null) DynamicDaylightTimeDisabled = true;
            if (DisableNtp == null) DisableNtp = true;
            if (RealTimeIsUniversal == null) RealTimeIsUniversal = false;

            if (services_stop == null) services_stop = "";
            if (services_start == null) services_start = "";
            if (services_deleteFromList == null) services_deleteFromList = "";
            if (services_stopOnlyList == null) services_stopOnlyList = false;
            if (services_startOnlyList == null) services_startOnlyList = false;

            if (TimeZoneKeyName == null) TimeZoneKeyName = "UTC";
            if (powerScheme == null) powerScheme = PowerScheme.Balanced;

            if (recoverymod_manual_allow == null) recoverymod_manual_allow = false;
            if (installermod_manual_allow == null) installermod_manual_allow = false;
            if (recoveryMenuAction == null) recoveryMenuAction = RecoveryMenuAction.Delete;
            if (ReplaceRecovery == null) ReplaceRecovery = "";
            if (AllowStartRecoveryFromBootloader == null) AllowStartRecoveryFromBootloader = false;

            for (int i = 0; i < renameTweaks_from.Length; i++)
            {
                Program.ReplaceAll(TweakList, renameTweaks_from[i], renameTweaks_to[i]);
            }

            if (bsod_autoreboot == null) bsod_autoreboot = true;
            if (bsod_disabledisplay == null) bsod_disabledisplay = true;
            if (ChangeTimezone == null) ChangeTimezone = true;
            if (EnableRecovery == null) EnableRecovery = false;

            if (prebuildEnabled == null) prebuildEnabled = false;
            if (prebuildEvent == null) prebuildEvent = "";
            if (prebuild_breakbefore == null) prebuild_breakbefore = false;
            if (prebuild_breakafter == null) prebuild_breakafter = false;

            if (postbuildEnabled == null) postbuildEnabled = false;
            if (postbuildEvent == null) postbuildEvent = "";
            if (postbuild_breakbefore == null) postbuild_breakbefore = false;
            if (postbuild_breakafter == null) postbuild_breakafter = false;

            if (winmountedEnabled == null) winmountedEnabled = false;
            if (winmountedEvent == null) winmountedEvent = "";
            if (winmounted_breakbefore == null) winmounted_breakbefore = false;
            if (winmounted_breakafter == null) winmounted_breakafter = false;

            if (winmountedEarlyEnabled == null) winmountedEarlyEnabled = false;
            if (winmountedEarlyEvent == null) winmountedEarlyEvent = "";
            if (winmountedEarly_breakbefore == null) winmountedEarly_breakbefore = false;
            if (winmountedEarly_breakafter == null) winmountedEarly_breakafter = false;

            if (recoveryMountedEarlyEnabled == null) recoveryMountedEarlyEnabled = false;
            if (recoveryMountedEarlyEvent == null) recoveryMountedEarlyEvent = "";
            if (recoveryMountedEarly_breakbefore == null) recoveryMountedEarly_breakbefore = false;
            if (recoveryMountedEarly_breakafter == null) recoveryMountedEarly_breakafter = false;

            if (installerMountedEarlyEnabled == null) installerMountedEarlyEnabled = false;
            if (installerMountedEarlyEvent == null) installerMountedEarlyEvent = "";
            if (installerMountedEarly_breakbefore == null) installerMountedEarly_breakbefore = false;
            if (installerMountedEarly_breakafter == null) installerMountedEarly_breakafter = false;

            if (schtasks_stopOrDelete == null) schtasks_stopOrDelete = "";
            if (schtasks_stopOrDelete_deleteFromList == null) schtasks_stopOrDelete_deleteFromList = "";
            if (schtasks_stopOrDeleteOnlyFromList == null) schtasks_stopOrDeleteOnlyFromList = false;

            if (recovery_winPE_mod == null) recovery_winPE_mod = new WinPeModifications();
            if (installer_winPE_mod == null) installer_winPE_mod = new WinPeModifications();

            installer_winPE_mod.initDefaults(0);
            recovery_winPE_mod.initDefaults(1);

            foreach (BuildItem buildItem in BuildItems)
            {
                buildItem.initDefaults();
            }

            if (install_bypass == null) install_bypass = true;
            if (keyboard_filter_enabled == null) keyboard_filter_enabled = true;
        }

        public void Save(string wnbFilePath)
        {
            winboxMakerVersion = Program.version_num;
            winboxMakerVersionStr = Program.version_str;

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(wnbFilePath, json);
        }

        public static WinBoxConfig? Load(string wnbFilePath)
        {
            try
            {
                string json = File.ReadAllText(wnbFilePath);
                WinBoxConfig? winBoxConfig = JsonSerializer.Deserialize<WinBoxConfig>(json);
                winBoxConfig?.InitDefaults();
                if (actualTweakListLevel > winBoxConfig.TweakListLevel) winBoxConfig.TweakListLevel = actualTweakListLevel;
                return winBoxConfig;
            } catch (Exception ex) {}
            return null;
        }

        public bool isBuildEventsUsed()
        {
            return prebuildEnabled == true ||
                postbuildEnabled == true ||
                winmountedEnabled == true ||
                winmountedEarlyEnabled == true ||
                recoveryMountedEarlyEnabled == true ||
                downloadEnabled == true ||
                buildEnabled == true ||
                customdism_enabled == true;
        }

        public bool isValidOemKey()
        {
            if (OemKey == null) return false;
            return OemKey.Length > 0 && !OemKey.Contains("\"");
        }
    }
}
