using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApp.Entities.Entities
{
    public class DepositeHistoryItem : BaseEntity
    {
        [ForeignKey("DepositeHistory")]
        public int DepositeHistoryId { get; set; }
        public virtual DepositeHistory DepositeHistory { get; set; }

        public int MonthNumber { get; set; }
        public decimal TotalMonthSum { get; set; }
        public int Percents { get; set; }
    }
}
