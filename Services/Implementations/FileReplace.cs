
namespace Services.Implementations
{
    public class FileReplace
    {
       
        public static void MoveCsvFiles(string sourceDirectory, string targetDirectory)
        {
            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            string[] csvFiles = Directory.GetFiles(sourceDirectory, "*.csv");

            foreach (string csvFile in csvFiles)
            {
                string fileName = Path.GetFileName(csvFile);
                string targetFilePath = Path.Combine(targetDirectory, fileName);

                bool overwrite = false;
          
                File.Move(csvFile, targetFilePath, overwrite);

                Console.WriteLine($"The file {fileName} is succesfuly moved to {targetDirectory}.");
            }
        }
        
    }
}
