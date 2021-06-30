using BankingApp.Api.Validators.Banking;
using BankingApp.ViewModels.Banking;
using NUnit.Framework;
using System.Collections.Generic;

namespace BankingApp.Tests.UnitTests
{
    [TestFixture]
    public class RequestCalculateDepositeBankingViewValidatorTest
    {
        private RequestCalculateDepositeBankingViewValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new RequestCalculateDepositeBankingViewValidator();
        }

        public static IEnumerable<RequestCalculateDepositeBankingView> RequestCalculateDepositeBankingViews
        {
            get
            {
                yield return new RequestCalculateDepositeBankingView { DepositeSum = -1, CalculationFormula = 0, MonthsCount = 1, Percents = 1};
                yield return new RequestCalculateDepositeBankingView { DepositeSum = 1, CalculationFormula = 0, MonthsCount = -1, Percents = 1 };
                yield return new RequestCalculateDepositeBankingView { DepositeSum = 1, CalculationFormula = 0, MonthsCount = 1, Percents = 101 };
                yield return new RequestCalculateDepositeBankingView { DepositeSum = 1, CalculationFormula = 0, MonthsCount = 1, Percents = -1 };
            }
        }

        [Test]
        [TestCaseSource("RequestCalculateDepositeBankingViews")]
        public void Test_RequestCalculateDepositeBankingView_Validation(RequestCalculateDepositeBankingView input)
        {
            var validateResult = _validator.Validate(input);

            if (validateResult.Errors.Count == 0)
            {
                Assert.Fail();
            }
        }

    }
}
