using BankingApp.Shared;
using BankingApp.Shared.Extensions;
using BankingApp.ViewModels.ViewModels.Authentication;
using FluentValidation;

namespace BankingApp.Api.Validators
{
    /// <summary>
    /// Validates properties of <see cref="SignInAuthenticationView"/>.
    /// </summary>
    public class SignInAuthenticationViewValidator : AbstractValidator<SignInAuthenticationView>
    {
        /// <summary>
        /// Assigns rules for validating properties of <see cref="SignInAuthenticationView"/>.
        /// </summary>
        public SignInAuthenticationViewValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(Constants.Errors.Authentication.EmailIsRequired)
                .EmailAddress().WithMessage(Constants.Errors.Authentication.InvalidEmailFormat);
            RuleFor(x => x.Password).Password();
        }
    }
}
