using System.Collections.Generic;

namespace BankingApp.ViewModels.Banking
{
    public class ResponseCalculateDepositeBankingView
    {
        public ResponseCalculateDepositeBankingView()
        {
            PerMonthInfos = new List<ResponseCalculateDepositeBankingViewItem>();
        }
        public IList<ResponseCalculateDepositeBankingViewItem> PerMonthInfos { get; set; }
    }
}
