using BankingApp.ViewModels.Banking.Calculate;
using BankingApp.ViewModels.Banking.History;
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Interfaces
{
    public interface IDepositeService
    {
        public Task<int> CalculateDepositeAsync(CalculateDepositeBankingView reqDeposite);
        public Task<ResponseCalculationHistoryBankingView> GetCalculationDepositeHistoryAsync();
        public Task<ResponseCalculationHistoryDetailsBankingView> GetCalculationHistoryDetailsAsync(int depositeHistoryId);
    }
}
