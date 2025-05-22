using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WinBox_Maker
{
    public class WinBoxConfig
    {
        public string? WinboxVersion { get; set; }

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
                return JsonSerializer.Deserialize<WinBoxConfig>(json);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
