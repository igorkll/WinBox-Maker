using System;
using System.Collections.Generic;
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

    public class WinBoxConfig
    {
        //public List<string>? Resources { get; set; }
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
        public int? WebSessionTimeout { get; set; }
        public int? ScreenTimeout { get; set; }
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


        public WinBoxConfig() {
            InitDefaults();
        }

        void InitDefaults()
        {
            //if (Resources == null) Resources = new List<string>();
            if (WinboxName == null) WinboxName = "Winbox Name";
            if (WinboxDescription == null) WinboxDescription = "Winbox Description";
            if (OemKey == null) OemKey = "";
            if (UseOemKey == null) UseOemKey = false;
            if (ProgramArgs == null) ProgramArgs = "";
            if (RawCommand == null) RawCommand = "";
            if (WebSite == null) WebSite = "";
            if (WebSessionTimeout == null) WebSessionTimeout = 0;
            if (ScreenTimeout == null) ScreenTimeout = 0;
            if (Architecture == null) Architecture = "x64";
            if (TweakList == null) TweakList = ["Integrate vc redist", "Disable boot circle", "Disable boot messages"];
            if (ProgramType == null) ProgramType = ProgramTypeEnum.ExecutableFile;
            if (LaunchMode == null) LaunchMode = ProgramLaunchModeEnum.insteadDesktop;
            if (AddVirtualDisplay == null) AddVirtualDisplay = false;
            if (VirtualDisplayWidth == null) VirtualDisplayWidth = 960;
            if (VirtualDisplayHeight == null) VirtualDisplayHeight = 640;
            if (UseEmbeddedDisplay == null) UseEmbeddedDisplay = false;
            if (CustomBootLogo_centering == null) CustomBootLogo_centering = false;
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
        }

        public void Save(string wnbFilePath)
        {
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
