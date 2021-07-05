using System.Collections.Generic;

namespace BankingApp.ViewModels.Banking.History
{
    public class ResponseCalculationHistoryBankingViewItem
    {
        public IList<ResponseCalculateDepositeBankingViewItem> DepositePerMonthInfo { get; set; }
        public ResponseDepositeInfoBankingViewItem DepositeInfo { get; set; }
    }
}
