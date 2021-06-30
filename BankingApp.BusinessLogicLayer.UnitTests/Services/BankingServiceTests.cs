
using BankingApp.BusinessLogicLayer.Services;
using BankingApp.ViewModels.Banking;
using BankingApp.ViewModels.Enums;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BankingApp.BusinessLogicLayer.UnitTests.Services
{
    [TestFixture]
    class BankingServiceTests
    {
        private BankingService _bankingService;

        [SetUp]
        public void SetUp()
        {
            _bankingService = new BankingService();
        }

        [Test]
        public void MonthNumber_Equals_ResultListCount()
        {
            const int MONTH_NUMBER = 100;

            var response = _bankingService.CalculateDeposite(
                new RequestCalculateDepositeBankingView
                {
                    DepositeSum = 1,
                    Percents = 1,
                    CalculationFormula = 0,
                    MonthsCount = MONTH_NUMBER
                }
            );

            Assert.NotNull(response);
            Assert.NotNull(response.PerMonthInfos);
            Assert.AreEqual(response.PerMonthInfos.Count, MONTH_NUMBER); // Equals for reference equality !!!
        }

        [Test]
        public void ValidResult_On_ValidRequest()
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

            Assert.AreEqual(JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(expectedResult));
        }
    }
}
