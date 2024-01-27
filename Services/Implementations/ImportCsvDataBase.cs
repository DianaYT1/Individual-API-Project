//using Services.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Services.Implementations
//{
//    public class ImportCsvDataBase
//    {
//        private readonly ApplicationDbContext dbContext;

//        public CsvDatabaseService(ApplicationDbContext dbContext)
//        {
//            this.dbContext = dbContext;
        
//        public async Task ImportCsvDataAsync(List<OrganizationModel> organizationModels)
//        {
//            using (var transaction = await dbContext.Database.BeginTransactionAsync())
//            {
//                try
//                {
//                    await dbContext.BulkInsertAsync(csvDataDtoList);

//                    transaction.Commit();
//                }
//                catch (Exception)
//                {
//                    transaction.Rollback();
//                    throw;
//                }
//            }
//        }
//    }
//}
