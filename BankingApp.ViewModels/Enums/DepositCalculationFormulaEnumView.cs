using System.ComponentModel.DataAnnotations;

namespace BankingApp.ViewModels.Enums
{
    /// <summary>
    /// Represents possible formulas for deposit calculation. Used as view model.
    /// </summary>
    public enum DepositCalculationFormulaEnumView : int
    {
        /// <summary>
        /// Simple interest formula.
        /// </summary>
        [Display(Name = "Simple Interest")]
        SimpleInterest = 0,

        /// <summary>
        /// Compound interset formula.
        /// </summary>
        [Display(Name = "Compound Interest")]
        CompoundInterest = 1
    }
}
