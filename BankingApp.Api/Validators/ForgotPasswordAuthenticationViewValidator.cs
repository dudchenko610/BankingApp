using BankingApp.Shared;
using BankingApp.ViewModels.ViewModels.Authentication;
using FluentValidation;

namespace BankingApp.Api.Validators
{
    /// <summary>
    /// Validates properties of <see cref="ForgotPasswordAuthenticationView"/>.
    /// </summary>
    public class ForgotPasswordAuthenticationViewValidator : AbstractValidator<ForgotPasswordAuthenticationView>
    {
        /// <summary>
        /// Assigns rules for validating properties of <see cref="ForgotPasswordAuthenticationView"/>.
        /// </summary>
        public ForgotPasswordAuthenticationViewValidator()
        {
            RuleFor(x => x.Email)
                    .NotEmpty().WithMessage(Constants.Errors.Authentication.EmailIsRequired)
                    .EmailAddress().WithMessage(Constants.Errors.Authentication.InvalidEmailFormat);
        }
    }
}
