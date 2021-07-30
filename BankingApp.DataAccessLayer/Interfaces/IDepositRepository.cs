using BankingApp.DataAccessLayer.Models;
using BankingApp.Entities.Entities;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Interfaces
{
    /// <summary>
    /// Gives interface to work with data in "Deposits" table.
    /// </summary>
    public interface IDepositRepository : IGenericRepository<Deposit>
    {

        /// <summary>
        /// Gets <see cref="Deposit"/> with its <see cref="MonthlyPayment"/>s.
        /// </summary>
        /// <param name="depositId">Id of deposit.</param>
        /// <returns><see cref="Deposit"/> containing <see cref="MonthlyPayment"/>s.</returns>
        Task<Deposit> GetDepositWithItemsByIdAsync(int depositId);

        /// <summary>
        /// Gets paged deposits.
        /// </summary>
        /// <param name="skip">Offset from begining of the table.</param>
        /// <param name="take">Number of deposits to take.</param>
        /// <param name="userId">Id of user whose deposits should be returned.</param>
        /// <returns>Total deposits count and deposits for requested page.</returns>
        Task<PagedDataView<Deposit>> GetAllAsync(int skip, int take, int userId);
    }
}
