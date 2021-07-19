using BankingApp.UI.Core.Attributes;
using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Core.Routes;
using BankingApp.ViewModels.Banking.Authentication;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BankingApp.UI.Pages.SignUpPage
{
    [Unauthorized]
    public partial class SignUpPage
    {
        private SignUpAuthenticationView _signUpView;

        [Inject]
        private IAuthenticationService _authenticationService { get; set; }
        [Inject]
        private ILoaderService _loaderService { get; set; }

        public SignUpPage()
        {
            _signUpView = new SignUpAuthenticationView();
        }

        private async Task OnFormSubmitAsync()
        {
            _loaderService.SwitchOn();
            await _authenticationService.SignUpAsync(_signUpView);
            _loaderService.SwitchOff();
        }
    }
}
