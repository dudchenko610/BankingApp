using BankingApp.ViewModels.Banking;
using BankingApp.ViewModels.Banking.History;
using BankingApp.ViewModels.Pagination;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    public interface IBankingHistoryService
    {
        Task<int> SaveDepositeCalculationAsync(RequestCalculateDepositeBankingView reqDepositeCalcInfo, 
            ResponseCalculateDepositeBankingView depositeCalculation);
        Task<ResponsePagedDataView<ResponseCalculationHistoryBankingViewItem>> GetDepositesCalculationHistoryAsync(RequestPaginationFilterView requestPaginationModel);
        Task<ResponseCalculationHistoryBankingViewItem> GetDepositeCalculationHistoryDetailsAsync(int depositeHistoryId);
    }
}
