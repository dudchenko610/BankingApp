using NUnit.Framework;
using System.Collections.Generic;
using FluentAssertions;
using BankingApp.Shared;
using BankingApp.Api.Validators;
using BankingApp.ViewModels.ViewModels.Deposit;

namespace BankingApp.Api.UnitTests.Validators
{
    [TestFixture]
    public class CalculateDepositViewValidatorTests
    {
        private CalculateDepositeViewValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new CalculateDepositeViewValidator();
        }

        [Test]
        public void Validate_DepositeSumLessThanZero_ExpectedResults()
        {
            var validateResult = _validator.Validate(RequestCalculateDepositeBankingViews()[0]);
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Deposit.IncorrectPriceFormat);
        }

        [Test]
        public void Validate_MonthCountLessThanZero_ExpectedResults()
        {
            var validateResult = _validator.Validate(RequestCalculateDepositeBankingViews()[1]);
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Deposit.IncorrectMonthFormat);
        }

        [Test]
        public void Validate_PercentBiggerThan100_ExpectedResults()
        {
            var validateResult = _validator.Validate(RequestCalculateDepositeBankingViews()[2]);
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Deposit.IncorrectPercentNumber);
        }

        [Test]
        public void Validate_PercentLessThan1_ExpectedResults()
        {
            var validateResult = _validator.Validate(RequestCalculateDepositeBankingViews()[3]);
            validateResult.Errors.Should().Contain(x => x.ErrorMessage == Constants.Errors.Deposit.IncorrectPercentNumber);
        }

        private IList<CalculateDepositView> RequestCalculateDepositeBankingViews()
        {
            return new List<CalculateDepositView>
            {
                new CalculateDepositView { DepositSum = -1.0m, CalculationFormula = 0, MonthsCount = 1, Percents = 1 },
                new CalculateDepositView { DepositSum = 1, CalculationFormula = 0, MonthsCount = -1, Percents = 1 },
                new CalculateDepositView { DepositSum = 1.05m, CalculationFormula = 0, MonthsCount = 0, Percents = 105 },
                new CalculateDepositView { DepositSum = 1, CalculationFormula = 0, MonthsCount = 1, Percents = 0 }
            };
        }
    }
}
