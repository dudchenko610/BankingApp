using BankingApp.BusinessLogicLayer.Services;
using BankingApp.ViewModels.Enums;
using FluentAssertions;
using AutoMapper;
using BankingApp.BusinessLogicLayer.Mapper;
using Moq;
using System.Threading.Tasks;
using BankingApp.Entities.Entities;
using BankingApp.ViewModels.Banking.Calculate;
using NUnit.Framework;
using BankingApp.ViewModels.Banking.History;
using System;
using BankingApp.Shared;
using BankingApp.ViewModels.Pagination;
using BankingApp.DataAccessLayer.Interfaces;

namespace BankingApp.BusinessLogicLayer.UnitTests.Services
{
    [TestFixture]
    public class BankingServiceTests
    {
        private const int InvalidPageNumber = 0;
        private const int InvalidPageSize = 0;
        private const int ValidPageNumber = 1;
        private const int ValidPageSize = 1;

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
            depositeServiceMoq.Setup(x => x.GetDepositeHistoryWithItemsAsync(It.IsAny<int>())).ReturnsAsync(new DepositeHistory());
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
            var resCalculationHistory = await _bankingService.GetDepositesCalculationHistoryAsync(ValidPageNumber, ValidPageSize);

            resCalculationHistory
                .Should().NotBeNull().And
                .BeOfType<ResponsePagedDataView<ResponseCalculationHistoryBankingViewItem>>()
                .Which.Data.Should().NotBeNull();
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
        
        [Test]
        public void GetDepositesCalculationHistory_PassInvalidPageNumber_ThrowsExceptionWithCorrespondingMessage()
        {
            Func<Task> taskFuncResult = async () => { await _bankingService.GetDepositesCalculationHistoryAsync(InvalidPageNumber, ValidPageSize); };
            taskFuncResult.Should().Throw<Exception>().WithMessage(Constants.Errors.Page.IncorrectPageNumberFormat);
        }

        [Test]
        public void GetDepositesCalculationHistory_PassInvalidPageSize_ThrowsExceptionWithCorrespondingMessage()
        {
            Func<Task> taskFuncResult = async () => { await _bankingService.GetDepositesCalculationHistoryAsync(ValidPageNumber, InvalidPageSize); };
            taskFuncResult.Should().Throw<Exception>().WithMessage(Constants.Errors.Page.IncorrectPageSizeFormat);
        }
    }
}
