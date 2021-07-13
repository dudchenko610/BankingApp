using BankingApp.DataAccessLayer.DatabaseContexts;
using BankingApp.DataAccessLayer.Interfaces;
using BankingApp.Entities.Entities;
using Microsoft.EntityFrameworkCore;
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
    }
}
