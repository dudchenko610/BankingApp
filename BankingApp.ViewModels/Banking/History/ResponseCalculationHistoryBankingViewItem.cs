using BankingApp.ViewModels.Banking.Calculate;
using System;
using System.Collections.Generic;

namespace BankingApp.ViewModels.Banking.History
{
    public class ResponseCalculationHistoryBankingViewItem
    {
        public int Id { get; set; }
        public string CalculationFormula { get; set; }
        public decimal DepositeSum { get; set; }
        public int MonthsCount { get; set; }
        public float Percents { get; set; }
        public DateTime CalulationDateTime { get; set; }

        public IList<ResponseCalculateDepositeBankingViewItem> DepositePerMonthInfo { get; set; }
    }
}
