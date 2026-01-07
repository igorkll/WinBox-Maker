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
        public EasyEmbedded(WinBoxProject winBoxProject) : base(winBoxProject, false)
        {
            InitializeComponent();

        }
    }
}
