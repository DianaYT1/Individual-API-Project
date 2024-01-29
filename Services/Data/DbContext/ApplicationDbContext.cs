using Microsoft.Data.SqlClient;
using System.Data;
using Services.Models;
using Services.Data.DbManager;

namespace Services.Data.DbContext
{
    public class ApplicationDbContext : IDisposable
    {
        string connectionString;

        private SqlConnection connection;
        private readonly ApplicationDbManager applicationDbManager;
        public ApplicationDbContext(string connectionString)
        {
            this.connectionString = connectionString;
            connection = new SqlConnection(connectionString);
            connection.Open();

            applicationDbManager = new ApplicationDbManager();
            applicationDbManager.EnsureDatabaseTables(connectionString);
        }

        public DataTable GetCompanies()
        {
            string sql = "SELECT * FROM Company";
            return ExecuteQuery(sql);
        }

        public DataTable GetCountries()
        {
            string sql = "SELECT * FROM Country";
            return ExecuteQuery(sql);
        }


        public void AddCompany(OrganizationModel model, SqlConnection connection, SqlTransaction transaction)
        {
            string checkIfExistsSql = "SELECT COUNT(*) FROM Company WHERE OrganizationId = @OrganizationId";
            int existingRecordsCount;

            using (SqlCommand command = new SqlCommand(checkIfExistsSql, connection, transaction))
            {
                command.Parameters.AddWithValue("@OrganizationId", model.OrganizationId);
                existingRecordsCount = Convert.ToInt32(command.ExecuteScalar());
            }

            if (existingRecordsCount > 0)
            {
               
                string updateSql = "UPDATE Company SET " +
                          "Name = @Name, " +
                          "Website = @Website, " +
                          "Country = @Country, " +
                          "Description = @Description, " +
                          "Founded = @Founded, " +
                          "Industry = @Industry, " +
                          "NumberOfEmployees = @NumberOfEmployees " +
                          "WHERE OrganizationId = @OrganizationId";

                using (SqlCommand command = new SqlCommand(updateSql, connection, transaction))
                {
                    SetParameters(command, model);
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                
                string insertSql = "INSERT INTO Company (OrganizationId, Name, Website, Country, Description, Founded, Industry, NumberOfEmployees) " +
                                   "VALUES (@OrganizationId, @Name, @Website, @Country, @Description, @Founded, @Industry, @NumberOfEmployees)";

                using (SqlCommand command = new SqlCommand(insertSql, connection, transaction))
                {
                    SetParameters(command, model);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void SetParameters(SqlCommand command, OrganizationModel model)
        {
            command.Parameters.AddWithValue("@OrganizationId", model.OrganizationId);
            command.Parameters.AddWithValue("@Name", EscapeSingleQuotes(model.Name));
            command.Parameters.AddWithValue("@Website", EscapeSingleQuotes(model.Website));
            command.Parameters.AddWithValue("@Country", EscapeSingleQuotes(model.Country));
            command.Parameters.AddWithValue("@Description", EscapeSingleQuotes(model.Description));
            command.Parameters.AddWithValue("@Founded", model.Founded);
            command.Parameters.AddWithValue("@Industry", EscapeSingleQuotes(model.Industry));
            command.Parameters.AddWithValue("@NumberOfEmployees", model.NumberOfEmployees);
        }

        private string EscapeSingleQuotes(string input)
        {
           
            return input.Replace("'", "''");
        }



        public void AddCountry(OrganizationModel model, SqlConnection connection, SqlTransaction transaction)
        {
            string checkIfExistsSql = "SELECT COUNT(*) FROM Country WHERE OrganizationId = @OrganizationId";
            int existingRecordsCount;

            using (SqlCommand command = new SqlCommand(checkIfExistsSql, connection, transaction))
            {
                command.Parameters.AddWithValue("@OrganizationId", model.OrganizationId);
                existingRecordsCount = Convert.ToInt32(command.ExecuteScalar());
            }

            if (existingRecordsCount > 0)
            {
               
                string updateSql = "UPDATE Country SET Country = @Country, Name = @Name WHERE OrganizationId = @OrganizationId";

                using (SqlCommand command = new SqlCommand(updateSql, connection, transaction))
                {
                    SetCountryParameters(command, model);
                    command.ExecuteNonQuery();
                }
            }
            else
            {

                string insertSql = "INSERT INTO Country (OrganizationId, Country, Name) VALUES (@OrganizationId, @Country, @Name)";

                using (SqlCommand command = new SqlCommand(insertSql, connection, transaction))
                {
                    SetCountryParameters(command, model);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void SetCountryParameters(SqlCommand command, OrganizationModel model)
        {
            command.Parameters.AddWithValue("@OrganizationId", model.OrganizationId);
            command.Parameters.AddWithValue("@Country", EscapeSingleQuotes(model.Country));
            command.Parameters.AddWithValue("@Name", EscapeSingleQuotes(model.Name));
        }
       public void ProcessBatch(List<OrganizationModel> batch, SqlConnection connection, SqlTransaction transaction)
        {
            batch.ForEach(organization =>
            {
                AddCompany(organization, connection, transaction);
                AddCountry(organization, connection, transaction);
            });
        }


        public void Dispose()
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
            }
        }

        private DataTable ExecuteQuery(string sql)
        {
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    return dataTable;
                }
            }
        }
    }
}

