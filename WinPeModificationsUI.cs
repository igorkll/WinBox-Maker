using Microsoft.WindowsAPICodePack.Taskbar;
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
        bool guiEventsLock = false;

        public WinPeModificationsUI(WinPeModifications winPeModifications, string titleSuffix)
        {
            InitializeComponent();

            this.winPeModifications = winPeModifications;
            this.Text = $"{this.Text} - {titleSuffix}";

            winboxRecoveryLogoType.Items.Clear();
            winboxRecoveryLogoType.Items.Add("Default Logo");
            winboxRecoveryLogoType.Items.Add("Custom Logo");
            winboxRecoveryLogoType.Items.Add("No Logo");

            UpdateGui();
        }

        void UpdateGui()
        {
            guiEventsLock = true;

            applyBaseSystemBCD.Checked = winPeModifications.applyBaseSystemBCD == true;
            app_override.Checked = winPeModifications.app_override == true;
            app_lowlevel.Checked = winPeModifications.app_lowlevel == true;
            remove_cmd_exe.Checked = winPeModifications.remove_cmd_exe == true;
            app_tab.SelectedIndex = (int)(winPeModifications.app_override_type ?? 0);
            app_custom_cmdline.Text = winPeModifications.app_custom_cmdline ?? "";

            app_tab.Enabled = winPeModifications.app_override == true;
            app_lowlevel.Enabled = winPeModifications.app_override == true;

            recovery_title.Text = winPeModifications.recovery_title ?? "";
            recovery_dataPaths.Text = winPeModifications.recovery_dataPaths ?? "";
            recovery_allowFactoryReset.Checked = winPeModifications.recovery_allowFactoryReset == true;
            recovery_allowFlashWithFactoryReset.Checked = winPeModifications.recovery_allowFactoryReset == true;
            recovery_allowFlashWithoutFactoryReset.Checked = winPeModifications.recovery_allowFlashWithoutFactoryReset == true;
            recovery_textOnInfoPage_en.Checked = winPeModifications.recovery_textOnInfoPage_en == true;
            recovery_textOnInfoPage.Enabled = winPeModifications.recovery_textOnInfoPage_en == true;
            recovery_textOnInfoPage.Text = winPeModifications.recovery_textOnInfoPage ?? "";
            recovery_wimName.Text = winPeModifications.recovery_wimName ?? "";
            recovery_imgName.Text = winPeModifications.recovery_imgName ?? "";
            recovery_ffuName.Text = winPeModifications.recovery_ffuName ?? "";
            recovery_allowFlashWim.Checked = winPeModifications.recovery_allowFlashWim == true;
            recovery_allowFlashImg.Checked = winPeModifications.recovery_allowFlashImg == true;
            recovery_allowFlashFfu.Checked = winPeModifications.recovery_allowFlashFfu == true;
            recovery_wimName.Enabled = winPeModifications.recovery_allowFlashWim == true;
            recovery_imgName.Enabled = winPeModifications.recovery_allowFlashImg == true;
            recovery_ffuName.Enabled = winPeModifications.recovery_allowFlashFfu == true;

            winboxRecoveryLogoType.SelectedIndex = (int)(winPeModifications.winboxRecoveryLogoType ?? 0);
            customRecoveryLogoPath.Text = winPeModifications.customRecoveryLogoPath ?? "";

            bool customRecoveryLogo = winPeModifications.winboxRecoveryLogoType == WinboxRecoveryLogoType.CustomLogo;
            customRecoveryLogoPath.Enabled = customRecoveryLogo;
            customRecoveryLogoPath_sel.Enabled = customRecoveryLogo;
            customRecoveryLogoPath_clr.Enabled = customRecoveryLogo;

            guiEventsLock = false;
        }

        private void applyBaseSystemBCD_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winPeModifications.applyBaseSystemBCD = applyBaseSystemBCD.Checked;
            Program.winBoxProject.SaveConfig();
        }

        private void app_override_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winPeModifications.app_override = app_override.Checked;
            Program.winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void app_custom_cmdline_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winPeModifications.app_custom_cmdline = app_custom_cmdline.Text;
            Program.winBoxProject.SaveConfig();
        }

        private void app_tab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winPeModifications.app_override_type = (AppOverrideType)app_tab.SelectedIndex;
            Program.winBoxProject.SaveConfig();
        }

        private void app_lowlevel_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winPeModifications.app_lowlevel = app_lowlevel.Checked;
            Program.winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void remove_cmd_exe_CheckedChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winPeModifications.remove_cmd_exe = remove_cmd_exe.Checked;
            Program.winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void winboxRecoveryLogoType_SelectedIndexChanged(object sender, EventArgs e)
        {
            winPeModifications.winboxRecoveryLogoType = (WinboxRecoveryLogoType)winboxRecoveryLogoType.SelectedIndex;
            winPeModifications.remove_cmd_exe = remove_cmd_exe.Checked;
            Program.winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void recovery_textOnInfoPage_en_CheckedChanged(object sender, EventArgs e)
        {
            winPeModifications.recovery_textOnInfoPage_en = recovery_textOnInfoPage_en.Checked;
            Program.winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void recovery_allowFlashWim_CheckedChanged(object sender, EventArgs e)
        {
            winPeModifications.recovery_allowFlashWim = recovery_allowFlashWim.Checked;
            Program.winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void recovery_allowFlashImg_CheckedChanged(object sender, EventArgs e)
        {
            winPeModifications.recovery_allowFlashImg = recovery_allowFlashImg.Checked;
            Program.winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void recovery_allowFlashFfu_CheckedChanged(object sender, EventArgs e)
        {
            winPeModifications.recovery_allowFlashFfu = recovery_allowFlashFfu.Checked;
            Program.winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void recovery_allowFlashWithoutFactoryReset_CheckedChanged(object sender, EventArgs e)
        {
            winPeModifications.recovery_allowFlashWithoutFactoryReset = recovery_allowFlashWithoutFactoryReset.Checked;
            Program.winBoxProject.SaveConfig();
        }

        private void recovery_allowFlashWithFactoryReset_CheckedChanged(object sender, EventArgs e)
        {
            winPeModifications.recovery_allowFlashWithFactoryReset = recovery_allowFlashWithFactoryReset.Checked;
            Program.winBoxProject.SaveConfig();
        }

        private void recovery_allowFactoryReset_CheckedChanged(object sender, EventArgs e)
        {
            winPeModifications.recovery_allowFactoryReset = recovery_allowFactoryReset.Checked;
            Program.winBoxProject.SaveConfig();
        }

        private void recovery_textOnInfoPage_TextChanged(object sender, EventArgs e)
        {
            winPeModifications.recovery_textOnInfoPage = recovery_textOnInfoPage.Text;
            Program.winBoxProject.SaveConfig();
        }

        private void customRecoveryLogoPath_TextChanged(object sender, EventArgs e)
        {
            winPeModifications.customRecoveryLogoPath = customRecoveryLogoPath.Text;
            Program.winBoxProject.SaveConfig();
        }

        private void UpdateProcessName(string text)
        {
        }

        private void UpdateProcessValue(int Value)
        {
        }

        void UnlockForm()
        {
            this.Enabled = true;
        }

        void LockForm()
        {
            this.Enabled = false;
        }

        private async void customRecoveryLogoPath_sel_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await Program.winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, Program.imageFilter, Program.winBoxProject.resourcesDirectoryPath, true);
            if (name != null)
            {
                winPeModifications.customRecoveryLogoPath = name;
                Program.winBoxProject.SaveConfig();
                UpdateGui();
            }
            UnlockForm();
        }

        private void customRecoveryLogoPath_clr_Click(object sender, EventArgs e)
        {
            winPeModifications.customRecoveryLogoPath = null;
            Program.winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void recovery_title_TextChanged(object sender, EventArgs e)
        {
            winPeModifications.recovery_title = recovery_title.Text;
            Program.winBoxProject.SaveConfig();
        }

        private void recovery_dataPaths_TextChanged(object sender, EventArgs e)
        {
            winPeModifications.recovery_dataPaths = recovery_dataPaths.Text;
            Program.winBoxProject.SaveConfig();
        }

        private void recovery_ffuName_TextChanged(object sender, EventArgs e)
        {
            winPeModifications.recovery_ffuName = recovery_ffuName.Text;
            Program.winBoxProject.SaveConfig();
        }

        private void recovery_imgName_TextChanged(object sender, EventArgs e)
        {
            winPeModifications.recovery_imgName = recovery_imgName.Text;
            Program.winBoxProject.SaveConfig();
        }

        private void recovery_wimName_TextChanged(object sender, EventArgs e)
        {
            winPeModifications.recovery_wimName = recovery_wimName.Text;
            Program.winBoxProject.SaveConfig();
        }
    }
}
