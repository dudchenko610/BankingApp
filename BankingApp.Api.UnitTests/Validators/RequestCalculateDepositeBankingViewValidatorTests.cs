using BankingApp.Api.Validators.Banking;
using NUnit.Framework;
using System.Collections.Generic;
using FluentAssertions;
using BankingApp.Shared;
using BankingApp.ViewModels.Banking.Calculate;

namespace BankingApp.Api.UnitTests.Validators
{
    [TestFixture]
    public class RequestCalculateDepositeBankingViewValidatorTests
    {
        private RequestCalculateDepositeBankingViewValidator _validator;

        private IList<RequestCalculateDepositeBankingView> RequestCalculateDepositeBankingViews()
        {
            return new List<RequestCalculateDepositeBankingView>
            {
                new RequestCalculateDepositeBankingView { DepositeSum = -1.0m, CalculationFormula = 0, MonthsCount = 1, Percents = 1 },
                new RequestCalculateDepositeBankingView { DepositeSum = 1, CalculationFormula = 0, MonthsCount = -1, Percents = 1 },
                new RequestCalculateDepositeBankingView { DepositeSum = 1.05m, CalculationFormula = 0, MonthsCount = 0, Percents = 105 },
                new RequestCalculateDepositeBankingView { DepositeSum = 1, CalculationFormula = 0, MonthsCount = 1, Percents = 0 }
            };
        }

        [SetUp]
        public void SetUp()
        {
            _validator = new RequestCalculateDepositeBankingViewValidator();
        }

        [Test]
        public void Validate_DepositeSumLessThanZero_ValidErrorMessage()
        {
            var validateResult = _validator.Validate(RequestCalculateDepositeBankingViews()[0]);
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Banking.IncorrectPriceFormat);
        }

        [Test]
        public void Validate_MonthCountLessThanZero_ValidErrorMessage()
        {
            var validateResult = _validator.Validate(RequestCalculateDepositeBankingViews()[1]);
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Banking.IncorrectMonthFormat);
        }

        [Test]
        public void Validate_PercentBiggerThan100_ValidErrorMessage()
        {
            var validateResult = _validator.Validate(RequestCalculateDepositeBankingViews()[2]);
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Banking.IncorrectPercentNumber);
        }

        [Test]
        public void Validate_PercentLessThan1_ValidErrorMessage()
        {
            var validateResult = _validator.Validate(RequestCalculateDepositeBankingViews()[3]);
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Banking.IncorrectPercentNumber);
        }
    }
}
