using BankingApp.Shared;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.ViewModels.Banking.Authentication
{
    public class SignInAuthenticationView
    {
        [Required(ErrorMessage = Constants.Errors.Authentication.EmailIsRequired)]
        [DataType(DataType.EmailAddress, ErrorMessage = Constants.Errors.Authentication.EmailRequired)]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = Constants.Errors.Authentication.PasswordIsRequired)]
        [MinLength(14, ErrorMessage = Constants.Errors.Authentication.PasswordLength)]
        [RegularExpression(@"([A-Z])([a-z])([0-9])([^a-zA-Z0-9])", ErrorMessage = Constants.Errors.Authentication.PasswordIsNotHardEnough)]
        public string Password { get; set; }
    }
}
