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
        public bool? UseOemKey { get; set; }
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
        public string? CustomBootLogo { get; set; }
        public bool? AddVirtualDisplay { get; set; }
        public int? VirtualDisplayWidth { get; set; }
        public int? VirtualDisplayHeight { get; set; }
        public bool? UseEmbeddedDisplay { get; set; }
        public bool? CustomBootLogo_centering { get; set; }
        public bool? CustomBootLogo_UseLogoBeforeApp { get; set; }
        public bool? prebuildEnabled { get; set; }
        public string? prebuildEvent { get; set; }
        public bool? postbuildEnabled { get; set; }
        public string? postbuildEvent { get; set; }
        public bool? winmountedEnabled { get; set; }
        public string? winmountedEvent { get; set; }
        public string? pythonVersion { get; set; }
        public bool? downloadEnabled { get; set; }
        public bool? buildEnabled { get; set; }
        public List<DownloadItem>? DownloadItems { get; set; }
        public List<BuildItem>? BuildItems { get; set; }
        public bool? forceIot { get; set; }
        public bool? dc_use { get; set; }
        public bool? enable_hibernation { get; set; }
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
        public bool? aaf_info_iso { get; set; }
        public bool? aaf_info_system { get; set; }
        public bool? aaf_info_boot { get; set; }
        public bool? oemkey_installer { get; set; }
        public bool? oemkey_dism { get; set; }
        public bool? oemkey_slmgr { get; set; }
        public bool? DynamicDaylightTimeDisabled { get; set; }
        public bool? DisableNtp { get; set; }
        public bool? RealTimeIsUniversal { get; set; }
        public string? services_stop { get; set; }
        public string? services_start { get; set; }
        public bool? services_stopOnlyList { get; set; }
        public bool? services_startOnlyList { get; set; }


        public WinBoxConfig() {
            InitDefaults();
        }

        void InitDefaults()
        {
            //if (Resources == null) Resources = new List<string>();
            if (winboxMakerVersion == null) winboxMakerVersion = Program.version_num;
            if (winboxMakerVersionStr == null) winboxMakerVersionStr = Program.version_str;

            if (WinboxName == null) WinboxName = "Winbox Name";
            if (WinboxDescription == null) WinboxDescription = "Winbox Description";
            if (OemKey == null) OemKey = "";
            if (UseOemKey == null) UseOemKey = false;
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
            if (TweakList == null) TweakList = ["Integrate vc redist", "Disable all boot UI", "Hide bootmgr errors"];
            if (ProgramType == null) ProgramType = ProgramTypeEnum.ExecutableFile;
            if (LaunchMode == null) LaunchMode = ProgramLaunchModeEnum.insteadDesktop;
            if (AddVirtualDisplay == null) AddVirtualDisplay = false;
            if (VirtualDisplayWidth == null) VirtualDisplayWidth = 960;
            if (VirtualDisplayHeight == null) VirtualDisplayHeight = 640;
            if (UseEmbeddedDisplay == null) UseEmbeddedDisplay = false;
            if (CustomBootLogo_centering == null) CustomBootLogo_centering = false;
            if (CustomBootLogo_UseLogoBeforeApp == null) CustomBootLogo_UseLogoBeforeApp = false;
            if (prebuildEnabled == null) prebuildEnabled = false;
            if (prebuildEvent == null) prebuildEvent = "";
            if (postbuildEnabled == null) postbuildEnabled = false;
            if (postbuildEvent == null) postbuildEvent = "";
            if (winmountedEnabled == null) winmountedEnabled = false;
            if (winmountedEvent == null) winmountedEvent = "";
            if (pythonVersion == null) pythonVersion = null;
            if (downloadEnabled == null) downloadEnabled = false;
            if (buildEnabled == null) buildEnabled = false;
            if (DownloadItems == null) DownloadItems = new List<DownloadItem>();
            if (BuildItems == null) BuildItems = new List<BuildItem>();
            if (forceIot == null) forceIot = false;
            if (dc_use == null) dc_use = false;
            if (enable_hibernation == null) enable_hibernation = false;
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
            if (aaf_info_iso == null) aaf_info_iso = true;
            if (aaf_info_system == null) aaf_info_system = true;
            if (aaf_info_boot == null) aaf_info_boot = true;

            if (oemkey_installer == null) oemkey_installer = true;
            if (oemkey_dism == null) oemkey_dism = true;
            if (oemkey_slmgr == null) oemkey_slmgr = false;

            if (DynamicDaylightTimeDisabled == null) DynamicDaylightTimeDisabled = true;
            if (DisableNtp == null) DisableNtp = true;
            if (RealTimeIsUniversal == null) RealTimeIsUniversal = true;

            if (services_stop == null) services_stop = "";
            if (services_start == null) services_start = "";
            if (services_stopOnlyList == null) services_stopOnlyList = false;
            if (services_startOnlyList == null) services_startOnlyList = false;
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
                return winBoxConfig;
            } catch (Exception ex) {}
            return null;
        }

        public bool isBuildEventsUsed()
        {
            return prebuildEnabled == true || postbuildEnabled == true || winmountedEnabled == true || downloadEnabled == true || buildEnabled == true;
        }
    }
}
