using FluentValidation;

namespace BankingApp.Shared.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder, int minimumLength = 14)
        {
            var options = ruleBuilder
                .NotEmpty().WithMessage(Constants.Errors.Account.PasswordEmpty)
                .MinimumLength(minimumLength).WithMessage(Constants.Errors.Account.PasswordLength)
                .Matches("[A-Z]").WithMessage(Constants.Errors.Account.PasswordUppercaseLetter)
                .Matches("[a-z]").WithMessage(Constants.Errors.Account.PasswordLowercaseLetter)
                .Matches("[0-9]").WithMessage(Constants.Errors.Account.PasswordDigit)
                .Matches("[^a-zA-Z0-9]").WithMessage(Constants.Errors.Account.PasswordSpecialCharacter);
            return options;
        }
    }
}
