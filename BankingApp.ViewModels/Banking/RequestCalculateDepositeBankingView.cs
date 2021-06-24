
using BankingApp.Shared;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.ViewModels.Banking
{
    public class RequestCalculateDepositeBankingView
    {
        [Required]
        [RegularExpression(@"\A[0-9]{1,10}(?:[.,][0-9]{1,2})?\z", ErrorMessage = Constants.Errors.Banking.INCORRECT_PRICE_FORMAT)]
        public decimal DepositeSum { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = Constants.Errors.Banking.INCORRECT_MONTH_NUMBER)]
        public int MonthsCount { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = Constants.Errors.Banking.INCORRECT_PERECENT_NUMBER)]
        public int Percents { get; set; }
    }
}
