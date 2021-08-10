using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApp.Entities.Entities
{
    /// <summary>
    /// Represents entity of "MonthlyPayments" table.
    /// </summary> 
    public class MonthlyPayment : BaseEntity
    {
        /// <summary>
        /// Id of deposit to which belongs that monthly payment.
        /// </summary>
        [ForeignKey("DepositeHistory")]
        public int DepositId { get; set; }

        /// <summary>
        /// Reference to <see cref="BankingApp.Entities.Entities.Deposit"/> class. Used for querying data.
        /// </summary>
        public virtual Deposit Deposit { get; set; }

        /// <summary>
        /// Number of month.
        /// </summary>
        public int MonthNumber { get; set; }

        /// <summary>
        /// Calculated sum of deposit for this month.
        /// </summary>
        public decimal TotalMonthSum { get; set; }

        /// <summary>
        /// Calculated percents for this month.
        /// </summary>
        public float Percents { get; set; }
    }
}
