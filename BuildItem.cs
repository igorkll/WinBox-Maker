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
        custom,
        electron_packager
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


        public string? electron_packager_path { get; set; }
        public string? electron_packager_name { get; set; }


        public void initDefaults()
        {
            if (name == null) name = "";
            if (type == null) type = BuildItemType.msbuild;
            if (subdirectory == null) subdirectory = "";
            if (subdirectory_enabled == null) subdirectory_enabled = false;
            if (msbuild_path == null) msbuild_path = "";
            if (msbuild_configuration == null) msbuild_configuration = "Release";
            if (cmake_path == null) cmake_path = "";
            if (cmake_configuration == null) cmake_configuration = "Release";
            if (cargo_path == null) cargo_path = "";
            if (custom_path == null) custom_path = "";
            if (custom_command == null) custom_command = "";
            if (electron_packager_path == null) electron_packager_path = "";
            if (electron_packager_name == null) electron_packager_name = "electron_app";
        }

    }
}
