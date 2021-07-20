
using BankingApp.Shared;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.ViewModels.Banking.Authentication
{
    public class ResetPasswordAuthenticationView
    {
        public string Email { get; set; }
        public string Code { get; set; }
        [MinLength(14, ErrorMessage = Constants.Errors.Authentication.PasswordIsTooShort)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)\S{14,}$", ErrorMessage = Constants.Errors.Authentication.PasswordIsNotHardEnough)]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = Constants.Errors.Authentication.ConfirmPasswordShouldMatchPassword)]
        public string ConfirmPassword { get; set; }
    }
}
