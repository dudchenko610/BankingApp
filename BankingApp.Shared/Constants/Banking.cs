
namespace BankingApp.Shared
{
    public static partial class Constants
    {
        public static partial class Errors
        {
            public static class Banking
            {
                public const string IncorrectPriceFormat = "Price can't have more than 2 decimal places";
                public const string IncorrectMonthFormat = "Months number should be positive non-zero integer value";
                public const string IncorrectPercentNumber = "Percent should be positive integer value in range of [1; 100]";
            }

            public static class Page
            {
                public const string IncorrectPageNumberFormat = "Page number should be integer, bigger than one number";
                public const string IncorrectPageSizeFormat = "Page size should be positive integer number";
            }
        }

        public static partial class Routes
        {
            public static class Banking
            {
                public const string BankingRoute = "api/banking";

                public const string CalculateDeposite = "calculateDeposite";
                public const string CalculationHistory = "calculationHistory";
                public const string CalculationHistoryDetails = "calculationHistoryDetails";
            }
        }

        public static partial class Page
        {
            public const int PageNumber = 1;
            public const int PageSizeElements = 2;
            public const string PagesNumberUrlParam = "pageNumber";
            public const string PageSizeUrlParam = "pageSize";
        }
    }
}
