using BankingApp.ViewModels.Banking;
using BankingApp.ViewModels.Banking.History;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    public interface IBankingService
    {
        ResponseCalculateDepositeBankingView CalculateDeposite(RequestCalculateDepositeBankingView reqDepositeCalcInfo);
        Task<int> SaveDepositeCalculationAsync(RequestCalculateDepositeBankingView reqDepositeCalcInfo,
            ResponseCalculateDepositeBankingView depositeCalculation);
        Task<ResponseCalculationHistoryBankingView> GetDepositesCalculationHistoryAsync();
        Task<ResponseCalculationHistoryBankingViewItem> GetDepositeCalculationHistoryDetailsAsync(int depositeHistoryId);
    }
}
