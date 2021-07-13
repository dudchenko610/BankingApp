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
using BankingApp.DataAccessLayer.Interfaces;
using System.Collections.Generic;
using System;
using BankingApp.Shared;

namespace BankingApp.BusinessLogicLayer.UnitTests.Services
{
    [TestFixture]
    public class DepositServiceTests
    {
        private const int DepositRepositoryAddReturnValue = 1;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(config => { config.AddProfile(new MapperProfile()); });
            _mapper = mapperConfig.CreateMapper();
        }

        [Test]
        public async Task Calculate_ValidDataPasses_ReturnsExpectedIdAndMappingHappensCorrectly()
        {
            Deposit deposit = null;

            var depositRepositoryMock = new Mock<IDepositRepository>();
            depositRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Deposit>()))
                .ReturnsAsync(DepositRepositoryAddReturnValue)
                .Callback((Deposit depHistory) => deposit = depHistory);

            DepositService depositService = new DepositService(_mapper, depositRepositoryMock.Object);
            int response = await depositService.CalculateAsync(GetValidCalculateDepositeView());

            response.Should().Be(DepositRepositoryAddReturnValue);
            deposit
                .Should().NotBeNull().And
                .BeEquivalentTo(GetValidCalculateDepositeView(), options => options.ExcludingMissingMembers());
        }

        [Test]
        public async Task GetAllAsync_CallGetAllMethod_ReturnsNotNullModelContainingCorrectlyMappedList()
        {
            var getAllDepositViewResponseFromService = GetValidDeposits();

            var depositRepositoryMock = new Mock<IDepositRepository>();
            depositRepositoryMock
                .Setup(x => x.GetAsync())
                .ReturnsAsync(getAllDepositViewResponseFromService);
            DepositService depositService = new DepositService(_mapper, depositRepositoryMock.Object);

            var resCalculationHistory = await depositService.GetAllAsync();

            resCalculationHistory
                .Should().NotBeNull().And
                .BeOfType<GetAllDepositView>()
                .Which.DepositItems.Should().NotBeNull();

            getAllDepositViewResponseFromService
                .Should().BeEquivalentTo(resCalculationHistory.DepositItems,
                    options => options.ExcludingMissingMembers().Excluding(x => x.CalculationFormula));
        }

        [Test]
        public async Task GetByIdAsync_CallGetByIdMethodPassingValidId_ReturnsNotNullModelContainingCorrectlyMappedList()
        {
            const int ValidDepositeHistoryId = 1;
            var getByIdDepositViewResponseFromService = GetValidDepositWithMonthlyPaymentItems();

            var depositRepositoryMock = new Mock<IDepositRepository>();
            depositRepositoryMock
                .Setup(x => x.GetDepositWithItemsByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(getByIdDepositViewResponseFromService);
            DepositService depositService = new DepositService(_mapper, depositRepositoryMock.Object);

            var responseGetByIdDepositView = await depositService.GetByIdAsync(ValidDepositeHistoryId);

            responseGetByIdDepositView
               .Should().NotBeNull().And
               .BeOfType<GetByIdDepositView>()
               .Which.MonthlyPaymentItems.Should().NotBeNull();

            getByIdDepositViewResponseFromService
                .Should().BeEquivalentTo(responseGetByIdDepositView, 
                    options => options.ExcludingMissingMembers().Excluding(x => x.CalculationFormula));
        }

        [Test]
        public void GetByIdAsync_CallGetByIdMethodPassingInvalidId_ThrowsExceptionWithCorrespondingMessage()
        {
            const int InvalidDepositeHistoryId = -1;

            var depositRepositoryMock = new Mock<IDepositRepository>();
            depositRepositoryMock
                .Setup(x => x.GetDepositWithItemsByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => { return null; });
            DepositService depositService = new DepositService(_mapper, depositRepositoryMock.Object);

            FluentActions.Awaiting(() => depositService.GetByIdAsync(InvalidDepositeHistoryId))
                .Should().Throw<Exception>().WithMessage(Constants.Errors.Deposit.IncorrectDepositeHistoryId);
        }

        private CalculateDepositView GetValidCalculateDepositeView()
        { 
            return new CalculateDepositView
            {
                DepositeSum = 100.00m,
                Percents = 5,
                CalculationFormula = DepositCalculationFormulaEnumView.SimpleInterest,
                MonthsCount = 2
            };
        }

        private IList<Deposit> GetValidDeposits()
        { 
            return new List<Deposit>
            {
                new Deposit {
                    Id = 1,
                    DepositeSum = 100,
                    CalculationFormula = Entities.Enums.CalculationFormulaEnum.SimpleInterest,
                    Percents = 2.45f
                },
                new Deposit {
                    Id = 2,
                    DepositeSum = 200,
                    CalculationFormula = Entities.Enums.CalculationFormulaEnum.CompoundInterest,
                    Percents = 5
                },
                new Deposit {
                    Id = 3,
                    DepositeSum = 300,
                    CalculationFormula = Entities.Enums.CalculationFormulaEnum.SimpleInterest,
                    Percents = 6
                }
            };
        }

        private Deposit GetValidDepositWithMonthlyPaymentItems()
        { 
            return new Deposit
            {
                Id = 1,
                CalculationFormula = Entities.Enums.CalculationFormulaEnum.SimpleInterest,
                CalulationDateTime = System.DateTime.Now,
                DepositeSum = 100.00m,
                MonthsCount = 4,
                Percents = 2.5f,
                MonthlyPayments = new List<MonthlyPayment>
                {
                    new MonthlyPayment
                    {
                        Id = 1,
                        DepositId = 1,
                        MonthNumber = 1,
                        Percents = 0.5f,
                        TotalMonthSum = 30.00m
                    },
                    new MonthlyPayment
                    {
                        Id = 2,
                        DepositId = 1,
                        MonthNumber = 2,
                        Percents = 1.5f,
                        TotalMonthSum = 70.00m
                    }
                }
            };
        }
    }
}
