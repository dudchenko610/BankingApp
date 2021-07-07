using BankingApp.ViewModels.Banking.History;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BankingApp.UI.Components.DepositeHistoryList
{
    public partial class DepositeHistoryList
    {
        [Parameter]
        public IList<ResponseCalculationHistoryBankingViewItem> DepositesHistoryList { get; set; }

        [Parameter]
        public EventCallback<int> OnDepositeHistoryItemClicked { get; set; }

        private void OnDepositeHistoryClicked(int id)
        {
            OnDepositeHistoryItemClicked.InvokeAsync(id);
        }
    }
}
