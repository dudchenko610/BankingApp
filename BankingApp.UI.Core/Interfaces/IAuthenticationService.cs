using BankingApp.ViewModels.Banking.Account;
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<TokensView> SignInAsync(SignInAuthenticationView signInAccountView);
        Task SignUpAsync(SignUpAuthenticationView signUpAccountView);
        Task<TokensView> ConfirmEmailAsync(ConfirmEmailAuthenticationView confirmEmailAccountView);
    }
}
