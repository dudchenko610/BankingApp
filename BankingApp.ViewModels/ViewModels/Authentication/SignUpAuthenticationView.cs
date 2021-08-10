using BankingApp.Shared;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.ViewModels.ViewModels.Authentication
{
    /// <summary>
    /// View model used to pass user's sign up data.
    /// </summary>
    public class SignUpAuthenticationView
    {
        /// <summary>
        /// User nickname.
        /// </summary>
        [Required(ErrorMessage = Constants.Errors.Authentication.NicknameIsRequired)]
        [StringLength(12, ErrorMessage = Constants.Errors.Authentication.NicknameLengthIsTooLong)]
        public string Nickname { get; set; }

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

        /// <summary>
        /// Password confirmation.
        /// </summary>
        [Required]
        [Compare("Password", ErrorMessage = Constants.Errors.Authentication.ConfirmPasswordShouldMatchPassword)]
        public string ConfirmPassword { get; set; }
    }
}
