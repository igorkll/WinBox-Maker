using System;
using System.IO;
using System.Runtime.InteropServices;

namespace WinBox_Maker
{
    public class BootresPatcher
    {
        // Структуры для работы с PE файлами
        [StructLayout(LayoutKind.Sequential)]
        public struct IMAGE_DOS_HEADER
        {
            public ushort e_magic;
            public ushort e_cblp;
            public ushort e_cp;
            public ushort e_crlc;
            public ushort e_cparhdr;
            public ushort e_minalloc;
            public ushort e_maxalloc;
            public ushort e_ss;
            public ushort e_sp;
            public ushort e_csum;
            public ushort e_ip;
            public ushort e_cs;
            public ushort e_lfarlc;
            public ushort e_ovno;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public ushort[] e_res1;
            public ushort e_oemid;
            public ushort e_oeminfo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public ushort[] e_res2;
            public int e_lfanew;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IMAGE_RESOURCE_DIRECTORY
        {
            public uint Characteristics;
            public uint TimeDateStamp;
            public ushort MajorVersion;
            public ushort MinorVersion;
            public ushort NumberOfNamedEntries;
            public ushort NumberOfIdEntries;
        }

        private const ushort IMAGE_DOS_SIGNATURE = 0x5A4D; // "MZ"
        private const uint IMAGE_NT_SIGNATURE = 0x00004550; // "PE\0\0"

        public bool PatchBootres(string bootResPath, string newLogoPath)
        {
            try
            {
                // Читаем файл bootres.dll
                byte[] bootResData = File.ReadAllBytes(bootResPath);

                // Проверяем сигнатуру MZ
                if (!IsValidDosHeader(bootResData))
                {
                    Console.WriteLine("Неверный формат PE файла");
                    return false;
                }

                // Находим ресурсы и заменяем логотип
                if (FindAndReplaceLogoResource(bootResData, newLogoPath))
                {
                    // Сохраняем измененный файл
                    File.WriteAllBytes(bootResPath, bootResData);
                    Console.WriteLine("Логотип успешно заменен!");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                return false;
            }
        }

        private bool IsValidDosHeader(byte[] fileData)
        {
            if (fileData.Length < 64) return false;

            ushort signature = BitConverter.ToUInt16(fileData, 0);
            return signature == IMAGE_DOS_SIGNATURE;
        }

        private bool FindAndReplaceLogoResource(byte[] fileData, string newLogoPath)
        {
            try
            {
                // Читаем новый логотип
                byte[] newLogoData = File.ReadAllBytes(newLogoPath);

                // Ищем ресурс логотипа (обычно это BITMAP с определенным ID)
                // Для bootres.dll логотип часто находится в ресурсах с ID 1
                int resourceOffset = FindResourceOffset(fileData, 2, 1); // RT_BITMAP = 2

                if (resourceOffset == -1)
                {
                    Console.WriteLine("Ресурс логотипа не найден");
                    return false;
                }

                // Заменяем данные ресурса
                ReplaceResourceData(fileData, resourceOffset, newLogoData);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при замене ресурса: {ex.Message}");
                return false;
            }
        }

        private int FindResourceOffset(byte[] fileData, int resourceType, int resourceId)
        {
            // Здесь должна быть реализация поиска смещения ресурса
            // Это сложная задача, требующая парсинга PE структуры

            // Упрощенный подход - поиск по сигнатурам
            // Для BMP файлов ищем сигнатуру "BM"
            byte[] bmpSignature = { 0x42, 0x4D }; // "BM"

            for (int i = 0; i < fileData.Length - bmpSignature.Length; i++)
            {
                if (fileData[i] == bmpSignature[0] && fileData[i + 1] == bmpSignature[1])
                {
                    // Проверяем, что это действительно BMP ресурс подходящего размера
                    if (IsValidBmpResource(fileData, i))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        private bool IsValidBmpResource(byte[] fileData, int offset)
        {
            // Проверяем размер BMP (первые 4 байта после сигнатуры - размер файла)
            if (offset + 6 >= fileData.Length) return false;

            int fileSize = BitConverter.ToInt32(fileData, offset + 2);
            return fileSize > 0 && fileSize < 1024 * 1024; // Разумный размер для логотипа
        }

        private void ReplaceResourceData(byte[] fileData, int offset, byte[] newData)
        {
            // Проверяем, что новые данные помещаются
            if (offset + newData.Length > fileData.Length)
            {
                // Если не помещаются, увеличиваем файл
                Array.Resize(ref fileData, offset + newData.Length);
            }

            // Копируем новые данные
            Array.Copy(newData, 0, fileData, offset, newData.Length);
        }
    }
}