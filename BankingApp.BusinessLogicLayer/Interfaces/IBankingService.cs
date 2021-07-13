using BankingApp.ViewModels.Banking.Calculate;
using BankingApp.ViewModels.Banking.History;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    public interface IBankingService
    {
        Task<int> CalculateDepositeAsync(CalculateDepositeBankingView reqDepositeCalcInfo);
        Task<ResponseCalculationHistoryBankingView> GetDepositesCalculationHistoryAsync();
        Task<ResponseCalculationHistoryDetailsBankingView> GetDepositeCalculationHistoryDetailsAsync(int depositeHistoryId);
    }
}
