using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinBox_Maker
{
    public partial class WinPeModificationsUI : Form
    {
        WinPeModifications winPeModifications;

        public WinPeModificationsUI(WinPeModifications winPeModifications, string titleSuffix)
        {
            InitializeComponent();

            this.winPeModifications = winPeModifications;
            this.Text = $"{this.Text} - {titleSuffix}";

            UpdateGui();
        }

        void UpdateGui()
        {
            applyBaseSystemBCD.Checked = winPeModifications.applyBaseSystemBCD == true;
            app_override.Checked = winPeModifications.app_override == true;
            app_tab.SelectedIndex = (int)(winPeModifications.appOverrideType ?? 0);
            app_custom_cmdline.Text = winPeModifications.app_custom_cmdline ?? "";

            app_tab.Enabled = winPeModifications.app_override == true;
        }

        private void applyBaseSystemBCD_CheckedChanged(object sender, EventArgs e)
        {
            winPeModifications.applyBaseSystemBCD = applyBaseSystemBCD.Checked;
            Program.winBoxProject.SaveConfig();
        }

        private void app_override_CheckedChanged(object sender, EventArgs e)
        {
            winPeModifications.app_override = app_override.Checked;
            Program.winBoxProject.SaveConfig();
        }

        private void app_custom_cmdline_TextChanged(object sender, EventArgs e)
        {
            winPeModifications.app_custom_cmdline = app_custom_cmdline.Text;
            Program.winBoxProject.SaveConfig();
        }

        private void app_tab_SelectedIndexChanged(object sender, EventArgs e)
        {
            winPeModifications.appOverrideType = (AppOverrideType)app_tab.SelectedIndex;
            Program.winBoxProject.SaveConfig();
        }
    }
}
