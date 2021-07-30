using BankingApp.Shared;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.ViewModels.ViewModels.Authentication
{
    /// <summary>
    /// View model used to reset the user's password.
    /// </summary>
    public class ResetPasswordAuthenticationView
    {
        /// <summary>
        /// User email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Password reset token.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// New password.
        /// </summary>
        [MinLength(14, ErrorMessage = Constants.Errors.Authentication.PasswordIsTooShort)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)\S{14,}$", ErrorMessage = Constants.Errors.Authentication.PasswordIsNotHardEnough)]
        public string Password { get; set; }

        /// <summary>
        /// Password confirmation.
        /// </summary>
        [Required]
        [Compare("Password", ErrorMessage = Constants.Errors.Authentication.ConfirmPasswordShouldMatchPassword)]
        public string ConfirmPassword { get; set; }
    }
}
