using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WinBox_Maker
{
    public class WinBoxConfig
    {
        //public List<string>? Resources { get; set; }
        public string? BaseWindowsImage { get; set; }
        public string? BaseWindowsVersion { get; set; }
        public string? WinboxName { get; set; }
        public string? WinboxDescription { get; set; }

        public WinBoxConfig() {
            InitDefaults();
        }

        void InitDefaults()
        {
            //if (Resources == null) Resources = new List<string>();
            if (WinboxName == null) WinboxName = "Winbox Name";
            if (WinboxDescription == null) WinboxDescription = "Winbox Description";
        }

        public void Save(string wnbFilePath)
        {
            string json = JsonSerializer.Serialize(this);
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
