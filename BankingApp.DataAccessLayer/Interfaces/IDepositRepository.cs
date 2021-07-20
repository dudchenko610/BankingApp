using BankingApp.DataAccessLayer.Models;
using BankingApp.Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Interfaces
{
    public interface IDepositRepository : IGenericRepository<Deposit>
    {
        Task<Deposit> GetDepositWithItemsByIdAsync(int depositId);
        Task<PaginationModel<Deposit>> GetAllAsync(int skip, int take, int userId);
    }
}
