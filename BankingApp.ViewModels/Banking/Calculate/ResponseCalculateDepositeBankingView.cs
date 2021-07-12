using System.Collections.Generic;

namespace BankingApp.ViewModels.Banking.Calculate
{
    public class ResponseCalculateDepositeBankingView
    {
        public IList<ResponseCalculateDepositeBankingViewItem> PerMonthInfos { get; set; }

        public ResponseCalculateDepositeBankingView()
        {
            PerMonthInfos = new List<ResponseCalculateDepositeBankingViewItem>();
        }
    }
}
