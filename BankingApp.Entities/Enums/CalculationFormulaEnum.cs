
namespace BankingApp.Entities.Enums
{
    /// <summary>
    /// Represents possible formulas for deposit calculation
    /// </summary>
    public enum CalculationFormulaEnum : int
    {
        /// <summary>
        /// Simple interest formula
        /// </summary>
        SimpleInterest = 0,

        /// <summary>
        /// Compound interset formula
        /// </summary>
        CompoundInterest = 1
    }
}
