using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApp.Entities.Entities
{
    public class MonthlyPayment : BaseEntity
    {
        [ForeignKey("DepositeHistory")]
        public int DepositId { get; set; }
        public virtual Deposit Deposit { get; set; }
        public int MonthNumber { get; set; }
        public decimal TotalMonthSum { get; set; }
        public float Percents { get; set; }
    }
}
