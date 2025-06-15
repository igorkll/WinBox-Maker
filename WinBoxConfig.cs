using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WinBox_Maker
{
    public enum ProgramModeEnum
    {
        AfterExplorer,
        InsteadExplorer
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
        public bool? ProgramAsAdmin { get; set; }
        public bool? disable_lockscreen { get; set; }
        public ProgramModeEnum? ProgramMode { get; set; }

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
            if (ProgramAsAdmin == null) ProgramAsAdmin = true;
            if (disable_lockscreen == null) disable_lockscreen = true;
            if (ProgramMode == null) ProgramMode = ProgramModeEnum.InsteadExplorer;
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
