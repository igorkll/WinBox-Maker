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
        bool loadingWindowsTask = false;

        public EasyEmbedded(WinBoxProject winBoxProject) : base(winBoxProject, true)
        {
            InitializeComponent();

            this.Text = $"{WinBox_Maker.Program.version} - {this.Text} (EaseEmbedded)";
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

        private void CustomBootLogo_select_Click(object sender, EventArgs e)
        {

        }

        private void CustomBootLogo_clear_Click(object sender, EventArgs e)
        {

        }

        private void ExportIsoInstaller_Click(object sender, EventArgs e)
        {

        }

        private void WindowsVersionSelect_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ArchitectureSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void ArchitectureSelect_TextChanged(object sender, EventArgs e)
        {

        }

        private void WindowsSelect_Click(object sender, EventArgs e)
        {

        }

        private void WindowsName_TextChanged(object sender, EventArgs e)
        {
        }

        private void WindowsVersionSelect_TextChanged(object sender, EventArgs e)
        {
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
