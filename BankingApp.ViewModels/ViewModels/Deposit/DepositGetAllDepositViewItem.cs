using System;

namespace BankingApp.ViewModels.ViewModels.Deposit
{
    /// <summary>
    /// View model used to represent deposit while queuering all deposits.
    /// </summary>
    public class DepositGetAllDepositViewItem
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
    }
}
