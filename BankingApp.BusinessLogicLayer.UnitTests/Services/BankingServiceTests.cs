using BankingApp.BusinessLogicLayer.Services;
using BankingApp.ViewModels.Enums;
using FluentAssertions;
using AutoMapper;
using BankingApp.BusinessLogicLayer.Mapper;
using Moq;
using BankingApp.DataAccessLayer.Repositories.Interfaces;
using System.Threading.Tasks;
using BankingApp.Entities.Entities;
using BankingApp.ViewModels.Banking.Calculate;
using NUnit.Framework;
using BankingApp.ViewModels.Banking.History;

namespace BankingApp.BusinessLogicLayer.UnitTests.Services
{
    [TestFixture]
    public class BankingServiceTests
    {
        private const int DepositeRepositoryAddAsyncReturnValue = 1;
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
            depositeServiceMoq.Setup(x => x.AddAsync(It.IsAny<DepositeHistory>())).ReturnsAsync(DepositeRepositoryAddAsyncReturnValue);
            _mapper = mapperConfig.CreateMapper();
            _bankingService = new BankingService(_mapper, depositeServiceMoq.Object);
        }

        [Test]
        public async Task CalculateDeposite_ValidDataPasses_ReturnsExpectedId()
        {
            const int MonthNumber = 100;

            int response = await _bankingService.CalculateDepositeAsync(
                new RequestCalculateDepositeBankingView
                {
                    DepositeSum = 1,
                    Percents = 1,
                    CalculationFormula = DepositeCalculationFormulaEnumView.SimpleInterest,
                    MonthsCount = MonthNumber
                }
            );

            response.Should().Be(DepositeRepositoryAddAsyncReturnValue);
        }

        [Test]
        public async Task GetDepositesCalculationHistory_CallGetHistoryMethod_ReturnsNotNullModelContainingNotNullList()
        {
            var resCalculationHistory = await _bankingService.GetDepositesCalculationHistoryAsync();

            resCalculationHistory
                .Should().NotBeNull().And
                .BeOfType<ResponseCalculationHistoryBankingView>()
                .Which.DepositesHistory.Should().NotBeNull();
        }

        [Test]
        public async Task GetDepositeCalculationHistoryDetails__CallGetHistoryDetailsMethodPassingValidId_ReturnsNotNullModelContainingNotNullList()
        {
            const int DepositeHistoryId = 1;
            var resCalculationHistoryDetails = await _bankingService.GetDepositeCalculationHistoryDetailsAsync(DepositeHistoryId);

            resCalculationHistoryDetails
               .Should().NotBeNull().And
               .BeOfType<ResponseCalculationHistoryDetailsBankingView>()
               .Which.DepositePerMonthInfo.Should().NotBeNull();
        }
    }
}
