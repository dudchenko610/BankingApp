
using BankingApp.BusinessLogicLayer.Services;
using BankingApp.ViewModels.Banking;
using BankingApp.ViewModels.Enums;
using NUnit.Framework;
using FluentAssertions;
using AutoMapper;
using BankingApp.BusinessLogicLayer.Mapper;
using Moq;
using BankingApp.DataAccessLayer.Repositories.Interfaces;
using System.Threading.Tasks;
using BankingApp.Entities.Entities;

namespace BankingApp.BusinessLogicLayer.UnitTests.Services
{
    [TestFixture]
    public class BankingServiceTests
    {
        private BankingService _bankingService;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new DepositeHistoryProfile());
                config.AddProfile(new DepositeHistoryItemProfile());
            });

            var depositeServiceMoq = new Mock<IDepositeHistoryRepository>();
            _mapper = mapperConfig.CreateMapper();
            _bankingService = new BankingService(_mapper, depositeServiceMoq.Object);
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

        [Test]
        public async Task SaveDepositeCalculation_PassDataForSave_ReturnsIdOfSavedEntity()
        {
            const int DatabaseId = 5;

            var mockDepositeHistoryRepository = new Mock<IDepositeHistoryRepository>();
            mockDepositeHistoryRepository.Setup(m => m.AddAsync(It.IsAny<DepositeHistory>())).ReturnsAsync(DatabaseId);
            var bankingService = new BankingService(_mapper, mockDepositeHistoryRepository.Object);

            int addedHistoryId = await bankingService.SaveDepositeCalculationAsync(new RequestCalculateDepositeBankingView(),
                new ResponseCalculateDepositeBankingView());

            addedHistoryId.Should().Be(DatabaseId);
        }
    }
}
