using BankingApp.Shared;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.ViewModels.Banking.Authentication
{
    public class ResetPasswordAuthenticationView
    {
        [Required(ErrorMessage = Constants.Errors.Authentication.EmailIsRequired)]
        [DataType(DataType.EmailAddress, ErrorMessage = Constants.Errors.Authentication.EmailRequired)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
