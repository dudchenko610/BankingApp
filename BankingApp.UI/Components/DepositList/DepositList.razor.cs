using BankingApp.ViewModels.ViewModels.Deposit;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BankingApp.UI.Components.DepositList
{
    public partial class DepositList
    {
        [Parameter]
        public IList<DepositGetAllDepositViewItem> DepositViewList { get; set; }

        [Parameter]
        public EventCallback<int> OnDepositItemClicked { get; set; }

        private void OnDepositeHistoryClicked(int id)
        {
            OnDepositItemClicked.InvokeAsync(id);
        }
    }
}
