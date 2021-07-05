
namespace BankingApp.DataAccessLayer.Entities
{
    public class DepositeHistoryItem
    {
        public int Id { get; set; }
        public int DepositeHistoryId { get; set; }
        public DepositeHistory DepositeHistory { get; set; }
        public int MonthNumber { get; set; }
        public decimal TotalMonthSum { get; set; }
        public int Percents { get; set; }
    }
}
