using BankingApp.Shared;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.ViewModels.ViewModels.Authentication
{
    /// <summary>
    /// View model used to pass user's sign in data.
    /// </summary>
    public class SignInAuthenticationView
    {
        /// <summary>
        /// User email.
        /// </summary>
        [Required(ErrorMessage = Constants.Errors.Authentication.EmailIsRequired)]
        [DataType(DataType.EmailAddress, ErrorMessage = Constants.Errors.Authentication.EmailFormatIncorrect)]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        [MinLength(14, ErrorMessage = Constants.Errors.Authentication.PasswordIsTooShort)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)\S{14,}$", ErrorMessage = Constants.Errors.Authentication.PasswordIsNotHardEnough)]
        public string Password { get; set; }
    }
}
