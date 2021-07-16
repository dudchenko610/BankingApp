
namespace BankingApp.Shared
{
    public static class Constants
    {
        public static class Errors
        {
            public static class Deposit
            {
                public const string IncorrectPriceFormat = "Price can't have more than 2 decimal places";
                public const string IncorrectMonthFormat = "Months number should be positive non-zero integer value";
                public const string IncorrectPercentNumber = "Percent should be positive fractional value in range of [1; 100] with two decimal places";

                public const string IncorrectDepositeHistoryId = "DepositeHistoryId should be integer, bigger than one number";
            }

            public static class Page
            {
                public const string IncorrectPageNumberFormat = "Page number should be integer, bigger than one number";
                public const string IncorrectPageSizeFormat = "Page size should be positive integer number, bigger than one number";
            }
        }

        public static class Routes
        {
            public static class Banking
            {
                public const string DepositRoute = "api/deposit";

                public const string Calculate = "calculateDeposit";
                public const string GetAll = "getAll";
                public const string GetById = "getById";
            }
        }

        public static class Page
        {
            public const int PageNumber = 1;
            public const int PageSizeElements = 2;
            public const string PagesNumberUrlParam = "pageNumber";
            public const string PageSizeUrlParam = "pageSize";
        }
    }
}
