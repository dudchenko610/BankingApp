using BankingApp.UI.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using BankingApp.UI.Core.Routes;
using BankingApp.ViewModels.Banking.Deposit;

namespace BankingApp.UI.Pages.MainPage
{
    public partial class MainPage
    {
        [Inject]
        private INavigationWrapper _navigationWrapper { get; set; }
        [Inject]
        private IDepositService _depositeService { get; set; }
        [Inject]
        private ILoaderService _loaderService { get; set; }
        
        protected async Task OnDepositeFormSubmit(CalculateDepositView reqModel)
        {
            _loaderService.SwitchOn();
            int depositeHistoryId = await _depositeService.CalculateAsync(reqModel);
            _loaderService.SwitchOff();

            _navigationWrapper.NavigateTo($"{Routes.DetailsPage}/{depositeHistoryId}");
        }
    }
}
