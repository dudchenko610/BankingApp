using BankingApp.Shared;
using BankingApp.Shared.Extensions;
using BankingApp.ViewModels.ViewModels.Authentication;
using FluentValidation;

namespace BankingApp.Api.Validators
{
    /// <summary>
    /// Validates properties of <see cref="ResetPasswordAuthenticationView"/>.
    /// </summary>
    public class ResetPasswordAuthenticationViewValidator : AbstractValidator<ResetPasswordAuthenticationView>
    {
        /// <summary>
        /// Assigns rules for validating properties of <see cref="ResetPasswordAuthenticationView"/>.
        /// </summary>
        public ResetPasswordAuthenticationViewValidator()
        {
            RuleFor(x => x.Password).Password();
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage(Constants.Errors.Authentication.InvalidEmailFormat);
        }
    }
}
