using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class FileReplace
    {

        public static void MoveCsvFiles(string sourceDirectory, string targetDirectory)
        {
            if (!Directory.Exists(targetDirectory)) // Проверка и създаване на целевата директория, ако тя не съществува
            {
                Directory.CreateDirectory(targetDirectory);
            }

            string[] csvFiles = Directory.GetFiles(sourceDirectory, "*.csv"); // Извличане на всички CSV файлове от изходната директория

            // Прохождане през всеки CSV файл и преместване в целевата директория
            foreach (string csvFile in csvFiles)
            {
                string fileName = Path.GetFileName(csvFile);
                string targetFilePath = Path.Combine(targetDirectory, fileName);

                // Променете го на true, ако искате да презаписвате съществуващи файлове
                bool overwrite = false;

                // Преместване на файловете
                File.Move(csvFile, targetFilePath, overwrite);

                Console.WriteLine($"The file {fileName} is succesfuly moved to {targetDirectory}.");
            }
        }

    }
}
