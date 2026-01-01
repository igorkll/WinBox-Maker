using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class FfuBuilder
{
    const uint FFU_SIGNATURE = 0x4655464D; // "MSFFU"
    const uint FFU_VERSION = 2;
    const int HEADER_SIZE = 4096;
    const int BLOCK_SIZE = 1024 * 1024; // 1MB

    public static void ConvertImgToFfu(string imgPath, string ffuPath)
    {
        byte[] img = File.ReadAllBytes(imgPath);

        long diskSize = img.Length;
        int blockCount = (int)Math.Ceiling(diskSize / (double)BLOCK_SIZE);

        using var sha = SHA256.Create();
        using var fs = new FileStream(ffuPath, FileMode.Create, FileAccess.Write);
        using var bw = new BinaryWriter(fs);

        /* ================= HEADER ================= */

        bw.Write(FFU_SIGNATURE);     // Signature
        bw.Write(FFU_VERSION);       // Version
        bw.Write((ulong)diskSize);   // Disk size
        bw.Write(BLOCK_SIZE);        // Block size
        bw.Write(blockCount);        // Number of blocks

        Guid diskGuid = Guid.NewGuid();
        bw.Write(diskGuid.ToByteArray());

        // Padding header to 4KB
        bw.Write(new byte[HEADER_SIZE - (int)fs.Position]);

        /* ================= BLOCK MAP ================= */

        long dataOffset = HEADER_SIZE + blockCount * 64; // simple fixed entry size

        for (int i = 0; i < blockCount; i++)
        {
            int offset = i * BLOCK_SIZE;
            int size = Math.Min(BLOCK_SIZE, img.Length - offset);

            byte[] block = new byte[BLOCK_SIZE];
            Array.Copy(img, offset, block, 0, size);

            byte[] hash = sha.ComputeHash(block);

            bw.Write(i);                  // Block index
            bw.Write((ulong)dataOffset);  // Offset in FFU
            bw.Write(BLOCK_SIZE);         // Block size
            bw.Write((byte)1);            // Present flag
            bw.Write(hash);               // SHA256

            dataOffset += BLOCK_SIZE;
        }

        /* ================= BLOCK DATA ================= */

        for (int i = 0; i < blockCount; i++)
        {
            int offset = i * BLOCK_SIZE;
            int size = Math.Min(BLOCK_SIZE, img.Length - offset);

            byte[] block = new byte[BLOCK_SIZE];
            Array.Copy(img, offset, block, 0, size);

            bw.Write(block);
        }
    }
}
