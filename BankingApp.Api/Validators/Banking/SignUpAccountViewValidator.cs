using BankingApp.Shared;
using BankingApp.Shared.Extensions;
using BankingApp.ViewModels.Banking.Authentication;
using FluentValidation;

namespace BankingApp.Api.Validators.Banking
{
    public class SignUpAccountViewValidator : AbstractValidator<SignUpAuthenticationView>
    {
        public SignUpAccountViewValidator()
        {
            RuleFor(x => x.Nickname).MaximumLength(12).WithMessage(Constants.Errors.Authentication.NicknameLengthIsTooLong);
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(Constants.Errors.Authentication.EmailRequired)
                .EmailAddress().WithMessage(Constants.Errors.Authentication.InvalidEmailFormat);
            RuleFor(x => x.Password).Password();
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage(Constants.Errors.Authentication.InvalidEmailFormat);
        }
    }
}
