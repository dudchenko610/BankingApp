using BankingApp.Shared;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.ViewModels.ViewModels.Authentication
{
    /// <summary>
    /// View model used to send data about user that wants to reset password.
    /// </summary>
    public class ForgotPasswordAuthenticationView
    {
        /// <summary>
        /// User email.
        /// </summary>
        [Required(ErrorMessage = Constants.Errors.Authentication.EmailIsRequired)]
        [DataType(DataType.EmailAddress, ErrorMessage = Constants.Errors.Authentication.EmailIsRequired)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
