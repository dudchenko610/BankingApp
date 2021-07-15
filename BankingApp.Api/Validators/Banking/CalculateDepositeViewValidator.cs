using BankingApp.Shared;
using BankingApp.ViewModels.Banking.Deposit;
using FluentValidation;

namespace BankingApp.Api.Validators.Banking
{
    public class CalculateDepositeViewValidator : AbstractValidator<CalculateDepositView>
    {
        public CalculateDepositeViewValidator()
        {
            RuleFor(d => d.Percents).InclusiveBetween(1, 100).WithMessage(Constants.Errors.Deposit.IncorrectPercentNumber);
            RuleFor(d => d.MonthsCount).InclusiveBetween(1, int.MaxValue).WithMessage(Constants.Errors.Deposit.IncorrectMonthFormat);
            RuleFor(d => d.DepositSum).ScalePrecision(2, 8).GreaterThan(0).WithMessage(Constants.Errors.Deposit.IncorrectPriceFormat);
        }
    }
}
