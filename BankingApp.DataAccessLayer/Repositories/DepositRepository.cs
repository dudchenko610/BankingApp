using BankingApp.DataAccessLayer.DatabaseContexts;
using BankingApp.DataAccessLayer.Interfaces;
using BankingApp.DataAccessLayer.Models;
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

        public async Task<Deposit> GetDepositWithItemsByIdAsync(int depositId)
        {
            var deposit = await _dbSet
                .Include(dep => dep.MonthlyPayments)
                .FirstOrDefaultAsync(dep => dep.Id == depositId);

            return deposit;
        }

        public async Task<PaginationModel<Deposit>> GetAllAsync(int skip, int take, int userId)
        {
            var deposits = await _dbSet.Where(x => x.UserId == userId).Skip(skip).Take(take).ToListAsync();
            var paginationModel = new PaginationModel<Deposit>
            {
                Items = deposits,
                TotalCount = await _dbSet.CountAsync()
            };

            return paginationModel;
        }
    }
}
