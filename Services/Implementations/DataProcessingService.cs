using Microsoft.Data.SqlClient;
using System.Diagnostics;
using Services.Data.DbContext;
using Services.Implementations;

public class DataProcessingService
{
    public void ProcessData()
    {
        string fromDirectoryFile = "C:\\Files";
        string toDirectoryFile = "C:\\Read";

        string connectionString = "Server=DESKTOP-HCQHH6R\\SQLEXPRESS;Database=myDataBase;Trusted_Connection=True;Encrypt=False;";
        Stopwatch stopwatch = Stopwatch.StartNew();

        using (var dbContext = new ApplicationDbContext(connectionString))
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                int batchSize = 1000;
                var dataBatches = new DirectoryFileReader().ReadDirectoryData(fromDirectoryFile, batchSize);

                foreach (var batch in dataBatches)
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            dbContext.ProcessBatch(batch, connection, transaction);
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }

                var companies = dbContext.GetCompanies();
                var countries = dbContext.GetCountries();

                string scriptPath = "C:\\Users\\John\\Desktop\\New folder (2)\\Project\\Services\\SqlScripts\\OrganizationDB.sql";
                string script = File.ReadAllText(scriptPath);

                FileReplace.MoveCsvFiles(fromDirectoryFile, toDirectoryFile);
                stopwatch.Stop();
                Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds} ms");
            }
        }
    }
}
