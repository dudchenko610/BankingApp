using BankingApp.Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Repositories.Interfaces
{
    public interface IDepositeHistoryRepository : IGenericRepository<DepositeHistory>
    {
        Task<DepositeHistory> GetDepositeHistoryWithItemsAsync(int depositeHistoryId);
        Task<(IList<DepositeHistory> DepositeHistory, int TotalCount)> GetDepositesHistoryPagedAsync(int skip, int take);
    }
}
