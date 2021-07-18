using BankingApp.UI.Core.Attributes;
using BankingApp.UI.Core.Interfaces;
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

        public SignUpPage()
        {
            _signUpView = new SignUpAuthenticationView();
        }

        private async Task OnFormSubmitAsync()
        {
            await _authenticationService.SignUpAsync(_signUpView);
        }
    }
}
