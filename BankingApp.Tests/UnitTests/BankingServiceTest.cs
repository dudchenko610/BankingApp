using BankingApp.BusinessLogicLayer.Services;
using BankingApp.ViewModels.Banking;
using NUnit.Framework;

namespace BankingApp.Tests.UnitTests
{
    [TestFixture]
    public class BankingServiceTest
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
    }
}
