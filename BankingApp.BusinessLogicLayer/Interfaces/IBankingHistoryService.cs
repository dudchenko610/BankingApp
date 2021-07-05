using BankingApp.ViewModels.Banking;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    public interface IBankingHistoryService
    {
        Task SaveDepositeCalculationAsync(ResponseCalculateDepositeBankingView depositeCalculation);
    }
}
