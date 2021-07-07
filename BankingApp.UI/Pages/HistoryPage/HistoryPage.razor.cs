using BankingApp.UI.Core.Enums;
using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Core.Routes;
using BankingApp.ViewModels.Banking.History;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.UI.Pages.HistoryPage
{
    public partial class HistoryPage
    {
        private static readonly int DepositesOnPage = 2;

        private HistoryPageState _historyPageState;
        private int _totalPageCount;

        private ResponseCalculationHistoryBankingView _depositeHistory;
        private IList<ResponseCalculationHistoryBankingViewItem> _pagedChunck;

        [Inject]
        private IDepositeService _depositeService { get; set; }
        [Inject]
        private NavigationManager _navigationManager { get; set; }

        [Parameter]
        public int Page { get; set; }

        public HistoryPage()
        {
            _historyPageState = HistoryPageState.LoadingState;
        }

        protected override async Task OnInitializedAsync()
        {
            _depositeHistory = await _depositeService.GetCalculationDepositeHistoryAsync();

            if (_depositeHistory.DepositesHistory.Count == 0)
            {
                _historyPageState = HistoryPageState.EmptyHistoryState;
                return;
            }

            _historyPageState = HistoryPageState.DisplayHistoryState;
            _totalPageCount = (int) Math.Ceiling(_depositeHistory.DepositesHistory.Count / ((double) DepositesOnPage));
            _pagedChunck = _depositeHistory.DepositesHistory.Skip((Page - 1) * DepositesOnPage).Take(DepositesOnPage).ToList();

            if (Page > _totalPageCount || Page < 1)
                _navigationManager.NavigateTo($"{Routes.HistoryPage}/1");
        }

        protected override void OnParametersSet()
        {
            _pagedChunck = _depositeHistory.DepositesHistory.Skip((Page - 1) * DepositesOnPage).Take(DepositesOnPage).ToList();
            StateHasChanged();
        }

        private void OnPageClicked(int page)
        {
            _navigationManager.NavigateTo($"{Routes.HistoryPage}/{page}");
        }
    }
}
