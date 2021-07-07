﻿using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using BankingApp.UI.Core.Routes;

namespace BankingApp.UI.Pages.MainPage
{
    public partial class MainPage
    {
        [Inject]
        private NavigationManager _navigationManager { get; set; }
        [Inject]
        private IDepositeService _depositeService { get; set; }
        [Inject]
        private ILoaderService _loaderService { get; set; }
        
        protected async Task OnDepositeFormSubmit(RequestCalculateDepositeBankingView reqModel)
        {
            _loaderService.SwitchOn();
            int depositeHistoryId = await _depositeService.CalculateDepositeAsync(reqModel);
            _loaderService.SwitchOff();

            _navigationManager.NavigateTo($"{Routes.DetailsPage}/{depositeHistoryId}");
        }

    }
}
