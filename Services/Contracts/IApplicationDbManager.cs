

namespace Services.Contracts
{
    public interface IApplicationDbManager
    {
            void EnsureDatabaseTables(string connectionString);
        
    }
}
