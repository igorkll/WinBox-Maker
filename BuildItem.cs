using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinBox_Maker
{
    public enum BuildItemType
    {
        msbuild,
        cmake,
        cargo,
        custom
    }

    public class BuildItem
    {
        public string? name { get; set; }
        public BuildItemType? type { get; set; }
        public string? subdirectory { get; set; }
        public bool subdirectory_enabled { get; set; }


        public string? msbuild_path { get; set; }
        public string? msbuild_configuration { get; set; }

        public string? cmake_path { get; set; }
        public string? cmake_configuration { get; set; }


        public string? cargo_path { get; set; }


        public string? custom_path { get; set; }
        public string? custom_command { get; set; }
    }
}
