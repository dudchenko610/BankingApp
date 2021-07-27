using BankingApp.ViewModels.Banking.Authentication;
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Interfaces
{
    public interface IAuthenticationService
    {
        TokensView TokensView { get; }
        bool IsAdmin { get; }
        Task InitializeAsync();
        Task<bool> SignUpAsync(SignUpAuthenticationView signUpAccountView);
        Task<bool> ConfirmEmailAsync(ConfirmEmailAuthenticationView confirmEmailAccountView);
        Task<bool> SignInAsync(SignInAuthenticationView signInAccountView);
        Task<bool> ResetPasswordAsync(ResetPasswordAuthenticationView resetPasswordAuthenticationView);
        Task<bool> ForgotPasswordAsync(ForgotPasswordAuthenticationView forgotPasswordAuthenticationView);
        Task LogoutAsync();
    }
}
