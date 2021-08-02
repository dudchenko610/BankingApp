using System;
using System.Collections.Generic;

namespace BankingApp.ViewModels.ViewModels.Deposit
{
    /// <summary>
    /// Used to represent deposit while queuering deposit by id.
    /// </summary>
    public class GetByIdDepositView
    {
        /// <summary>
        /// Deposit id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Formula for deposit calculation.
        /// </summary>
        public string CalculationFormula { get; set; }

        /// <summary>
        /// Sum of deposit.
        /// </summary>
        public decimal DepositSum { get; set; }

        /// <summary>
        /// Count of months.
        /// </summary>
        public int MonthsCount { get; set; }

        /// <summary>
        /// Deposit percent.
        /// </summary>
        public float Percents { get; set; }

        /// <summary>
        /// Time of deposit calculation.
        /// </summary>
        public DateTime CalсulationDateTime { get; set; }

        /// <summary>
        /// List of items with per month deposit data.
        /// </summary>
        public IList<MonthlyPaymentGetByIdDepositViewItem> MonthlyPaymentItems { get; set; }
    }

    /// <summary>
    /// View model used to represent monthlt payment.
    /// </summary>
    public class MonthlyPaymentGetByIdDepositViewItem
    {
        /// <summary>
        /// Number of the month.
        /// </summary>
        public int MonthNumber { get; set; }

        /// <summary>
        /// Deposit sum per of the month.
        /// </summary>
        public decimal TotalMonthSum { get; set; }

        /// <summary>
        /// Percent per the month.
        /// </summary>
        public float Percents { get; set; }
    }
}
