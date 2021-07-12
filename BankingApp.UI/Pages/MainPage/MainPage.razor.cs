using BankingApp.UI.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using BankingApp.UI.Core.Routes;
using BankingApp.ViewModels.Banking.Calculate;

namespace BankingApp.UI.Pages.MainPage
{
    public partial class MainPage
    {
        [Inject]
        private INavigationWrapper _navigationWrapper { get; set; }
        [Inject]
        private IDepositeService _depositeService { get; set; }
        [Inject]
        private ILoaderService _loaderService { get; set; }
        
        protected async Task OnDepositeFormSubmit(RequestCalculateDepositeBankingView reqModel)
        {
            _loaderService.SwitchOn();
            int depositeHistoryId = await _depositeService.CalculateDepositeAsync(reqModel);
            _loaderService.SwitchOff();

            _navigationWrapper.NavigateTo($"{Routes.DetailsPage}/{depositeHistoryId}");
        }
    }
}
