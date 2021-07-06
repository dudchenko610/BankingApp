using BankingApp.ViewModels.Banking.History;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BankingApp.UI.Components.DepositeHistoryList
{
    public partial class DepositeHistoryList
    {
        private bool[] _collapsed;

        [Parameter]
        public IList<ResponseCalculationHistoryBankingViewItem> DepositesHistoryList { get; set; }

        protected override void OnInitialized() => _collapsed = new bool[DepositesHistoryList.Count];
        protected override void OnParametersSet() => _collapsed = new bool[DepositesHistoryList.Count];

        private void SetCollapse(int i)
        {
            _collapsed[i] = !_collapsed[i];
            StateHasChanged();
        } 
    }
}
