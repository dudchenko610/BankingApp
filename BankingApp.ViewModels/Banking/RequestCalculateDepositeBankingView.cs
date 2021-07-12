using BankingApp.Shared;
using BankingApp.ViewModels.Enums;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.ViewModels.Banking
{
    public class RequestCalculateDepositeBankingView
    {
        public DepositeCalculationFormulaEnumView CalculationFormula { get; set; }

        [Required]
        [RegularExpression(@"\A[0-9]{1,10}(?:[.,][0-9]{1,2})?\z", ErrorMessage = Constants.Errors.Banking.IncorrectPriceFormat)]
        public decimal DepositeSum { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = Constants.Errors.Banking.IncorrectMonthFormat)]
        public int MonthsCount { get; set; }

        [Required]
        [RegularExpression(@"\A[0-9]{1,10}(?:[.,][0-9]{1,2})?\z", ErrorMessage = Constants.Errors.Banking.IncorrectPercentNumber)]
        [Range(1.0f, 100.0f, ErrorMessage = Constants.Errors.Banking.IncorrectPercentNumber)]
        public decimal Percents { get; set; }
    }
}
