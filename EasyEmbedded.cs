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
    public partial class EasyEmbedded : EditorForm
    {
        public EasyEmbedded(WinBoxProject winBoxProject) : base(winBoxProject, true)
        {
            InitializeComponent();

            this.taskbarManager = TaskbarManager.Instance;
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
