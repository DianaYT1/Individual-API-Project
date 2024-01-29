using Services.Models;

namespace Services.Contracts
{
    public interface IOrganizationService
    {
        public ICollection<OrganizationModel> GetData();
        public List<OrganizationModel> GetLargestCompanies();

        public int GetTotalEmployeesByIndustry(string industry);

    }
}
