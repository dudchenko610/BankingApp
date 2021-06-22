using Shared.ViewModels.Banking;

namespace BusinessLogicLayer.Facade
{
    public interface IBankingService
    {
        DepositeOutputData CalculateDeposite(DepositeInputData depositeInputData);
    }
}
