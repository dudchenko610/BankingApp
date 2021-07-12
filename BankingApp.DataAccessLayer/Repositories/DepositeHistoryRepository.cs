using BankingApp.DataAccessLayer.DatabaseContexts;
using BankingApp.DataAccessLayer.Interfaces;
using BankingApp.DataAccessLayer.Repositories.EFRepositories;
using BankingApp.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Repositories
{
    public class DepositeHistoryRepository : GenericRepository<DepositeHistory>, IDepositeHistoryRepository
    {
        public DepositeHistoryRepository(BankingDbContext dbContext) : base(dbContext)
        { 
        }

        public async Task<DepositeHistory> GetDepositeHistoryWithItemsAsync(int depositeHistoryId)
        {
            var depositeHistory = await _dbSet
                .Include(dep => dep.DepositeHistoryItems)
                .FirstOrDefaultAsync(dep => dep.Id == depositeHistoryId);
            return depositeHistory;
        }

        public async Task<(IList<DepositeHistory> DepositeHistory, int TotalCount)> GetDepositesHistoryPagedAsync(int skip, int take)
        {
            var wholeList = await _dbSet.ToListAsync();
            var depositesHistory = wholeList.Skip(skip).Take(take).ToList();
            return (depositesHistory, wholeList.Count);
        }
    }
}
