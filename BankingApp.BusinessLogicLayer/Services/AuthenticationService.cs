using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.ViewModels.Banking.Account;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public async Task<TokensView> ConfirmEmailAsync(ConfirmEmailAuthenticationView confirmEmailAccountView)
        {
            throw new System.NotImplementedException();
        }

        public async Task<TokensView> SignInAsync(SignInAuthenticationView signInAccountView)
        {
            throw new System.NotImplementedException();
        }

        public async Task SignUpAsync(SignUpAuthenticationView signUpAccountView)
        {
            throw new System.NotImplementedException();
        }
    }
}
