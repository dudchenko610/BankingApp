using BankingApp.ViewModels.Banking;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    public interface IBankingService
    {
        ResponseCalculateDepositeBankingView CalculateDepositeSimpleInterest(RequestCalculateDepositeBankingView reqDepositeCalcInfo);
        ResponseCalculateDepositeBankingView CalculateDepositeCompoundInterest(RequestCalculateDepositeBankingView reqDepositeCalcInfo);
    }
}
