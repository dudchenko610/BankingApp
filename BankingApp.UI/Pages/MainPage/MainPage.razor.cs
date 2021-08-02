using BankingApp.UI.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using static BankingApp.UI.Core.Constants.Constants;
using BankingApp.ViewModels.ViewModels.Deposit;

namespace BankingApp.UI.Pages.MainPage
{
    /// <summary>
    /// Component renders add-deposit form.
    /// </summary>
    public partial class MainPage
    {
        [Inject]
        private INavigationWrapper _navigationWrapper { get; set; }
        [Inject]
        private IDepositService _depositeService { get; set; }
        [Inject]
        private ILoaderService _loaderService { get; set; }
        
        protected async Task OnDepositFormSubmit(CalculateDepositView reqModel)
        {
            _loaderService.SwitchOn();
            int depositeHistoryId = await _depositeService.CalculateAsync(reqModel);
            _loaderService.SwitchOff();

            _navigationWrapper.NavigateTo($"{Routes.DetailsPage}/{depositeHistoryId}");
        }
    }
}
