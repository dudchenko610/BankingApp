using BankingApp.ViewModels.Banking.Deposit;
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Interfaces
{
    public interface IDepositService
    {
        public Task<int> CalculateAsync(CalculateDepositView reqDeposite);
        public Task<GetAllDepositView> GetAllAsync();
        public Task<GetByIdDepositView> GetByIdAsync(int depositeHistoryId);
    }
}
