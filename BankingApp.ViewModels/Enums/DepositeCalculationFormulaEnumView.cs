using System.ComponentModel.DataAnnotations;

namespace BankingApp.ViewModels.Enums
{
    public enum DepositeCalculationFormulaEnumView
    {
        [Display(Name = "Simple Interest")]
        SimpleInterest = 0,

        [Display(Name = "Compound Interest")]
        CompoundInterest = 1
    }
}
