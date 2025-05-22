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
    }
}
