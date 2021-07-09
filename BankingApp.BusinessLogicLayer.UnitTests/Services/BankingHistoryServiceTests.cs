using AutoMapper;
using BankingApp.BusinessLogicLayer.Mapper;
using BankingApp.BusinessLogicLayer.Services;
using BankingApp.DataAccessLayer.Repositories.Interfaces;
using BankingApp.Entities.Entities;
using BankingApp.ViewModels.Banking;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;

namespace BankingApp.BusinessLogicLayer.UnitTests.Services
{
    [TestFixture]
    public class BankingHistoryServiceTests
    {
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new DepositeHistoryProfile());
                config.AddProfile(new DepositeHistoryItemProfile());
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Test]
        public async Task SaveDepositeCalculation_PassDataForSave_ReturnsIdOfSavedEntity()
        {
            const int DatabaseId = 5;

            var mockDepositeHistoryRepository = new Mock<IDepositeHistoryRepository>();
            mockDepositeHistoryRepository.Setup(m => m.AddAsync(It.IsAny<DepositeHistory>())).ReturnsAsync(DatabaseId);
            var bankingHistoryService = new BankingHistoryService(_mapper, mockDepositeHistoryRepository.Object);

            int addedHistoryId = await bankingHistoryService.SaveDepositeCalculationAsync(new RequestCalculateDepositeBankingView(),
                new ResponseCalculateDepositeBankingView());

            addedHistoryId.Should().Be(DatabaseId);
        }
    }
}
