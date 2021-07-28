using BankingApp.ViewModels.ViewModels.Deposit;
using BankingApp.ViewModels.ViewModels.Pagination;
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Interfaces
{
    public interface IDepositService
    {
        public Task<int> CalculateAsync(CalculateDepositView reqDeposite);
        public Task<PagedDataView<DepositGetAllDepositViewItem>> GetAllAsync(int pageNumber, int pageSize);
        public Task<GetByIdDepositView> GetByIdAsync(int depositeHistoryId);
    }
}
