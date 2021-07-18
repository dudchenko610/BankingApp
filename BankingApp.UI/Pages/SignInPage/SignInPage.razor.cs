using BankingApp.UI.Core.Attributes;
using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking.Authentication;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BankingApp.UI.Pages.SignInPage
{
    [Unauthorized]
    public partial class SignInPage
    {
        private SignInAuthenticationView _signInView;

        [Inject]
        private IAuthenticationService _authenticationService { get; set; }

        public SignInPage()
        {
            _signInView = new SignInAuthenticationView();
        }

        private async Task OnFormSubmitAsync()
        {
            await _authenticationService.SignInAsync(_signInView);
        }
    }
}
