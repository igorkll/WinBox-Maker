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

namespace WinBox_Maker
{
    public partial class ProgramSettings : Form
    {
        Action exitCallback;

        public ProgramSettings(Action _exitCallback)
        {
            InitializeComponent();
            exitCallback = _exitCallback;
            this.FormClosing += new FormClosingEventHandler(FormClosingCallback);
            UpdateGui();
        }

        public void UpdateGui()
        {
            msbuildPath.Text = Program.winboxSettings.path_msbuild;
            cmakePath.Text = Program.winboxSettings.path_cmake;
            pipPath.Text = Program.winboxSettings.path_pip;
        }

        private void FormClosingCallback(object? sender, FormClosingEventArgs e)
        {
            exitCallback();
        }

        private void AutoDetect_Click(object sender, EventArgs e)
        {
            Program.winboxSettings.AutoDetect();
            UpdateGui();
        }

        private void msbuildPath_TextChanged(object sender, EventArgs e)
        {
            Program.winboxSettings.path_msbuild = msbuildPath.Text;
            Program.winboxSettings.Save();
        }

        private void cmakePath_TextChanged(object sender, EventArgs e)
        {
            Program.winboxSettings.path_cmake = cmakePath.Text;
            Program.winboxSettings.Save();
        }

        private void pipPath_TextChanged(object sender, EventArgs e)
        {
            Program.winboxSettings.path_pip = pipPath.Text;
            Program.winboxSettings.Save();
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
    }
}
