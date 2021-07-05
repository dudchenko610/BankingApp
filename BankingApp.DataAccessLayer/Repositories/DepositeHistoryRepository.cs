using BankingApp.DataAccessLayer.Core;
using BankingApp.DataAccessLayer.DatabaseContexts;
using BankingApp.DataAccessLayer.Entities;

namespace BankingApp.DataAccessLayer.Repositories
{
    public class DepositeHistoryRepository : GenericRepository<DepositeHistory>
    {
        public DepositeHistoryRepository(BankingDbContext dbContext) : base(dbContext)
        { 
        }
    }
}
