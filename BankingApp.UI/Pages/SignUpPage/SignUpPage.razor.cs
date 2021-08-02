using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.ViewModels.Authentication;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using static BankingApp.UI.Core.Constants.Constants;

namespace BankingApp.UI.Pages.SignUpPage
{
    /// <summary>
    /// Component renders sign up form.
    /// </summary>
    public partial class SignUpPage
    {
        private SignUpAuthenticationView _signUpView;

        [Inject]
        private IAuthenticationService _authenticationService { get; set; } 
        [Inject]
        private INavigationWrapper _navigationWrapper { get; set; }
        [Inject]
        private ILoaderService _loaderService { get; set; }
        [Inject]
        private IToastService _toastService { get; set; }

        /// <summary>
        /// Creates instance of <see cref="SignUpPage"/>.
        /// </summary>
        public SignUpPage()
        {
            _signUpView = new SignUpAuthenticationView();
        }

        private async Task OnFormSubmitAsync()
        {
            _loaderService.SwitchOn();

            if (await _authenticationService.SignUpAsync(_signUpView))
            {
                _toastService.ShowSuccess(Notifications.ConfirmYourEmail);
                _navigationWrapper.NavigateTo(Routes.SignInPage);
            }

            _loaderService.SwitchOff();
        }
    }
}
