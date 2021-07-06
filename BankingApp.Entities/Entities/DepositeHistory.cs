using System;
using System.Collections.Generic;

namespace BankingApp.Entities.Entities
{
    public class DepositeHistory : BaseEntity
    {
        public string CalculationFormula { get; set; }
        public decimal DepositeSum { get; set; }
        public int MonthsCount { get; set; }
        public int Percents { get; set; }
        public DateTime CalulationDateTime { get; set; }
        public virtual IList<DepositeHistoryItem> DepositeHistoryItems { get; set; }
    }
}
