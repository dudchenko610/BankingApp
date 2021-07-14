using BankingApp.ViewModels.Banking.Deposit;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    public interface IDepositService
    {
        Task<int> CalculateAsync(CalculateDepositView reqDepositeCalcInfo);
        Task<GetAllDepositView> GetAllAsync();
        Task<GetByIdDepositView> GetByIdAsync(int depositeHistoryId);
    }
}
