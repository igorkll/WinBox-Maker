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
        }
    }
}
