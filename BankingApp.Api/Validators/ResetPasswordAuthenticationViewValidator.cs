using BankingApp.Shared;
using BankingApp.Shared.Extensions;
using BankingApp.ViewModels.Banking.Authentication;
using FluentValidation;

namespace BankingApp.Api.Validators
{
    public class ResetPasswordAuthenticationViewValidator : AbstractValidator<ResetPasswordAuthenticationView>
    {
        public ResetPasswordAuthenticationViewValidator()
        {
            RuleFor(x => x.Password).Password();
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage(Constants.Errors.Authentication.InvalidEmailFormat);
        }
    }
}
