using BankingApp.ViewModels.Banking.Calculate;
using BankingApp.ViewModels.Banking.History;
using BankingApp.ViewModels.Pagination;
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Interfaces
{
    public interface IDepositeService
    {
        public Task<int> CalculateDepositeAsync(RequestCalculateDepositeBankingView reqDeposite);
        public Task<ResponsePagedDataView<ResponseCalculationHistoryBankingViewItem>> GetCalculationDepositeHistoryAsync(int pageNumber, int pageSize);
        public Task<ResponseCalculationHistoryDetailsBankingView> GetCalculationHistoryDetailsAsync(int depositeHistoryId);
    }
}
