using BankingApp.Shared;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.ViewModels.Banking.Authentication
{
    public class SignUpAuthenticationView
    {
        [Required(ErrorMessage = Constants.Errors.Authentication.NicknameIsRequired)]
        [StringLength(12, ErrorMessage = Constants.Errors.Authentication.NicknameLengthIsTooLong)]
        public string Nickname { get; set; }
        [Required(ErrorMessage = Constants.Errors.Authentication.EmailIsRequired)]
        [DataType(DataType.EmailAddress, ErrorMessage = Constants.Errors.Authentication.EmailFormatIncorrect)]
        [EmailAddress]
        public string Email { get; set; }
        [MinLength(14, ErrorMessage = Constants.Errors.Authentication.PasswordIsTooShort)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)\S{14,}$", ErrorMessage = Constants.Errors.Authentication.PasswordIsNotHardEnough)]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = Constants.Errors.Authentication.ConfirmPasswordShouldMatchPassword)]
        public string ConfirmPassword { get; set; }
    }
}
