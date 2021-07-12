using BankingApp.ViewModels.Banking.Calculate;
using BankingApp.ViewModels.Banking.History;
using BankingApp.ViewModels.Pagination;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    public interface IBankingService
    {
        Task<int> CalculateDepositeAsync(RequestCalculateDepositeBankingView reqDepositeCalcInfo);
        Task<int> SaveDepositeCalculationAsync(RequestCalculateDepositeBankingView reqDepositeCalcInfo,
            ResponseCalculateDepositeBankingView depositeCalculation);
        Task<ResponsePagedDataView<ResponseCalculationHistoryBankingViewItem>> GetDepositesCalculationHistoryAsync(int pageNumber, int pageSize);
        Task<ResponseCalculationHistoryDetailsBankingView> GetDepositeCalculationHistoryDetailsAsync(int depositeHistoryId);
    }
}
