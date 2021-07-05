
using BankingApp.BusinessLogicLayer.Services;
using BankingApp.ViewModels.Banking;
using BankingApp.ViewModels.Enums;
using NUnit.Framework;
using FluentAssertions;

namespace BankingApp.BusinessLogicLayer.UnitTests.Services
{
    [TestFixture]
    public class BankingServiceTests
    {
        private BankingCalculationService _bankingService;

        [SetUp]
        public void SetUp()
        {
            _bankingService = new BankingCalculationService();
        }

        [Test]
        public void CalculateDeposite_SimpleInterestFormulaPasses_PerMonthInfosCountEqualsToMonthCount()
        {
            const int MonthNumber = 100;

            var response = _bankingService.CalculateDeposite(
                new RequestCalculateDepositeBankingView
                {
                    DepositeSum = 1,
                    Percents = 1,
                    CalculationFormula = DepositeCalculationFormulaEnumView.SimpleInterest,
                    MonthsCount = MonthNumber
                }
            );

            response.Should()
                .NotBeNull().And.BeOfType<ResponseCalculateDepositeBankingView>()
                .Which.PerMonthInfos.Should().NotBeNull().And.HaveCount(MonthNumber);
        }

        [Test]
        public void CalculateDeposite_CompoundInterestFormulaPasses_ResultEqualsToExpected()
        {
            var response = _bankingService.CalculateDeposite(
                new RequestCalculateDepositeBankingView
                {
                    DepositeSum = 100,
                    Percents = 10,
                    CalculationFormula = DepositeCalculationFormulaEnumView.CompoundInterest,
                    MonthsCount = 2
                }
            );

            var expectedResult = new ResponseCalculateDepositeBankingView
            {
                PerMonthInfos =
                {
                    new ResponseCalculateDepositeBankingViewItem { MonthNumber = 1, TotalMonthSum = 100.83m, Percents = 0 },
                    new ResponseCalculateDepositeBankingViewItem { MonthNumber = 2, TotalMonthSum = 101.67m, Percents = 1 }
                }
            };

            response.Should().BeEquivalentTo(expectedResult);
            
        }
    }
}
