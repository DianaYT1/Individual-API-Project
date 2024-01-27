using CsvHelper;
using CsvHelper.Configuration;
using Services.Contracts;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using CsvHelper.TypeConversion;
using System.IO;
using System.Linq;

namespace Services.Implementations
{
    public class DirectoryFileReader : IFileReader
    {
        public ICollection<OrganizationModel> ReadDirectoryData(string dir)
        {
            List<OrganizationModel> results = new List<OrganizationModel>();
            foreach (var file in Directory.GetFiles(dir))
            {
                using (var reader = new StreamReader(file))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    HasHeaderRecord = true  // Указва, че първият ред трябва да се счита за хедър
                }))
                {
                    results.AddRange(csv.GetRecords<OrganizationModel>());
                }
            }
            return results;
        }
    }
}
