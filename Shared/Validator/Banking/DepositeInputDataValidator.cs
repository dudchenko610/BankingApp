using FluentValidation;
using FluentValidation.Validators;
using Shared.ViewModels.Banking;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Validator.Banking
{
    public class DepositeInputDataValidator : AbstractValidator<DepositeInputData>
    {
        public DepositeInputDataValidator()
        {
            RuleFor(d => d.Percents).InclusiveBetween(1, 100).WithMessage(Constants.Errors.Banking.INCORRECT_PERECENT_NUMBER);
            RuleFor(d => d.Months).InclusiveBetween(1, int.MaxValue).WithMessage(Constants.Errors.Banking.INCORRECT_MONTH_NUMBER);
            RuleFor(d => d.DepositSum).ScalePrecision(2, 8).WithMessage(Constants.Errors.Banking.INCORRECT_PRICE_FORMAT);
        }
    }
}
