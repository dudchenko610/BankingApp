using BankingApp.Entities.Enums;
using System;
using System.Collections.Generic;

namespace BankingApp.Entities.Entities
{
    /// <summary>
    /// Represents entity of "Deposits" table.
    /// </summary> 
    public class Deposit : BaseEntity
    {
        /// <summary>
        /// Creates instance of <see cref="Deposit"/>. Initialized <see cref="MonthlyPayments" with empty list./>
        /// </summary>
        public Deposit()
        {
            MonthlyPayments = new List<MonthlyPayment>();
        }

        /// <summary>
        /// If of user that owns the deposit.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Reference to <see cref="BankingApp.Entities.Entities.User"/> class. Used for querying data.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Formula that was used while deposit calculation.
        /// </summary>
        public CalculationFormulaEnum CalculationFormula { get; set; }

        /// <summary>
        /// Deposit sum passed by user.
        /// </summary>
        public decimal DepositSum { get; set; }

        /// <summary>
        /// Number of mounts passed by user.
        /// </summary>
        public int MonthsCount { get; set; }

        /// <summary>
        /// Percent passed by user
        /// </summary>
        public float Percents { get; set; }

        /// <summary>
        /// Time of deposit calculation
        /// </summary>
        public DateTime CalсulationDateTime { get; set; }

        /// <summary>
        /// Calculated deposit per month
        /// </summary>
        public virtual IList<MonthlyPayment> MonthlyPayments { get; set; }
    }
}
