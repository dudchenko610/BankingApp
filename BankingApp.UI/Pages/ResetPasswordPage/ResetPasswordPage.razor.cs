using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking.Authentication;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using static BankingApp.UI.Core.Constants.Constants;

namespace BankingApp.UI.Pages.ResetPasswordPage
{
    public partial class ResetPasswordPage
    {
        private ResetPasswordAuthenticationView _resetPasswordView;

        [Inject]
        private IAuthenticationService _authenticationService { get; set; }
        [Inject]
        private ILoaderService _loaderService { get; set; }
        [Inject]
        private IToastService _toastService { get; set; }
        [Inject]
        private INavigationWrapper _navigationWrapper { get; set; }

        public ResetPasswordPage()
        {
            _resetPasswordView = new ResetPasswordAuthenticationView();
        }
        private async Task OnFormSubmitAsync()
        {
            _loaderService.SwitchOn();
            if (await _authenticationService.ResetPasswordAsync(_resetPasswordView))
            {
                _toastService.ShowSuccess(Notifications.PasswordResetSuccessfully);
                _navigationWrapper.NavigateTo(Routes.SignInPage);
            }

            _loaderService.SwitchOff();
        }
    }
}
