using Services.Models;

namespace Services.Contracts
{
    public interface IFileReader
    {
        public List<List<OrganizationModel>> ReadDirectoryData(string dir, int batchSize);
    }
}
