using BankingApp.ViewModels.Banking.Calculate;
using BankingApp.ViewModels.Banking.History;
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
