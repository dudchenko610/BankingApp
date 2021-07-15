using System.ComponentModel.DataAnnotations;

namespace BankingApp.ViewModels.Enums
{
    public enum DepositCalculationFormulaEnumView : int
    {
        [Display(Name = "Simple Interest")]
        SimpleInterest = 0,

        [Display(Name = "Compound Interest")]
        CompoundInterest = 1
    }
}
