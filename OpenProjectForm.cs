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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WinBox_Maker
{
    public partial class OpenProjectForm : Form
    {
        public EditorForm? editorForm = null;

        public OpenProjectForm()
        {
            InitializeComponent();
            this.Text = Program.version + " - " + this.Text;
        }

        void LoadProject(string path)
        {
            WinBoxProject winBoxProject = new WinBoxProject(path);
            string? err = winBoxProject.GetError();
            if (err != null)
            {
                MessageBox.Show(err, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            editorForm = new EditorForm(winBoxProject);
            Program.SwitchForm(this, editorForm);
        }

        private async void OpenProject_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                openFileDialog.Filter = "WinBox projects (*.wnb)|*.wnb";
                openFileDialog.Title = "Open WinBox project";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadProject(openFileDialog.FileName);
                    await Telemetry.sendTelemetry(TelemetryPackageType.LoadProject, Path.GetDirectoryName(openFileDialog.FileName));
                }
            }
        }

        private async void NewProject_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                folderBrowserDialog.Description = "select the directory where the winbox project will be created (create a separate directory)";
                folderBrowserDialog.ShowNewFolderButton = true;

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderBrowserDialog.SelectedPath;
                    if (Program.IsDirectoryEmpty(selectedPath))
                    {
                        LoadProject(Path.Combine(selectedPath, "winbox.wnb"));
                        await Telemetry.sendTelemetry(TelemetryPackageType.NewProject, selectedPath);
                    }
                    else
                    {
                        MessageBox.Show("the selected directory is not empty. create a new directory for the project", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void logichub_Click(object sender, EventArgs e)
        {
            Program.OpenWebPage(Program.logichubUrl);
        }

        private void README_Click(object sender, EventArgs e)
        {
            Form form = new TextViewer(Program.ResourcePath("README.md"));
            form.Show();
        }

        private void LICENSE_Click(object sender, EventArgs e)
        {
            Form form = new TextViewer(Program.ResourcePath("LICENSE.txt"));
            form.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            WinBox_Maker.Program.OpenWebPage(WinBox_Maker.Program.logichubUrl + "#winbox");
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            WinBox_Maker.Program.OpenWebPage(WinBox_Maker.Program.logichubUrl);
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
        }

        void UnlockForm()
        {
            UnlockFormRecursion(this);
        }

        void LockForm()
        {
            OpenProject.Enabled = false;
            NewProject.Enabled = false;
            pictureBox5.Enabled = false;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            LockForm();
            Form form = new ProgramSettings(UnlockForm);
            form.Show();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void EasyEmbedded_Click(object sender, EventArgs e)
        {

        }
    }
}
