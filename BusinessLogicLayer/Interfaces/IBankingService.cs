using SharedViewModels.Banking;

namespace BusinessLogicLayer.Interfaces
{
    public interface IBankingService
    {
        ResponseCalculateDepositeBankingView CalculateDeposite(RequestCalculateDepositeBankingView reqDepositeCalcInfo);
    }
}
