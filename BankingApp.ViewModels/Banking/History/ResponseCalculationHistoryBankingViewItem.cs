using System.Collections.Generic;

namespace BankingApp.ViewModels.Banking.History
{
    public class ResponseCalculationHistoryBankingViewItem
    {
        public string CalculationFormula { get; set; }
        public decimal DepositeSum { get; set; }
        public int MonthsCount { get; set; }
        public int Percents { get; set; }

        public IList<ResponseCalculateDepositeBankingViewItem> DepositePerMonthInfo { get; set; }
    }
}
