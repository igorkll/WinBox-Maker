using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public OpenProjectForm()
        {
            InitializeComponent();
            this.Text = Program.version + " - " + this.Text;
        }

        private void OpenProject_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                openFileDialog.Filter = "WinBox projects (*.wnb)|*.wnb";
                openFileDialog.Title = "Open WinBox project";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Program.SwitchForm(this, new EditorForm(openFileDialog.FileName));
                }
            }
        }

        private void NewProject_Click(object sender, EventArgs e)
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
                        Program.SwitchForm(this, new EditorForm(Path.Combine(selectedPath, "winbox.wnb")));
                    }
                    else
                    {
                        MessageBox.Show("the selected directory is not empty. create a new directory for the project", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
