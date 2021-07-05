﻿
using System;
using System.Collections.Generic;

namespace BankingApp.DataAccessLayer.Entities
{
    public class DepositeHistory
    {
        public int Id { get; set; }

        public string CalculationFormula { get; set; }
        public decimal DepositeSum { get; set; }
        public int MonthsCount { get; set; }
        public int Percents { get; set; }

        public DateTime CalulationDateTime { get; set; }

        public IList<DepositeHistoryItem> DepositeHistoryItems { get; set; }
    }
}
