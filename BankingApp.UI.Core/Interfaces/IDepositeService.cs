using BankingApp.ViewModels.Banking;
using BankingApp.ViewModels.Banking.History;
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Interfaces
{
    public interface IDepositeService
    {
        public Task<ResponseCalculateDepositeBankingView> CalculateDepositeAsync(RequestCalculateDepositeBankingView reqDeposite);
        public Task<ResponseCalculationHistoryBankingView> GetCalculationDepositeHistoryAsync();
    }
}
