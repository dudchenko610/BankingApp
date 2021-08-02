using BankingApp.ViewModels.ViewModels.Deposit;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BankingApp.UI.Components.DepositList
{
    /// <summary>
    /// Component renders list of deposits.
    /// </summary>
    public partial class DepositList
    {
        /// <summary>
        /// List of rendered deposits.
        /// </summary>
        [Parameter]
        public IList<DepositGetAllDepositViewItem> DepositViewList { get; set; }

        /// <summary>
        /// Triggers when user clicks deposit item.
        /// </summary>
        [Parameter]
        public EventCallback<int> OnDepositItemClicked { get; set; }

        private void OnDepositeHistoryClicked(int id)
        {
            OnDepositItemClicked.InvokeAsync(id);
        }
    }
}
