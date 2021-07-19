using BankingApp.UI.Core.Attributes;
using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Core.Routes;
using BankingApp.ViewModels.Banking.Authentication;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BankingApp.UI.Pages.ConfirmEmailPage
{
    [Unauthorized]
    public partial class ConfirmEmailPage
    {
        [Inject]
        private IAuthenticationService _authenticationService { get; set; }
        [Inject]
        private INavigationWrapper _navigationWrapper { get; set; }

        [Parameter]
        public string Email { get; set; }
        [Parameter]
        public string Code { get; set; }

        private async Task OnClickConfirmEmailAsync()
        {
            var confirmEmailView = new ConfirmEmailAuthenticationView
            {
                Email = Email,
                Code = Code
            };

            await _authenticationService.ConfirmEmailAsync(confirmEmailView);
            _navigationWrapper.NavigateTo(Routes.SignInPage);
        }
    }
}
