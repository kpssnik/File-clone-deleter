using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileCloneDeleter
{
    class Program
    {
        static List<string> fileNames = new List<string>();
        static void Main(string[] args)
        {
            UpdateFileNames();

            Console.WriteLine("Удалять файлы с названием 'копия'?");
            Console.WriteLine("1-Да\tЛюбой другой символ-Нет");

            if (Console.ReadLine() == "1")
            {
                #region copynameDelete
                Console.WriteLine("Удаляем фотографии с названием копия...");
                foreach (var file in fileNames)
                {
                    if (file.Contains("копия")) File.Delete(file);
                }
                Console.WriteLine("Удалил фотографии с названием копия...");
                Console.Beep();
                #endregion
            }

            UpdateFileNames();
            List<string> confirmed = new List<string>();

            Console.WriteLine("Удаляем фото по составу");
            while (true)
            {
                UpdateFileNames();
                try
                {
                    foreach (var file in fileNames)
                    {
                        string originalName = file;
                        if (confirmed.Contains(originalName)) continue;
                        Console.WriteLine("Сравниваем " + file + " с остальными");
                        string originalCode = string.Empty;
                        string copyCode = string.Empty;

                        using (StreamReader sr = new StreamReader(originalName)) originalCode = sr.ReadToEnd();
                        foreach (var copy in fileNames)
                        {
                            try
                            {
                                if (!copy.Equals(originalName))
                                {
                                    using (StreamReader sr = new StreamReader(copy)) copyCode = sr.ReadToEnd();

                                    if (originalCode == copyCode)
                                    {
                                        File.Delete(copy);
                                        PrintMessage("Удалил " + copy, ConsoleColor.Green);
                                        UpdateFileNames();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(ex.Message);
                                Console.ResetColor();
                                continue;
                            }
                        }
                        confirmed.Add(originalName);
                        UpdateFileNames();
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                    continue;
                }
            }

        }

        static void UpdateFileNames()
        {
            fileNames = Directory.GetFiles(Directory.GetCurrentDirectory()).ToList();
            fileNames.Sort();
        }

        static void PrintMessage(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ResetColor();
        }


    }
}
