﻿
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
    }
}
