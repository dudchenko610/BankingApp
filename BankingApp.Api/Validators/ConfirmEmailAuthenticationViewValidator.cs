using BankingApp.Shared;
using BankingApp.ViewModels.ViewModels.Authentication;
using FluentValidation;

namespace BankingApp.Api.Validators
{
    public class ConfirmEmailAuthenticationViewValidator : AbstractValidator<ConfirmEmailAuthenticationView>
    {
        public ConfirmEmailAuthenticationViewValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(Constants.Errors.Authentication.EmailIsRequired)
                .EmailAddress().WithMessage(Constants.Errors.Authentication.InvalidEmailFormat);
        }
    }
}
