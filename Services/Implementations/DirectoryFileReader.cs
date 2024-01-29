using CsvHelper.Configuration;
using CsvHelper;
using Services.Models;
using System.Globalization;
using Services.Contracts;

namespace Services.Implementations
{
    public class DirectoryFileReader : IFileReader
    {
        public List<List<OrganizationModel>> ReadDirectoryData(string dir, int batchSize)
        {
            List<List<OrganizationModel>> results = new List<List<OrganizationModel>>();

            foreach (var file in Directory.GetFiles(dir))
            {
                using (var reader = new StreamReader(file))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    HasHeaderRecord = true
                }))
                {
                    var records = csv.GetRecords<OrganizationModel>().ToList();

                    for (int i = 0; i < records.Count; i += batchSize)
                    {
                        var batch = records.Skip(i).Take(batchSize).ToList();
                        results.Add(batch);
                    }
                }
            }

            return results;
        }

    }
}

