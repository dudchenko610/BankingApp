using BankingApp.DataAccessLayer.DatabaseContexts;
using BankingApp.DataAccessLayer.Interfaces;
using BankingApp.DataAccessLayer.Models;
using BankingApp.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Repositories
{
    public class DepositRepository : GenericRepository<Deposit>, IDepositRepository
    {
        public DepositRepository(BankingDbContext dbContext) : base(dbContext)
        { 
        }

        public async Task<Deposit> GetDepositWithItemsByIdAsync(int depositId)
        {
            var depositeHistory = await _dbSet
                .Include(dep => dep.MonthlyPayments)
                .FirstOrDefaultAsync(dep => dep.Id == depositId);

            return depositeHistory;
        }

        public async Task<PagedDataView<Deposit>> GetAllAsync(int skip, int take, int userId)
        {
            var deposits = await _dbSet.Where(x => x.UserId == userId).Skip(skip).Take(take).ToListAsync();
            var paginationModel = new PagedDataView<Deposit>
            {
                Items = deposits,
                TotalCount = await _dbSet.CountAsync()
            };

            return paginationModel;
        }
    }
}
