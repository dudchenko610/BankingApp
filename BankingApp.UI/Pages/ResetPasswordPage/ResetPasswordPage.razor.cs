using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.ViewModels.Authentication;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.Threading.Tasks;
using static BankingApp.UI.Core.Constants.Constants;

namespace BankingApp.UI.Pages.ResetPasswordPage
{
    /// <summary>
    /// Component renders reset password form.
    /// </summary>
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

        /// <summary>
        /// Creates instance of <see cref="ResetPasswordPage"/>.
        /// </summary>
        public ResetPasswordPage()
        {
            _resetPasswordView = new ResetPasswordAuthenticationView();
        }

        private async Task OnFormSubmitAsync()
        {
            var uri = _navigationWrapper.ToAbsoluteUri(_navigationWrapper.Uri);
            var queryStrings = QueryHelpers.ParseQuery(uri.Query);

            if (queryStrings.TryGetValue("email", out var email))
            {
                _resetPasswordView.Email = email;
            }

            if (queryStrings.TryGetValue("code", out var code))
            {
                _resetPasswordView.Code = code;
            }  

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
