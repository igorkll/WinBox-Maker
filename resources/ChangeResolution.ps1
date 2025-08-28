param (
    [int]$Width = -1,
    [int]$Height = -1,
    [int]$BitDepth = -1,
    [int]$Refresh = -1,
    [int]$Orientation = -1
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
        private const int DM_DISPLAYORIENTATION = 0x80;

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

    public static bool SetDisplay(int width, int height, int bits, int freq, int orientation)
    {
        DEVMODE dm = new DEVMODE();
        dm.dmSize = (short)System.Runtime.InteropServices.Marshal.SizeOf(typeof(DEVMODE));

        if (0 != EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref dm))
        {
            // Флаги для полей
            const int DM_PELSWIDTH        = 0x80000;
            const int DM_PELSHEIGHT       = 0x100000;
            const int DM_BITSPERPEL       = 0x40000;
            const int DM_DISPLAYFREQUENCY = 0x400000;
            const int DM_DISPLAYORIENTATION = 0x80;

            dm.dmFields = 0; // обнуляем и будем выставлять только то, что нужно

            if (width >= 0)
            {
                dm.dmPelsWidth = width;
                dm.dmFields |= DM_PELSWIDTH;
            }

            if (height >= 0)
            {
                dm.dmPelsHeight = height;
                dm.dmFields |= DM_PELSHEIGHT;
            }

            if (bits >= 0)
            {
                dm.dmBitsPerPel = bits;
                dm.dmFields |= DM_BITSPERPEL;
            }

            if (freq >= 0)
            {
                dm.dmDisplayFrequency = freq;
                dm.dmFields |= DM_DISPLAYFREQUENCY;
            }

            if (orientation >= 0)
            {
                dm.dmDisplayOrientation = orientation;
                dm.dmFields |= DM_DISPLAYORIENTATION;
            }

            int iRet = ChangeDisplaySettings(ref dm, CDS_UPDATEREGISTRY);
            return iRet == DISP_CHANGE_SUCCESSFUL;
        }

        return false;
    }
}
"@

# --- Apply resolution, color depth, refresh rate and orientation ---
if (-not [Display]::SetDisplay($Width, $Height, $BitDepth, $Refresh, $Orientation)) {
    Write-Host "Failed to change display settings!"
} else {
    Write-Host "Resolution: ${Width}x${Height}, Bit depth: ${BitDepth}, Refresh: ${Refresh}Hz, Orientation: ${Orientation}"
}
