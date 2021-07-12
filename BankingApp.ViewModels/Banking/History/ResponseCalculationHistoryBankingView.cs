using System.Collections.Generic;

namespace BankingApp.ViewModels.Banking.History
{
    public class ResponseCalculationHistoryBankingView
    {
        public IList<ResponseCalculationHistoryBankingViewItem> DepositesHistory { get; set; }
        
        public ResponseCalculationHistoryBankingView()
        {
            DepositesHistory = new List<ResponseCalculationHistoryBankingViewItem>();
        }
    }
}
