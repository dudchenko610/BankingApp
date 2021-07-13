using System;
using System.Collections.Generic;

namespace BankingApp.ViewModels.Banking.History
{
    public class ResponseCalculationHistoryDetailsBankingView
    {
        public int Id { get; set; }
        public string CalculationFormula { get; set; }
        public decimal DepositeSum { get; set; }
        public int MonthsCount { get; set; }
        public float Percents { get; set; }
        public DateTime CalulationDateTime { get; set; }

        public IList<MonthlyPaymentResponseCalculationHistoryDetailsBankingViewItem> DepositePerMonthInfo { get; set; }
    }

    public class MonthlyPaymentResponseCalculationHistoryDetailsBankingViewItem
    {
        public int MonthNumber { get; set; }
        public decimal TotalMonthSum { get; set; }
        public float Percents { get; set; }
    }
}
