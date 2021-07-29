using FluentValidation;

namespace BankingApp.Shared.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder
                .NotEmpty().WithMessage(Constants.Errors.Authentication.PasswordEmpty)
                .MinimumLength(Constants.Password.MinPasswordLength).WithMessage(Constants.Errors.Authentication.PasswordIsTooShort)
                .Matches("[A-Z]").WithMessage(Constants.Errors.Authentication.PasswordUppercaseLetter)
                .Matches("[a-z]").WithMessage(Constants.Errors.Authentication.PasswordLowercaseLetter)
                .Matches("[0-9]").WithMessage(Constants.Errors.Authentication.PasswordDigit);

            return options;
        }
    }
}
