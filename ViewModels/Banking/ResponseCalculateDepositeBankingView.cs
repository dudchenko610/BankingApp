using System.Collections.Generic;

namespace ViewModels.Banking
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
