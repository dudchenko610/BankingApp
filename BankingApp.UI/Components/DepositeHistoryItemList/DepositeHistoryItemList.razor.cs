using BankingApp.ViewModels.Banking;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BankingApp.UI.Components.DepositeHistoryItemList
{
    public partial class DepositeHistoryItemList
    {
        [Parameter]
        public IList<ResponseCalculateDepositeBankingViewItem> PerMonthInfos { get; set; }
    }
}
