using BankingApp.Shared;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.ViewModels.ViewModels.Authentication
{
    public class ForgotPasswordAuthenticationView
    {
        [Required(ErrorMessage = Constants.Errors.Authentication.EmailIsRequired)]
        [DataType(DataType.EmailAddress, ErrorMessage = Constants.Errors.Authentication.EmailIsRequired)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
