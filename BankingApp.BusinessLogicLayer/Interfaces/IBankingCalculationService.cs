using BankingApp.ViewModels.Banking;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    public interface IBankingCalculationService
    {
        ResponseCalculateDepositeBankingView CalculateDeposite(RequestCalculateDepositeBankingView reqDepositeCalcInfo);
    }
}
