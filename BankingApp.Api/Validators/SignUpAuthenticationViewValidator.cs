using BankingApp.Shared;
using BankingApp.Shared.Extensions;
using BankingApp.ViewModels.ViewModels.Authentication;
using FluentValidation;

namespace BankingApp.Api.Validators
{
    /// <summary>
    /// Validates properties of <see cref="SignUpAuthenticationView"/>.
    /// </summary>
    public class SignUpAuthenticationViewValidator : AbstractValidator<SignUpAuthenticationView>
    {
        /// <summary>
        /// Assigns rules for validating properties of <see cref="SignUpAuthenticationView"/>.
        /// </summary>
        public SignUpAuthenticationViewValidator()
        {
            RuleFor(x => x.Nickname)
                .NotEmpty().WithMessage(Constants.Errors.Authentication.NicknameIsRequired)
                .MaximumLength(12).WithMessage(Constants.Errors.Authentication.NicknameLengthIsTooLong);
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(Constants.Errors.Authentication.EmailIsRequired)
                .EmailAddress().WithMessage(Constants.Errors.Authentication.InvalidEmailFormat);
            RuleFor(x => x.Password).Password();
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage(Constants.Errors.Authentication.ConfirmPasswordShouldMatchPassword);
        }
    }
}
