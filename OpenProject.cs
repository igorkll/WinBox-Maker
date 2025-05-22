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
    public partial class OpenProject : Form
    {
        public OpenProject()
        {
            InitializeComponent();
            this.Text = Program.version + " - " + this.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.OpenForm(this, new Editor());
        }
    }
}
