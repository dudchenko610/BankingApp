using BankingApp.DataAccessLayer.DatabaseContexts;
using BankingApp.DataAccessLayer.Entities;
using BankingApp.DataAccessLayer.Repositories.EFRepositories;
using BankingApp.DataAccessLayer.Repositories.Interfaces;

namespace BankingApp.DataAccessLayer.Repositories
{
    public class DepositeHistoryRepository : GenericRepository<DepositeHistory>, IDepositeHistoryRepository
    {
        public DepositeHistoryRepository(BankingDbContext dbContext) : base(dbContext)
        { 
        }
    }
}
