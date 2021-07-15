using BankingApp.DataAccessLayer.DatabaseContexts;
using BankingApp.DataAccessLayer.Interfaces;
using BankingApp.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Repositories
{
    public class DepositRepository : GenericRepository<Deposit>, IDepositRepository
    {
        public DepositRepository(BankingDbContext dbContext) : base(dbContext)
        { 
        }

        public async Task<Deposit> GetDepositWithItemsByIdAsync(int depositeHistoryId)
        {
            var depositeHistory = await _dbSet
                .Include(dep => dep.MonthlyPayments)
                .FirstOrDefaultAsync(dep => dep.Id == depositeHistoryId);
            return depositeHistory;
        }

        public async Task<(IList<Deposit> Deposits, int TotalCount)> GetDepositsPagedAsync(int skip, int take)
        {
            var deposits = await _dbSet.Skip(skip).Take(take).ToListAsync();
            return (deposits, await _dbSet.CountAsync());
        }
    }
}
