using BankingApp.ViewModels.Banking;
using BankingApp.ViewModels.Banking.History;
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Interfaces
{
    public interface IDepositeService
    {
        public Task<int> CalculateDepositeAsync(RequestCalculateDepositeBankingView reqDeposite);
        public Task<ResponseCalculationHistoryBankingView> GetCalculationDepositeHistoryAsync();
        public Task<ResponseCalculationHistoryBankingViewItem> GetCalculationHistoryDetailsAsync(int depositeHistoryId);
    }
}
