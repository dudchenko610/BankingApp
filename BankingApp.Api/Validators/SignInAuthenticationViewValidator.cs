using BankingApp.Shared;
using BankingApp.Shared.Extensions;
using BankingApp.ViewModels.ViewModels.Authentication;
using FluentValidation;

namespace BankingApp.Api.Validators
{
    public class SignInAuthenticationViewValidator : AbstractValidator<SignInAuthenticationView>
    {
        public SignInAuthenticationViewValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(Constants.Errors.Authentication.EmailIsRequired)
                .EmailAddress().WithMessage(Constants.Errors.Authentication.InvalidEmailFormat);
            RuleFor(x => x.Password).Password();
        }
    }
}
