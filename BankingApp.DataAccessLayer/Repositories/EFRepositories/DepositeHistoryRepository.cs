using BankingApp.DataAccessLayer.DatabaseContexts;
using BankingApp.DataAccessLayer.Repositories.EFRepositories;
using BankingApp.DataAccessLayer.Repositories.Interfaces;
using BankingApp.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Repositories
{
    public class DepositeHistoryRepository : GenericRepository<DepositeHistory>, IDepositeHistoryRepository
    {
        public DepositeHistoryRepository(BankingDbContext dbContext) : base(dbContext)
        { 
        }

        public async Task<IList<DepositeHistory>> GetDepositesHistoryWithItemsAsync()
        {
            var depositesHistory = await _dbSet.Include(dh => dh.DepositeHistoryItems).ToListAsync();
            return depositesHistory;
        }
    }
}
