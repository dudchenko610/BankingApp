using BankingApp.Shared;
using BankingApp.ViewModels.Enums;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.ViewModels.ViewModels.Deposit
{
    /// <summary>
    /// View model used to pass deposit data for calculation.
    /// </summary>
    public class CalculateDepositView
    {
        /// <summary>
        /// Enum representing formula for deposit calculation.
        /// </summary>
        public DepositCalculationFormulaEnumView CalculationFormula { get; set; }

        /// <summary>
        /// Sum of deposit.
        /// </summary>
        [Required]
        [RegularExpression(@"\A[0-9]{1,10}(?:[.,][0-9]{1,2})?\z", ErrorMessage = Constants.Errors.Deposit.IncorrectPriceFormat)]
        public decimal DepositSum { get; set; }

        /// <summary>
        /// Number of months.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = Constants.Errors.Deposit.IncorrectMonthFormat)]
        public int MonthsCount { get; set; }

        /// <summary>
        /// Percents.
        /// </summary>
        [Required]
        [RegularExpression(@"\A[0-9]{1,10}(?:[.,][0-9]{1,2})?\z", ErrorMessage = Constants.Errors.Deposit.IncorrectPercentNumber)]
        [Range(1.0f, 100.0f, ErrorMessage = Constants.Errors.Deposit.IncorrectPercentNumber)]
        public decimal Percents { get; set; }
    }
}
