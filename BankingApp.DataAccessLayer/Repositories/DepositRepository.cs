using BankingApp.DataAccessLayer.DatabaseContexts;
using BankingApp.DataAccessLayer.Interfaces;
using BankingApp.DataAccessLayer.Models;
using BankingApp.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Repositories
{
    /// <summary>
    /// Gives interface to work with data in "Deposits" table.
    /// </summary>
    public class DepositRepository : GenericRepository<Deposit>, IDepositRepository
    {
        /// <summary>
        /// Creates instance of <see cref="DepositRepository"/>.
        /// </summary>
        /// <param name="dbContext">Gives access to database.</param>
        public DepositRepository(BankingDbContext dbContext) : base(dbContext)
        { 
        }

        /// <summary>
        /// Gets <see cref="Deposit"/> with its <see cref="MonthlyPayment"/>s from database.
        /// </summary>
        /// <param name="depositId">Id of deposit.</param>
        /// <returns><see cref="Deposit"/> containing <see cref="MonthlyPayment"/>s.</returns>
        public async Task<Deposit> GetDepositWithItemsByIdAsync(int depositId)
        {
            var deposit = await _dbSet
                .Include(dep => dep.MonthlyPayments)
                .FirstOrDefaultAsync(dep => dep.Id == depositId);

            return deposit;
        }

        /// <summary>
        /// Gets paged deposits from database.
        /// </summary>
        /// <param name="skip">Offset from begining of the table.</param>
        /// <param name="take">Number of deposits to take.</param>
        /// <param name="userId">Id of user whose deposits should be returned.</param>
        /// <returns>Total deposits count and deposits for requested page.</returns>
        public async Task<PaginationModel<Deposit>> GetAllAsync(int skip, int take, int userId)
        {
            var deposits = await _dbSet.Where(x => x.UserId == userId).Skip(skip).Take(take).ToListAsync();
            var paginationModel = new PaginationModel<Deposit>
            {
                Items = deposits,
                TotalCount = await _dbSet.Where(x => x.UserId == userId).CountAsync()
            };

            return paginationModel;
        }
    }
}
