using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace WinBox_Maker
{
    internal class ImageConverter
    {
        public static void ConvertToBmp_54_24(string path, string outputPath) //54 header lenght, 24 bits per color
        {
            using (Bitmap bitmap = new Bitmap(path))
            {
                SaveBmp_54_24(bitmap, outputPath);
            }
        }

        public static void SaveBmp_54_24(Bitmap bitmap, string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                byte[] bmpHeader = new byte[54];
                int width = bitmap.Width;
                int height = bitmap.Height;
                int rowSize = (width * 3 + 3) & ~3;
                int pixelArraySize = rowSize * height;

                bmpHeader[0] = (byte)'B';
                bmpHeader[1] = (byte)'M';
                BitConverter.GetBytes(54 + pixelArraySize).CopyTo(bmpHeader, 2);
                bmpHeader[10] = 54;
                bmpHeader[14] = 40;
                BitConverter.GetBytes(width).CopyTo(bmpHeader, 18);
                BitConverter.GetBytes(height).CopyTo(bmpHeader, 22);
                bmpHeader[26] = 1;
                bmpHeader[28] = 24;

                fs.Write(bmpHeader, 0, bmpHeader.Length);

                for (int y = height - 1; y >= 0; y--)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Color pixel = bitmap.GetPixel(x, y);
                        float alpha = pixel.A / 255.0f;
                        byte finalR = (byte)(alpha * pixel.R);
                        byte finalG = (byte)(alpha * pixel.G);
                        byte finalB = (byte)(alpha * pixel.B);
                        fs.WriteByte(finalB);
                        fs.WriteByte(finalG);
                        fs.WriteByte(finalR);
                    }

                    for (int padding = 0; padding < rowSize - (width * 3); padding++)
                    {
                        fs.WriteByte(0);
                    }
                }
            }
        }
    }
}
