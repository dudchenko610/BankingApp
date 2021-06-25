using BankingApp.ViewModels.Banking;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BankingApp.UI.Components.DepositeResponseList
{
    public partial class DepositeResponseList
    {
        [Parameter]
        public IList<ResponseCalculateDepositeBankingViewItem> PerMonthInfos { get; set; }
    }
}
