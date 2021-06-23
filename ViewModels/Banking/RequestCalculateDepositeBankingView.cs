
namespace ViewModels.Banking
{
    public class RequestCalculateDepositeBankingView
    {
        //  [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = Constants.Errors.Banking.INCORRECT_PRICE_FORMAT)]
        public decimal DepositeSum { get; set; }

        //   [Range(1, int.MaxValue, ErrorMessage = Constants.Errors.Banking.INCORRECT_MONTH_NUMBER)]
        public int MonthsCount { get; set; }

        //  [Range(1, 100, ErrorMessage = Constants.Errors.Banking.INCORRECT_PERECENT_NUMBER)]
        public int Percents { get; set; }
    }
}
