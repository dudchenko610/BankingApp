using BankingApp.Shared;
using BankingApp.ViewModels.Enums;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.ViewModels.ViewModels.Deposit
{
    public class CalculateDepositView
    {
        public DepositCalculationFormulaEnumView CalculationFormula { get; set; }

        [Required]
        [RegularExpression(@"\A[0-9]{1,10}(?:[.,][0-9]{1,2})?\z", ErrorMessage = Constants.Errors.Deposit.IncorrectPriceFormat)]
        public decimal DepositSum { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = Constants.Errors.Deposit.IncorrectMonthFormat)]
        public int MonthsCount { get; set; }

        [Required]
        [RegularExpression(@"\A[0-9]{1,10}(?:[.,][0-9]{1,2})?\z", ErrorMessage = Constants.Errors.Deposit.IncorrectPercentNumber)]
        [Range(1.0f, 100.0f, ErrorMessage = Constants.Errors.Deposit.IncorrectPercentNumber)]
        public decimal Percents { get; set; }
    }
}
