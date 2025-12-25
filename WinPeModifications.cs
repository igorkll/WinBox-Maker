using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public AppOverrideType? appOverrideType { get; set; }
        public string? app_custom_cmdline { get; set; }

        public void initDefaults()
        {
            if (enabled == null) enabled = true;
            if (applyBaseSystemBCD == null) applyBaseSystemBCD = true;
            if (app_override == null) app_override = false;
            if (appOverrideType == null) appOverrideType = AppOverrideType.WinboxMakerRecovery;
            if (app_custom_cmdline == null) app_custom_cmdline = "";
        }

        // ------------------------------

        public void openGui(string titleSuffix)
        {
            WinPeModificationsUI newForm = new WinPeModificationsUI(this, titleSuffix);
            newForm.ShowDialog();
        }

        public async Task modMountedWim(string mountedPath)
        {
            if (enabled != true) return;
            await BcdChanger.modifyWinBCD(mountedPath, this);
        }

        public async Task modMountedIso(string mountedPath)
        {
            if (enabled != true) return;
            await BcdChanger.modifyWinBCD(mountedPath, this);
        }
    }
}
