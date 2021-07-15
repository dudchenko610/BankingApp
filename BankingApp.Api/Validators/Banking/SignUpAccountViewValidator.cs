using BankingApp.Shared;
using BankingApp.Shared.Extensions;
using BankingApp.ViewModels.Banking.Account;
using FluentValidation;

namespace BankingApp.Api.Validators.Banking
{
    public class SignUpAccountViewValidator : AbstractValidator<SignUpAuthenticationView>
    {
        public SignUpAccountViewValidator()
        {
            RuleFor(x => x.Nickname).MaximumLength(12);
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(Constants.Errors.Account.EmailRequired)
                .EmailAddress().WithMessage(Constants.Errors.Account.InvalidEmailFormat);
            RuleFor(x => x.Password).Password();
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);
        }
    }
}
