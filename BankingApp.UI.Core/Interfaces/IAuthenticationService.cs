using BankingApp.Entities.Entities;
using BankingApp.ViewModels.ViewModels.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Interfaces
{
    /// <summary>
    ///  Allows the user to provide authentication operations with account.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Used to get actual information about access token.
        /// </summary>
        TokensView TokensView { get; }

        /// <summary>
        /// Indicates if user has admin role.
        /// </summary>
        public bool IsAdmin { get; }

        /// <summary>
        /// Gets list of roles.
        /// </summary>
        /// <returns>List of role names.</returns>
        IList<string> GetRoles();

        /// <summary>
        /// Decodes access token from local storage.
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// Makes user registered the system.
        /// </summary>
        /// <param name="signUpAccountView">View model containing data needed to register user.</param>
        /// <returns>True if operation succeed and false otherwise./></returns>
        Task<bool> SignUpAsync(SignUpAuthenticationView signUpAccountView);


        /// <summary>
        /// Confirms user's email in system
        /// </summary>
        /// <param name="confirmEmailAccountView">View model containing user's email and confirmation token.</param>
        /// <returns>True if operation succeed and false otherwise.</returns>
        Task<bool> ConfirmEmailAsync(ConfirmEmailAuthenticationView confirmEmailAccountView);

        /// <summary>
        /// Makes user logged in the system.
        /// </summary>
        /// <param name="signInAccountView">View model containing data needed to sign in user.</param>
        /// <returns>True if operation succeed and false otherwise.</returns>
        Task<bool> SignInAsync(SignInAuthenticationView signInAccountView);

        /// <summary>
        /// Replaces old password with new.
        /// </summary>
        /// <param name="resetPasswordAuthenticationView">View model containing email, reset password token and new password.</param>
        /// <returns>True if operation succeed and false otherwise.</returns>
        Task<bool> ResetPasswordAsync(ResetPasswordAuthenticationView resetPasswordAuthenticationView);

        /// <summary>
        /// Provides user with ability to reset password.
        /// </summary>
        /// <param name="forgotPasswordAuthenticationView">View model containing user's email.</param>
        /// <returns>True if operation succeed and false otherwise.</returns>
        Task<bool> ForgotPasswordAsync(ForgotPasswordAuthenticationView forgotPasswordAuthenticationView);

        /// <summary>
        /// Logouts user.
        /// </summary>
        Task LogoutAsync();
    }
}
