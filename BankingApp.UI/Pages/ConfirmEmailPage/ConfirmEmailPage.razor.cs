using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.ViewModels.Authentication;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.Threading.Tasks;
using static BankingApp.UI.Core.Constants.Constants;

namespace BankingApp.UI.Pages.ConfirmEmailPage
{
    /// <summary>
    /// Component renders confirm email button.
    /// </summary>
    public partial class ConfirmEmailPage
    {
        [Inject]
        private IAuthenticationService _authenticationService { get; set; }
        [Inject]
        private INavigationWrapper _navigationWrapper { get; set; }
        [Inject]
        private ILoaderService _loaderService { get; set; }
        [Inject]
        private IToastService _toastService { get; set; }

        private async Task OnClickConfirmEmailAsync()
        {
            var confirmEmailView = new ConfirmEmailAuthenticationView();
            var uri = _navigationWrapper.ToAbsoluteUri(_navigationWrapper.Uri);

            var queryStrings = QueryHelpers.ParseQuery(uri.Query);

            if (queryStrings.TryGetValue("email", out var email))
            {
                confirmEmailView.Email= email;
            }

            if (queryStrings.TryGetValue("code", out var code))
            {
                confirmEmailView.Code = code;
            }

            _loaderService.SwitchOn();

            if (await _authenticationService.ConfirmEmailAsync(confirmEmailView))
            {
                _toastService.ShowSuccess(Notifications.EmailSuccessfullyConfirmed);
                _navigationWrapper.NavigateTo(Routes.SignInPage);
            }

            _loaderService.SwitchOff();
        }
    }
}
