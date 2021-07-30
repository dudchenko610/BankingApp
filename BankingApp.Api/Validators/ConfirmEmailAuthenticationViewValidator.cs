using BankingApp.Shared;
using BankingApp.ViewModels.ViewModels.Authentication;
using FluentValidation;

namespace BankingApp.Api.Validators
{
    /// <summary>
    /// Validates properties of <see cref="ConfirmEmailAuthenticationView"/>.
    /// </summary>
    public class ConfirmEmailAuthenticationViewValidator : AbstractValidator<ConfirmEmailAuthenticationView>
    {
        /// <summary>
        /// Assigns rules for validating properties of <see cref="ConfirmEmailAuthenticationView"/>
        /// </summary>
        public ConfirmEmailAuthenticationViewValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(Constants.Errors.Authentication.EmailIsRequired)
                .EmailAddress().WithMessage(Constants.Errors.Authentication.InvalidEmailFormat);
        }
    }
}
