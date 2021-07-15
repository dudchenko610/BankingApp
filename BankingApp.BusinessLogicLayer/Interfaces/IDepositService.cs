using BankingApp.ViewModels.Banking.Deposit;
using BankingApp.ViewModels.Pagination;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    public interface IDepositService
    {
        Task<int> CalculateAsync(CalculateDepositView reqDepositeCalcInfo);
        Task<PagedDataView<DepositGetAllDepositViewItem>> GetAllAsync(int pageNumber, int pageSize);
        Task<GetByIdDepositView> GetByIdAsync(int depositeHistoryId);
    }
}
