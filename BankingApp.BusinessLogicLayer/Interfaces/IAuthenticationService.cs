using BankingApp.ViewModels.ViewModels.Authentication;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    public interface IAuthenticationService
    {
        Task<TokensView> SignInAsync(SignInAuthenticationView signInAccountView);
        Task SignUpAsync(SignUpAuthenticationView signUpAccountView);
        Task ConfirmEmailAsync(ConfirmEmailAuthenticationView confirmEmailAccountView);
        Task ForgotPasswordAsync(ForgotPasswordAuthenticationView forgotPasswordAuthenticationView);
        Task ResetPasswordAsync(ResetPasswordAuthenticationView resetPasswordAuthenticationView);
    }
}
