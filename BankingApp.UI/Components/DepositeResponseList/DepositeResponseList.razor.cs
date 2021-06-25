using BankingApp.ViewModels.Banking;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BankingApp.UI.Components.DepositeResponseList
{
    public partial class DepositeResponseList
    {
        [CascadingParameter(Name = "PerMonthInfos")]
        public IList<ResponseCalculateDepositeBankingViewItem> PerMonthInfos { get; set; }
    }
}
