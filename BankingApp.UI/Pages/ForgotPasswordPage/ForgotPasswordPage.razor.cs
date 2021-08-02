using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.ViewModels.Authentication;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using static BankingApp.UI.Core.Constants.Constants;

namespace BankingApp.UI.Pages.ForgotPasswordPage
{
    /// <summary>
    /// Component renders forgot password form.
    /// </summary>
    public partial class ForgotPasswordPage
    {
        private ForgotPasswordAuthenticationView _forgotPasswordView;

        [Inject]
        private IAuthenticationService _authenticationService { get; set; }
        [Inject]
        private ILoaderService _loaderService { get; set; }
        [Inject]
        private IToastService _toastService { get; set; }
        [Inject]
        private INavigationWrapper _navigationWrapper { get; set; }

        /// <summary>
        /// Creates instance of <see cref="ForgotPasswordPage"/>.
        /// </summary>
        public ForgotPasswordPage()
        {
            _forgotPasswordView = new ForgotPasswordAuthenticationView();
        }

        private async Task OnFormSubmitAsync()
        {
            _loaderService.SwitchOn();

            if (await _authenticationService.ForgotPasswordAsync(_forgotPasswordView))
            {
                _toastService.ShowSuccess(Notifications.PasswordResetEmailSent);
                _navigationWrapper.NavigateTo(Routes.SignInPage);
            }

            _loaderService.SwitchOff();
        }
    }
}
