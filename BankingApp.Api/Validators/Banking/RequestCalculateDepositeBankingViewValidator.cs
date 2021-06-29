using BankingApp.Shared;
using BankingApp.ViewModels.Banking;
using FluentValidation;

namespace BankingApp.Api.Validators.Banking
{
    public class RequestCalculateDepositeBankingViewValidator : AbstractValidator<RequestCalculateDepositeBankingView>
    {
        public RequestCalculateDepositeBankingViewValidator()
        {
            RuleFor(d => d.Percents).InclusiveBetween(1, 100).WithMessage(Constants.Errors.Banking.IncorrectPercentNumber);
            RuleFor(d => d.MonthsCount).InclusiveBetween(1, int.MaxValue).WithMessage(Constants.Errors.Banking.IncorrectMonthFormat);
            RuleFor(d => d.DepositeSum).ScalePrecision(2, 8).WithMessage(Constants.Errors.Banking.IncorrectPriceFormat);
        }
    }
}
