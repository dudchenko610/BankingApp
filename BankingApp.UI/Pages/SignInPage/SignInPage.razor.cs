using BankingApp.UI.Core.Attributes;
using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking.Authentication;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using static BankingApp.UI.Core.Constants.Constants;

namespace BankingApp.UI.Pages.SignInPage
{
    [Unauthorized]
    public partial class SignInPage
    {
        private SignInAuthenticationView _signInView;

        [Inject]
        private IAuthenticationService _authenticationService { get; set; }
        [Inject]
        private ILoaderService _loaderService { get; set; }
        [Inject]
        private IToastService _toastService { get; set; }
        [Inject]
        private INavigationWrapper _navigationWrapper { get; set; }

        public SignInPage()
        {
            _signInView = new SignInAuthenticationView();
        }

        private async Task OnFormSubmitAsync()
        {
            _loaderService.SwitchOn();
            if (await _authenticationService.SignInAsync(_signInView))
            {
                _toastService.ShowSuccess(Notifications.SignInSuccess);
                _navigationWrapper.NavigateTo(Routes.MainPage);
            }

            _loaderService.SwitchOff();
        }
    }
}
