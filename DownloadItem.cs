using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinBox_Maker
{
    public class DownloadItem
    {
        public string? path { get; set; }
        public string? url { get; set; }
        public bool? cache { get; set; }
        public bool? unpack { get; set; }
    }
}
