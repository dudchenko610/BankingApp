using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Core.Routes;
using BankingApp.ViewModels.Banking.History;
using BankingApp.ViewModels.Pagination;
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

        private int _totalPageCount;
        private ResponsePagedDataView<ResponseCalculationHistoryBankingViewItem> _pagedInfo;

        [Inject]
        private ILoaderService _loaderService { get; set; }
        [Inject]
        private IDepositeService _depositeService { get; set; }
        [Inject]
        private INavigationWrapper _navigationWrapper { get; set; }
        [Parameter]
        public int Page { get; set; }

        public HistoryPage()
        {
            _pagedInfo = null;
        }

        protected override async Task OnInitializedAsync()
        {
            await UpdateDepositeHistoryDataAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            await UpdateDepositeHistoryDataAsync();
            StateHasChanged();
        }

        private void OnPageClicked(int page)
        {
            _navigationWrapper.NavigateTo($"{Routes.HistoryPage}/{page}");
        }

        private void OnDepositeHistoryItemClicked(int depositeHistoryId)
        {
            _navigationWrapper.NavigateTo($"{Routes.DetailsPage}/{depositeHistoryId}");
        }

        private async Task UpdateDepositeHistoryDataAsync()
        {
            if (Page < 1)
            {
                _navigationWrapper.NavigateTo($"{Routes.HistoryPage}/1");
                return;
            }

            _loaderService.SwitchOn();
            _pagedInfo = await _depositeService.GetCalculationDepositeHistoryAsync(Page, DepositesOnPage);
            _loaderService.SwitchOff();

            if (_pagedInfo.Data.Count == 0)
                return;

            _totalPageCount = (int)Math.Ceiling(_pagedInfo.TotalItems / ((double)DepositesOnPage));

            if (Page > _totalPageCount || Page < 1)
                _navigationWrapper.NavigateTo($"{Routes.HistoryPage}/1");
        }
    }
}
