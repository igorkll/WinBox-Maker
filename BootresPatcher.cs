using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinBox_Maker
{
    static class BootresPatcher
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr BeginUpdateResource(string pFileName, [MarshalAs(UnmanagedType.Bool)] bool bDeleteExistingResources);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool UpdateResource(IntPtr hUpdate, IntPtr lpType, IntPtr lpName, ushort wLanguage, byte[] lpData, uint cbData);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool EndUpdateResource(IntPtr hUpdate, bool fDiscard);


        static public void PatchBootres(string dllPath, string newimagePath) {
            string tempLogo = Path.GetTempFileName();
            ImageConverter.ConvertToBmp_54_24(newimagePath, tempLogo);

            byte[] newBmpData = System.IO.File.ReadAllBytes(tempLogo);

            IntPtr h = BeginUpdateResource(dllPath, false);

            if (h == IntPtr.Zero)
            {
                Program.Error("Ошибка BeginUpdateResource");
                return;
            }

            if (!UpdateResource(h, (IntPtr)2, (IntPtr)1, 0x0409, newBmpData, (uint)newBmpData.Length))
            {
                Program.Error("Ошибка UpdateResource");
                return;
            }

            if (!EndUpdateResource(h, false))
            {
                Program.Error("Ошибка EndUpdateResource");
                return;
            }

            File.Delete(tempLogo);
        }
    }
}
