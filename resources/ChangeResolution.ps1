param (
    [int]$Width,
    [int]$Height,
    [int]$BitDepth = 32,
    [int]$Refresh = 60,
    [int]$Scaling = 100
)

Add-Type -TypeDefinition @"
using System;
using System.Runtime.InteropServices;

public class Display {
    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
    public struct DEVMODE {
        private const int DM_PELSWIDTH = 0x80000;
        private const int DM_PELSHEIGHT = 0x100000;
        private const int DM_BITSPERPEL = 0x40000;
        private const int DM_DISPLAYFREQUENCY = 0x400000;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=32)]
        public string dmDeviceName;
        public short dmSpecVersion;
        public short dmDriverVersion;
        public short dmSize;
        public short dmDriverExtra;
        public int dmFields;

        public int dmPositionX;
        public int dmPositionY;
        public int dmDisplayOrientation;
        public int dmDisplayFixedOutput;

        public short dmColor;
        public short dmDuplex;
        public short dmYResolution;
        public short dmTTOption;
        public short dmCollate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst=32)]
        public string dmFormName;
        public short dmLogPixels;
        public int dmBitsPerPel;
        public int dmPelsWidth;
        public int dmPelsHeight;
        public int dmDisplayFlags;
        public int dmDisplayFrequency;
        public int dmICMMethod;
        public int dmICMIntent;
        public int dmMediaType;
        public int dmDitherType;
        public int dmReserved1;
        public int dmReserved2;
        public int dmPanningWidth;
        public int dmPanningHeight;
    };

    [DllImport("user32.dll")]
    public static extern int EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);

    [DllImport("user32.dll")]
    public static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);

    public const int ENUM_CURRENT_SETTINGS = -1;
    public const int CDS_UPDATEREGISTRY = 0x01;
    public const int DISP_CHANGE_SUCCESSFUL = 0;
    public const int DISP_CHANGE_RESTART = 1;

    public static bool SetResolution(int width, int height, int bits, int freq) {
        DEVMODE dm = new DEVMODE();
        dm.dmSize = (short)System.Runtime.InteropServices.Marshal.SizeOf(typeof(DEVMODE));
        if (0 != EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref dm)) {
            dm.dmPelsWidth = width;
            dm.dmPelsHeight = height;
            dm.dmBitsPerPel = bits;
            dm.dmDisplayFrequency = freq;
            dm.dmFields = 0x180000 | 0x40000 | 0x400000; // Width + Height + BitDepth + Refresh
            int iRet = ChangeDisplaySettings(ref dm, CDS_UPDATEREGISTRY);
            return iRet == DISP_CHANGE_SUCCESSFUL;
        }
        return false;
    }
}
"@

# --- Change resolution, bit depth and refresh rate ---
if (-not [Display]::SetResolution($Width, $Height, $BitDepth, $Refresh)) {
    Write-Host "Failed to change display settings!"
} else {
    Write-Host "Resolution set to ${Width}x${Height}, Bit depth: ${BitDepth}, Refresh rate: ${Refresh}Hz"
}

# --- Change scaling (DPI) ---
# Windows stores DPI scaling in the registry
# LogPixels = base DPI (96 * scale%), Win8DpiScaling must be set to 1
$scaleValue = $Scaling * 10
$regPath = "HKCU:\Control Panel\Desktop"

Set-ItemProperty -Path $regPath -Name "LogPixels" -Type DWord -Value ([int](96 * $Scaling / 100)) -Force
Set-ItemProperty -Path $regPath -Name "Win8DpiScaling" -Type DWord -Value 1 -Force
Set-ItemProperty -Path $regPath -Name "DpiScalingVer" -Type DWord -Value 0x00001000 -Force

Write-Host "Scaling set to ${Scaling}% (requires logoff/login or explorer.exe restart)"
