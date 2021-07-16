using BankingApp.Shared;
using BankingApp.Shared.Extensions;
using BankingApp.ViewModels.Banking.Account;
using FluentValidation;

namespace BankingApp.Api.Validators.Banking
{
    public class SignInAccountViewValidator : AbstractValidator<SignInAuthenticationView>
    {
        public SignInAccountViewValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(Constants.Errors.Account.EmailRequired)
                .EmailAddress().WithMessage(Constants.Errors.Account.InvalidEmailFormat);
            RuleFor(x => x.Password).Password();
        }
    }
}
