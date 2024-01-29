
using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Services.Models

{
    public class OrganizationModel
    {
        public int Index { get; set; }

        [Name("Organization Id")]
        public string OrganizationId { get; set; }

        public string Name { get; set; }

        public string Website { get; set; }

        [StringLength(255)]
        public string Country { get; set; }

        public string Description { get; set; }

        public string Founded { get; set; }

        [StringLength(255)]
        public string Industry { get; set; }

        [Name("Number of employees")]
        public int NumberOfEmployees { get; set; }
    }

}
