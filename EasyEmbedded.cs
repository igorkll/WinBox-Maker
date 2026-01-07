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
    //public partial class EasyEmbedded : Form
    {
        public EasyEmbedded(WinBoxProject winBoxProject) : base(winBoxProject, true)
        //public EasyEmbedded()
        {
            InitializeComponent();
            realInit(winBoxProject, true);
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
