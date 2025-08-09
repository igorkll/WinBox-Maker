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
        }

        private void FormClosingCallback(object? sender, FormClosingEventArgs e)
        {
            exitCallback();
        }
    }
}
