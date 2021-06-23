using FluentValidation;
using BankingApp.Shared;

namespace BankingApp.ViewModels.Banking.Validators
{
    public class RequestCalculateDepositeBankingViewValidator : AbstractValidator<RequestCalculateDepositeBankingView>
    {
        public RequestCalculateDepositeBankingViewValidator()
        {
            RuleFor(d => d.Percents).InclusiveBetween(1, 100).WithMessage(Constants.Errors.Banking.INCORRECT_PERECENT_NUMBER);
            RuleFor(d => d.MonthsCount).InclusiveBetween(1, int.MaxValue).WithMessage(Constants.Errors.Banking.INCORRECT_MONTH_NUMBER);
            RuleFor(d => d.DepositeSum).ScalePrecision(2, 8).WithMessage(Constants.Errors.Banking.INCORRECT_PRICE_FORMAT);
        }
    }
}
