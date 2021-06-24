using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApp.Shared
{
    public static partial class Constants
    {
        public static partial class Errors
        {
            public static class Banking
            {
                public const string INCORRECT_PRICE_FORMAT = "Price can't have more than 2 decimal places";
                public const string INCORRECT_MONTH_NUMBER = "Months number should be positive non-zero integer value";
                public const string INCORRECT_PERECENT_NUMBER = "Months number should be positive integer value in range of [1; 100]";
            }
        }

        public static partial class Routes
        {
            public static class Banking
            {
                public const string CALCULATE_DEPOSITE = "calculateDeposite";
            }
        }
    }
}
