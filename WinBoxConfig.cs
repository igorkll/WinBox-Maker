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
        WebSite
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
        public int? WebSessionTimeout { get; set; }
        public int? ScreenTimeout { get; set; }
        public ProgramTypeEnum? ProgramType { get; set; }

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
            if (ProgramType == null) ProgramType = ProgramTypeEnum.ExecutableFile;
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
    }
}
