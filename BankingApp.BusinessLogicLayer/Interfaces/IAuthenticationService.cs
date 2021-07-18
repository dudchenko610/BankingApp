using BankingApp.ViewModels.Banking.Account;
using BankingApp.ViewModels.Banking.Authentication;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    public interface IAuthenticationService
    {
        Task<TokensView> SignInAsync(SignInAuthenticationView signInAccountView);
        Task SignUpAsync(SignUpAuthenticationView signUpAccountView);
        Task<TokensView> ConfirmEmailAsync(ConfirmEmailAuthenticationView confirmEmailAccountView);
    }
}
