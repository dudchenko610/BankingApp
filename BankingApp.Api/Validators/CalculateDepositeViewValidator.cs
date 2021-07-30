using BankingApp.Shared;
using BankingApp.ViewModels.ViewModels.Deposit;
using FluentValidation;

namespace BankingApp.Api.Validators
{
    /// <summary>
    /// Validates properties of <see cref="CalculateDepositView"/>.
    /// </summary>
    public class CalculateDepositeViewValidator : AbstractValidator<CalculateDepositView>
    {
        /// <summary>
        /// Assigns rules for validating properties of <see cref="CalculateDepositView"/>
        /// </summary>
        public CalculateDepositeViewValidator()
        {
            RuleFor(d => d.Percents).InclusiveBetween(1, 100).WithMessage(Constants.Errors.Deposit.IncorrectPercentNumber);
            RuleFor(d => d.MonthsCount).InclusiveBetween(1, int.MaxValue).WithMessage(Constants.Errors.Deposit.IncorrectMonthFormat);
            RuleFor(d => d.DepositSum).ScalePrecision(2, 8).GreaterThan(0).WithMessage(Constants.Errors.Deposit.IncorrectPriceFormat);
        }
    }
}
