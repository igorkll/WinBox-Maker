using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinBox_Maker
{
    public class WinPeModifications
    {
        public bool? enabled { get; set; }

        public void initDefaults()
        {
            if (enabled == null) enabled = true;
        }

        // ------------------------------

        public void openGui()
        {

        }

        public async Task modMountedWim(string mountedPath)
        {
            if (enabled != true) return;
            await BcdChanger.modifyWinBCD(mountedPath, this);
        }

        public async Task modMountedIso(string mountedPath)
        {
            if (enabled != true) return;
            await BcdChanger.modifyWinBCD(mountedPath, this);
        }
    }
}
