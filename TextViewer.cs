using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinBox_Maker
{
    public partial class TextViewer : Form
    {
        public TextViewer(string path)
        {
            InitializeComponent();
            this.Text = $"TextViewer - {Path.GetFileNameWithoutExtension(path)}";
            this.Textbox.Text = File.ReadAllText(path);
        }
    }
}
