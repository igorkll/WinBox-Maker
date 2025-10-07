using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WinBox_Maker
{
    static public class RegPatcher
    {
        static public void regPatcher(string regPath, string newRegPath)
        {
            if (string.IsNullOrWhiteSpace(regPath)) throw new ArgumentException(nameof(regPath));
            if (string.IsNullOrWhiteSpace(newRegPath)) throw new ArgumentException(nameof(newRegPath));
            if (!File.Exists(regPath)) throw new FileNotFoundException("Исходный reg файл не найден", regPath);

            // Нормализуем newRegPath (уберём лишние слеши, пробелы)
            newRegPath = newRegPath.Trim();
            // Убедимся, что нет завершающего '\'
            if (newRegPath.EndsWith("\\") || newRegPath.EndsWith("/"))
                newRegPath = newRegPath.TrimEnd('\\', '/');

            string outPath = regPath + ".patched.reg";

            // Регулярка для распознавания строк с ключами вида:
            // [HKEY_LOCAL_MACHINE\SOFTWARE\Sub\Key]
            // или
            // [-HKEY_LOCAL_MACHINE\SOFTWARE\Sub\Key]
            // Допускает сокращение HKLM и любые регистры.
            //
            // Группы:
            // 1: ведущий минус (если есть) без скобок, т.е. "-" или empty
            // 2: Hive (HKEY_LOCAL_MACHINE или HKLM)
            // 3: путь после hive (начинается с \...), включая \SOFTWARE...
            var keyLineRegex = new Regex(@"^\s*\[\s*(\-)?\s*(HKEY_LOCAL_MACHINE|HKLM)\s*(\\.+?)\s*\]\s*$",
                                         RegexOptions.IgnoreCase | RegexOptions.Compiled);

            bool insideAcceptedKey = false; // true, когда текущая секция - под HKLM\SOFTWARE и должна писаться
            bool seenAnyKey = false;        // флаг: встретился ли уже первый ключ (для логики сохранения комментариев до ключа)

            using (var reader = new StreamReader(regPath, Encoding.Unicode)) // .reg обычно в UTF-16 LE
            {
                // Иногда .reg бывают в ANSI/UTF8. Попробуем UTF-16; если прочтение упадёт, fallback:
                // Но StreamReader с Encoding.Unicode не бросит — пусть будет такое предупреждение.
            }

            // Попробуем открыть с autodetect: используем File.ReadAllText? Лучше построчно с автоопределением BOM:
            using (var fs = new FileStream(regPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var sr = new StreamReader(fs, true))
            using (var sw = new StreamWriter(outPath, false, new UTF8Encoding(false))) // пишем UTF-8 без BOM
            {
                string line;
                string pendingHeader = null;
                bool headerWritten = false;

                while ((line = sr.ReadLine()) != null)
                {
                    // Сначала просто захватим шапку файла (обычно "Windows Registry Editor Version 5.00")
                    if (!headerWritten)
                    {
                        // Если строка начинается с "[" - значит начался первый ключ
                        if (line.TrimStart().StartsWith("["))
                        {
                            headerWritten = true;
                            // Выписываем накопленную шапку (если была)
                            if (!string.IsNullOrEmpty(pendingHeader))
                            {
                                sw.WriteLine(pendingHeader);
                                pendingHeader = null;
                            }
                            // далее обработаем текущу строку как ключ (ниже)
                        }
                        else
                        {
                            // Сохраняем все строки до первого ключа (обычно комментарии/шапка)
                            // Но по заданию — нужно сохранить шапку. Поэтому пишем её прямо.
                            // Это сохраняет BOM/версию/комментарии до первого ключа.
                            sw.WriteLine(line);
                            continue;
                        }
                    }

                    // если мы здесь — либо header уже написан, либо эта строка сама была ключом
                    // проверим, является ли строка объявлением ключа:
                    var m = keyLineRegex.Match(line);
                    if (m.Success)
                    {
                        seenAnyKey = true;
                        string minus = m.Groups[1].Value; // "-" или empty
                        string hive = m.Groups[2].Value;  // HKLM or HKEY_LOCAL_MACHINE
                        string restPath = m.Groups[3].Value; // begins with backslash, e.g. \SOFTWARE\...\...

                        // Нормализуем слеши в restPath
                        restPath = restPath.Replace('/', '\\');

                        // Проверяем, начинается ли restPath с \SOFTWARE (независимо от регистра)
                        // или, реже, с \SOFTWARE\...
                        if (restPath.StartsWith("\\SOFTWARE", StringComparison.OrdinalIgnoreCase))
                        {
                            // Получаем остаток после "\SOFTWARE"
                            string remainder = restPath.Length > 9 ? restPath.Substring(9) : ""; // 9 == len("\SOFTWARE")
                                                                                                 // Склеиваем новый ключ: newRegPath + remainder
                                                                                                 // Убедимся, что между newRegPath и remainder ровно один '\'
                            string newKeyPath = newRegPath + (remainder.Length > 0 ? remainder : "");
                            // Запишем с сохранением минуса
                            if (!string.IsNullOrEmpty(minus))
                                sw.WriteLine("[-" + newKeyPath + "]");
                            else
                                sw.WriteLine("[" + newKeyPath + "]");
                            insideAcceptedKey = true;
                        }
                        else
                        {
                            // Ключ не в HKLM\SOFTWARE => отбрасываем его и все последующие строки до следующего ключа
                            insideAcceptedKey = false;
                            // НЕ пишем эту строку в выходной файл
                        }
                        continue;
                    }

                    // если не объявление ключа:
                    // - если внутри принятой секции — просто переписываем строку (сохранить значения)
                    // - иначе — игнорируем (отбрасываем строки, относящиеся к другим кустам)
                    if (insideAcceptedKey)
                    {
                        sw.WriteLine(line);
                    }
                    else
                    {
                        // not in accepted key -> drop line
                        // ничего не пишем
                    }
                } // while lines
            } // using streams

            // Готово. Результат в outPath
        }
    }
}
