using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking.Account;
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public Task<TokensView> ConfirmEmailAsync(ConfirmEmailAuthenticationView confirmEmailAccountView)
        {
            throw new System.NotImplementedException();
        }

        public Task<TokensView> SignInAsync(SignInAuthenticationView signInAccountView)
        {
            throw new System.NotImplementedException();
        }

        public Task SignUpAsync(SignUpAuthenticationView signUpAccountView)
        {
            throw new System.NotImplementedException();
        }
    }
}
