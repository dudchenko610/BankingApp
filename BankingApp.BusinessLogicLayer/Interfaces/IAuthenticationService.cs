using BankingApp.ViewModels.ViewModels.Authentication;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Allows the user to provide authentication operations with account.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Makes user logged in the system.
        /// </summary>
        /// <param name="signInAccountView">View model containing data needed to sign in user.</param>
        /// <returns>View model containing access token.</returns>
        Task<TokensView> SignInAsync(SignInAuthenticationView signInAccountView);

        /// <summary>
        /// Makes user registered the system.
        /// </summary>
        /// <param name="signUpAccountView">View model containing data needed to register user.</param>
        Task SignUpAsync(SignUpAuthenticationView signUpAccountView);

        /// <summary>
        /// Confirms user's email in system
        /// </summary>
        /// <param name="confirmEmailAccountView">View model containing user's email and confirmation token.</param>
        Task ConfirmEmailAsync(ConfirmEmailAuthenticationView confirmEmailAccountView);

        /// <summary>
        /// Provides user with ability to reset password.
        /// </summary>
        /// <param name="forgotPasswordAuthenticationView">View model containing user's email.</param>
        Task ForgotPasswordAsync(ForgotPasswordAuthenticationView forgotPasswordAuthenticationView);

        /// <summary>
        /// Replaces old password with new.
        /// </summary>
        /// <param name="resetPasswordAuthenticationView">View model containing email, reset password token and new password.</param>
        Task ResetPasswordAsync(ResetPasswordAuthenticationView resetPasswordAuthenticationView);
    }
}
