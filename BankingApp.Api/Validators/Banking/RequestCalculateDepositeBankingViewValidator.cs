using BankingApp.Shared;
using BankingApp.ViewModels.Banking.Calculate;
using FluentValidation;

namespace BankingApp.Api.Validators.Banking
{
    public class RequestCalculateDepositeBankingViewValidator : AbstractValidator<CalculateDepositeBankingView>
    {
        public RequestCalculateDepositeBankingViewValidator()
        {
            RuleFor(d => d.Percents).InclusiveBetween(1, 100).WithMessage(Constants.Errors.Banking.IncorrectPercentNumber);
            RuleFor(d => d.MonthsCount).InclusiveBetween(1, int.MaxValue).WithMessage(Constants.Errors.Banking.IncorrectMonthFormat);
            RuleFor(d => d.DepositeSum).ScalePrecision(2, 8).GreaterThan(0).WithMessage(Constants.Errors.Banking.IncorrectPriceFormat);
        }
    }
}
