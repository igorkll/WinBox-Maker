using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;

namespace WinBox_Maker
{
    public partial class EasyEmbedded : EditorForm
    {
        bool guiEventsLock = false;
        bool loadingWindowsTask = false;
        bool windowsImagePathChanged = false;

        public EasyEmbedded(WinBoxProject winBoxProject) : base(winBoxProject, true)
        {
            InitializeComponent();

            this.Text = $"{WinBox_Maker.Program.version} - {this.Text}";
            this.winBoxProject = winBoxProject;
            Program.winBoxProject = winBoxProject;
            this.taskbarManager = TaskbarManager.Instance;

            ArchitectureSelect.Items.Clear();
            ArchitectureSelect.Items.Add("x64");
            ArchitectureSelect.Items.Add("x86");
            ArchitectureSelect.Items.Add("arm64");

            UnlockForm();
            if (winBoxProject.NeedLoadWindows())
            {
                UpdateGui();
                LoadWindowsTask();
            }
            else
            {
                UpdateWindowsVersionsList();
                UpdateGui();
            }

            eventWarningDelay();
        }

        async void LoadWindowsTask(bool firstLoaded = false)
        {
            if (loadingWindowsTask) return;

            loadingWindowsTask = true;
            LockForm();
            await winBoxProject.LoadWindowsImageAsync(UpdateProcessName, UpdateProcessValue);
            UnlockForm();
            UpdateWindowsVersionsList();
            UpdateGui();
            loadingWindowsTask = false;

            winBoxProject.SaveConfig();
        }

        void UpdateGui()
        {
            guiEventsLock = true;

            guiEventsLock = false;
            UpdateGuiWithoutWindowsVersion();
        }

        void UpdateGuiWithoutWindowsVersion()
        {
            guiEventsLock = true;
            WindowsName.Text = winBoxProject.winBoxConfig.BaseWindowsImage ?? "";

            bool canExport = winBoxProject.canExport();
            ExportIsoInstaller.Enabled = canExport;

            guiEventsLock = false;
        }

        void UnlockFormRecursion(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                control.Enabled = true;

                if (control.HasChildren)
                {
                    UnlockFormRecursion(control);
                }
            }
            UpdateGui();
        }

        void LockFormRecursion(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                control.Enabled = false;

                if (control.HasChildren)
                {
                    LockFormRecursion(control);
                }
            }
        }

        void UnlockForm()
        {
            UpdateProcessName(defaultProcessName);
            UpdateProcessValue(0);
            UnlockFormRecursion(this);
        }

        void LockForm()
        {
            LockFormRecursion(this);
        }

        private void UpdateProcessName(string text)
        {
            ProcessName.Text = text;
        }

        private void UpdateProcessValue(int Value)
        {
            ProcessValue.Value = Value;
            taskbarManager.SetProgressState(Value == 0 ? TaskbarProgressBarState.NoProgress : TaskbarProgressBarState.Normal, this.Handle);
            taskbarManager.SetProgressValue(Value, 100, this.Handle);
        }

        private async void CustomBootLogo_select_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, Program.imageFilter, winBoxProject.resourcesDirectoryPath, true);
            if (name != null)
            {
                winBoxProject.winBoxConfig.CustomBootLogo = name;
                //ImageConverter.ConvertToBmp_54_24(Path.Combine(winBoxProject.resourcesDirectoryPath, winBoxProject.winBoxConfig.CustomBootLogo), Path.Combine(winBoxProject.baseDirectoryPath, "winbox_temp", "debug.bmp"));
                winBoxProject.SaveConfig();
            }
            UnlockForm();
        }

        private void CustomBootLogo_clear_Click(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.CustomBootLogo = null;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private async void ExportIsoInstaller_Click(object sender, EventArgs e)
        {
            LockForm();
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = winBoxProject.buildDirectoryPath;
                saveFileDialog.Filter = "WinBox installer (*.iso)|*.iso";
                saveFileDialog.Title = $"Save you WinBox installer ({winBoxProject.winBoxConfig.WinboxName})";
                saveFileDialog.DefaultExt = "iso";
                saveFileDialog.FileName = winBoxProject.winBoxConfig.WinboxName;
                saveFileDialog.AddExtension = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    WindowsDescription windowsDescription = new WindowsDescription
                    {
                        name = winBoxProject.winBoxConfig.WinboxName,
                        description = winBoxProject.winBoxConfig.WinboxDescription
                    };
                    await winBoxProject.BuildIsoAsync(UpdateProcessName, UpdateProcessValue, saveFileDialog.FileName, windowsDescription);
                }
            }
            UnlockForm();
        }

        private void WindowsVersionSelect_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ArchitectureSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void ArchitectureSelect_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            winBoxProject.winBoxConfig.Architecture = ArchitectureSelect.Text;
            winBoxProject.SaveConfig();
        }

        private async void WindowsSelect_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, "Windows image (*.iso)|*.iso", winBoxProject.imagesDirectoryPath, false);
            UnlockForm();

            if (name != null)
            {
                winBoxProject.UnloadWindowsImage();
                winBoxProject.winBoxConfig.BaseWindowsImage = name;
                winBoxProject.SaveConfig();
                UpdateGui();
                LoadWindowsTask(true);
            }
        }

        private void WindowsName_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            windowsImagePathChanged = true;
            winBoxProject.UnloadWindowsImage();
            WindowsVersionSelect.Items.Clear();
            winBoxProject.winBoxConfig.BaseWindowsImage = WindowsName.Text;
            winBoxProject.winBoxConfig.BaseWindowsVersion = null;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void WindowsVersionSelect_TextChanged(object sender, EventArgs e)
        {
            if (guiEventsLock) return;

            if (winBoxProject.winBoxConfig.BaseWindowsImage == null)
            {
                WindowsVersionSelect.Text = null;
                return;
            }

            winBoxProject.winBoxConfig.BaseWindowsVersion = WindowsVersionSelect.Text;
            winBoxProject.SaveConfig();
            UpdateGuiWithoutWindowsVersion();
        }

        private void ee_file_select_Click(object sender, EventArgs e)
        {

        }

        private void ee_file_clear_Click(object sender, EventArgs e)
        {

        }

        private void ee_allfiles_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ee_onefile_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
