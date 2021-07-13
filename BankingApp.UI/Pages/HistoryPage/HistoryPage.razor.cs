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

        private int _totalPageCount;
        private GetAllDepositView _depositViews;
        private IList<DepositGetAllDepositViewItem> _pagedChunck;

        [Inject]
        private ILoaderService _loaderService { get; set; }
        [Inject]
        private IDepositService _depositeService { get; set; }
        [Inject]
        private INavigationWrapper _navigationWrapper { get; set; }
        [Parameter]
        public int Page { get; set; }

        public HistoryPage()
        {
            _depositViews = null;
        }

        protected override async Task OnInitializedAsync()
        {
            _loaderService.SwitchOn();
            _depositViews = await _depositeService.GetAllAsync();
            _loaderService.SwitchOff();

            if (_depositViews.DepositItems.Count == 0)
                return;

            _totalPageCount = (int) Math.Ceiling(_depositViews.DepositItems.Count / ((double) DepositesOnPage));
            _pagedChunck = _depositViews.DepositItems.Skip((Page - 1) * DepositesOnPage).Take(DepositesOnPage).ToList();

            if (Page > _totalPageCount || Page < 1)
                _navigationWrapper.NavigateTo($"{Routes.HistoryPage}/1");
        }

        protected override void OnParametersSet()
        {
            _pagedChunck = _depositViews.DepositItems.Skip((Page - 1) * DepositesOnPage).Take(DepositesOnPage).ToList();
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
    }
}
