using BankingApp.Entities.Entities;
using BankingApp.ViewModels.Banking.Account;
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Interfaces
{
    public interface IAuthenticationService
    {
        User User { get; }
        Task InitializeAsync();
        Task SignUpAsync(SignUpAuthenticationView signUpAccountView);
        Task<TokensView> ConfirmEmailAsync(ConfirmEmailAuthenticationView confirmEmailAccountView);

        Task<TokensView> SignInAsync(SignInAuthenticationView signInAccountView);
        Task LogoutAsync();
    }
}
