using System.Data;
using Services.Data.DbContext;
using Services.Models;
using Services.Contracts;
using Microsoft.Extensions.Caching.Memory;


namespace Services.Implementations
{
    public class OrganizationService : IOrganizationService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMemoryCache _memoryCache;

        public OrganizationService(ApplicationDbContext dbContext, IMemoryCache memoryCache)
        {
            this.dbContext = dbContext ;
            _memoryCache = memoryCache;
        }

        public ICollection<OrganizationModel> GetData()
        {
            
            if (!_memoryCache.TryGetValue("DataCacheKey", out List<OrganizationModel> list))
            {
                
                list = new List<OrganizationModel>();

              
                DataTable companiesTable = dbContext.GetCompanies();
                int index = 1;
                foreach (DataRow row in companiesTable.Rows)
                {
                    list.Add(new OrganizationModel
                    {
                        Index = index,
                        OrganizationId = row["OrganizationId"].ToString(),
                        Name = row["Name"].ToString(),
                        Website = row["Website"].ToString(),
                        Country = row["Country"].ToString(),
                        Description = row["Description"].ToString(),
                        Founded = row["Founded"].ToString(),
                        Industry = row["Industry"].ToString(),
                        NumberOfEmployees = (int)row["NumberOfEmployees"]
                    });

                    index++;
                }

                
                DataTable countriesTable = dbContext.GetCountries();
                foreach (DataRow row in countriesTable.Rows)
                {
                    list.Add(new OrganizationModel
                    {
                        Index = index,
                        OrganizationId = row["OrganizationId"].ToString(),
                        Country = row["Country"].ToString(),
                        Name = row["Name"].ToString()
                    });

                    index++;
                }

             
                _memoryCache.Set("DataCacheKey", list, TimeSpan.FromMinutes(10));
            }

            return list;
        }

        public List<OrganizationModel> GetLargestCompanies()
        {
            
            if (!_memoryCache.TryGetValue("LargestCompaniesCacheKey", out List<OrganizationModel> largestCompanies))
            {
                
                largestCompanies = dbContext.GetCompanies().AsEnumerable()
                    .OrderByDescending(c => (int)c["NumberOfEmployees"])
                    .Take(10)
                    .Select(row => new OrganizationModel
                    {
                        OrganizationId = row["OrganizationId"].ToString(),
                        Name = row["Name"].ToString(),
                        Website = row["Website"].ToString(),
                        Country = row["Country"].ToString(),
                        Description = row["Description"].ToString(),
                        Founded = row["Founded"].ToString(),
                        Industry = row["Industry"].ToString(),
                        NumberOfEmployees = (int)row["NumberOfEmployees"]
                    })
                    .ToList();

              
                _memoryCache.Set("LargestCompaniesCacheKey", largestCompanies, TimeSpan.FromMinutes(10));
            }

            return largestCompanies;
        }

        public int GetTotalEmployeesByIndustry(string industry)
        {
            if (!_memoryCache.TryGetValue($"TotalEmployeesByIndustry_{industry}", out int totalEmployees))
            {
                var companies = dbContext.GetCompanies();

                totalEmployees = companies.AsEnumerable()
                    .Where(c => c["Industry"].ToString() == industry)
                    .Sum(c => (int)c["NumberOfEmployees"]);

                _memoryCache.Set($"TotalEmployeesByIndustry_{industry}", totalEmployees, TimeSpan.FromMinutes(10));
            }

            return totalEmployees;
        }
    }
}
