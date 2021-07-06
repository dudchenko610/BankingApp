using BankingApp.Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Repositories.Interfaces
{
    public interface IDepositeHistoryRepository : IGenericRepository<DepositeHistory>
    {
        Task<IList<DepositeHistory>> GetDepositesHistoryWithItemsAsync();
    }
}
