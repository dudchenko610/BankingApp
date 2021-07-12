using BankingApp.ViewModels.Banking.History;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BankingApp.UI.Components.DepositeHistoryItemList
{
    public partial class DepositeHistoryItemList
    {
        [Parameter]
        public IList<ResponseCalculationHistoryBankingViewItem> PerMonthInfos { get; set; }
    }
}
