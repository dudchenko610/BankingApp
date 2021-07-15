using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.ViewModels.Banking.Account;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Services
{
    public class AccountService : IAccountService
    {
        public async Task<TokensView> ConfirmEmailAsync(ConfirmEmailAccountView confirmEmailAccountView)
        {
            throw new System.NotImplementedException();
        }

        public async Task<TokensView> SignInAsync(SignInAccountView signInAccountView)
        {
            throw new System.NotImplementedException();
        }

        public async Task SignUpAsync(SignUpAccountView signUpAccountView)
        {
            throw new System.NotImplementedException();
        }
    }
}
