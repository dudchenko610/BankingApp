using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.ViewModels.Deposit;
using BankingApp.ViewModels.ViewModels.Pagination;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Threading.Tasks;
using static BankingApp.UI.Core.Constants.Constants;

namespace BankingApp.UI.Pages.HistoryPage
{
    /// <summary>
    /// Component renders list of deposit items.
    /// </summary>
    public partial class HistoryPage
    {
        private const int DepositsOnPage = 2;

        private int _totalPageCount;
        private PagedDataView<DepositGetAllDepositViewItem> _pagedDeposits;

        [Inject]
        private ILoaderService _loaderService { get; set; }
        [Inject]
        private IDepositService _depositService { get; set; }
        [Inject]
        private INavigationWrapper _navigationWrapper { get; set; }

        /// <summary>
        /// Number of rendered page.
        /// </summary>
        [Parameter]
        public int Page { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await UpdateDepositeHistoryDataAsync();
            StateHasChanged();
        }

        private void OnPageClicked(int page)
        {
            _navigationWrapper.NavigateTo($"{Routes.HistoryPage}/{page}");
        }

        private void OnDepositeHistoryItemClicked(int depositId)
        {
            _navigationWrapper.NavigateTo($"{Routes.DetailsPage}/{depositId}");
        }

        private async Task UpdateDepositeHistoryDataAsync()
        {
            if (Page < 1)
            {
                Page = 1;
            }

            _loaderService.SwitchOn();
            _pagedDeposits = await _depositService.GetAllAsync(Page, DepositsOnPage);
            _loaderService.SwitchOff();

            if (_pagedDeposits is null || _pagedDeposits.Items.Any())
            {
                return;
            }

            _totalPageCount = (int)Math.Ceiling(_pagedDeposits.TotalItems / ((double)DepositsOnPage));

            if (Page > _totalPageCount || Page < 1)
            { 
                _navigationWrapper.NavigateTo($"{Routes.HistoryPage}/1");
            }
        }
    }
}
