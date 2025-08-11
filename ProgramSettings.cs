using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WinBox_Maker
{
    public partial class ProgramSettings : Form
    {
        Action exitCallback;
        bool guiUpdate = false;

        public ProgramSettings(Action _exitCallback)
        {
            InitializeComponent();
            exitCallback = _exitCallback;
            this.FormClosing += new FormClosingEventHandler(FormClosingCallback);
            UpdateGui();
        }

        public void UpdateGui()
        {
            guiUpdate = true;
            msbuildPath.Text = Program.winboxSettings.path_msbuild;
            cmakePath.Text = Program.winboxSettings.path_cmake;
            pipPath.Text = Program.winboxSettings.path_pip;
            cargoPath.Text = Program.winboxSettings.path_cargo;
            qemuPath.Text = Program.winboxSettings.path_qemu_folder;
            guiUpdate = false;
        }

        private void FormClosingCallback(object? sender, FormClosingEventArgs e)
        {
            exitCallback();
        }

        private void AutoDetect_Click(object sender, EventArgs e)
        {
            Program.winboxSettings.AutoDetect(false, true);
            UpdateGui();
        }

        private void openProgramData_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", Program.appdataPath);
            }
            catch (Exception ex)
            {
            }
        }

        private void msbuildPath_TextChanged(object sender, EventArgs e)
        {
            if (guiUpdate) return;

            Program.winboxSettings.path_msbuild = msbuildPath.Text;
            Program.winboxSettings.Save();
        }

        private void cmakePath_TextChanged(object sender, EventArgs e)
        {
            if (guiUpdate) return;

            Program.winboxSettings.path_cmake = cmakePath.Text;
            Program.winboxSettings.Save();
        }

        private void pipPath_TextChanged(object sender, EventArgs e)
        {
            if (guiUpdate) return;

            Program.winboxSettings.path_pip = pipPath.Text;
            Program.winboxSettings.Save();
        }

        private void cargoPath_TextChanged(object sender, EventArgs e)
        {
            if (guiUpdate) return;

            Program.winboxSettings.path_cargo = cargoPath.Text;
            Program.winboxSettings.Save();
        }

        private void qemuPath_TextChanged(object sender, EventArgs e)
        {
            if (guiUpdate) return;

            Program.winboxSettings.path_qemu_folder = qemuPath.Text;
            Program.winboxSettings.Save();
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
                if (control.Name != "openProgramData")
                {
                    control.Enabled = false;
                }

                if (control.HasChildren)
                {
                    LockFormRecursion(control);
                }
            }
        }

        void UnlockForm()
        {
            UnlockFormRecursion(this);
        }

        void LockForm()
        {
            LockFormRecursion(this);
        }

        string? selectFile(string name)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*",
                Title = $"Select {name}"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }

            return null;
        }

        string? selectFolder(string name)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                Description = $"Select {name}",
                ShowNewFolderButton = true
            };

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                return folderBrowserDialog.SelectedPath;
            }

            return null;
        }

        private void selectMsbuild_Click(object sender, EventArgs e)
        {
            LockForm();
            string? path = selectFile("msbuild");
            if (path != null)
            {
                Program.winboxSettings.path_msbuild = path;
                Program.winboxSettings.Save();
            }
            UnlockForm();
        }

        private void selectCmake_Click(object sender, EventArgs e)
        {
            LockForm();
            string? path = selectFile("cmake");
            if (path != null)
            {
                Program.winboxSettings.path_cmake = path;
                Program.winboxSettings.Save();
            }
            UnlockForm();
        }

        private void selectPip_Click(object sender, EventArgs e)
        {
            LockForm();
            string? path = selectFile("pip");
            if (path != null)
            {
                Program.winboxSettings.path_pip = path;
                Program.winboxSettings.Save();
            }
            UnlockForm();
        }

        private void selectCargo_Click(object sender, EventArgs e)
        {
            LockForm();
            string? path = selectFile("cargo");
            if (path != null)
            {
                Program.winboxSettings.path_cargo = path;
                Program.winboxSettings.Save();
            }
            UnlockForm();
        }

        private void selectQemu_Click(object sender, EventArgs e)
        {
            LockForm();
            string? path = selectFolder("qemu");
            if (path != null)
            {
                Program.winboxSettings.path_qemu_folder = path;
                Program.winboxSettings.Save();
            }
            UnlockForm();
        }
    }
}
