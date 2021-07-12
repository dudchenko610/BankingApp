using BankingApp.ViewModels.Banking.Calculate;
using BankingApp.ViewModels.Banking.History;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    public interface IBankingService
    {
        Task<int> CalculateDepositeAsync(RequestCalculateDepositeBankingView reqDepositeCalcInfo);
        Task<int> SaveDepositeCalculationAsync(RequestCalculateDepositeBankingView reqDepositeCalcInfo,
            ResponseCalculateDepositeBankingView depositeCalculation);
        Task<ResponseCalculationHistoryBankingView> GetDepositesCalculationHistoryAsync();
        Task<ResponseCalculationHistoryBankingViewItem> GetDepositeCalculationHistoryDetailsAsync(int depositeHistoryId);
    }
}
