// See https://aka.ms/new-console-template for more information
using Microsoft.Data.SqlClient;
using Services.Data.DbContext;
using Services.Implementations;
using Services.Models;
using System.Data;
using System.Diagnostics;
using System.Linq;

Console.WriteLine("The data from the csv file is being processed");

string fromDirectoryFile = "C:\\Files";
string toDirectoryFile = "C:\\Read";

string connectionString = "Server=DESKTOP-HCQHH6R\\SQLEXPRESS;Database=myDataBase;Trusted_Connection=True;Encrypt=False;";
Stopwatch stopwatch = Stopwatch.StartNew();

using (var dbContext = new ApplicationDbContext(connectionString))
{
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.Open();

        // Batch size for processing
        int batchSize = 100;
        var data = new DirectoryFileReader().ReadDirectoryData(fromDirectoryFile);

        for (int i = 0; i < data.Count; i += batchSize)
        {
            var batch = data.Skip(i).Take(batchSize).ToList();
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
    }

    var companies = dbContext.GetCompanies();
    var countries = dbContext.GetCountries();

    string scriptPath = "C:\\Users\\John\\Desktop\\New folder (2)\\Project\\Services\\SqlScripts\\OrganizationDB.sql";
    string script = File.ReadAllText(scriptPath);
    //ExecuteScript(connectionString, script);

    FileReplace.MoveCsvFiles(fromDirectoryFile, toDirectoryFile);
    stopwatch.Stop();
    Console.WriteLine($"Time taken: {stopwatch.ElapsedMilliseconds} ms");
}



