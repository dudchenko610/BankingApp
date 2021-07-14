﻿using BankingApp.BusinessLogicLayer.Services;
using BankingApp.ViewModels.Enums;
using FluentAssertions;
using AutoMapper;
using BankingApp.BusinessLogicLayer.Mapper;
using Moq;
using System.Threading.Tasks;
using BankingApp.Entities.Entities;
using NUnit.Framework;
using BankingApp.DataAccessLayer.Interfaces;
using System.Collections.Generic;
using System;
using BankingApp.Shared;
using BankingApp.ViewModels.Banking.Deposit;
using BankingApp.Shared.Extensions;

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
        public async Task Calculate_SimpleInterestCalculationFormula_ReturnsExpectedIdAndMappingHappensCorrectly()
        {
            var inputServiceModel = GetValidSimpleInterestCalculateDepositeView();
            var validMonthlyPayments = GetValidSimpleInterestMonthlyPayments();

            Deposit deposit = null;

            var depositRepositoryMock = new Mock<IDepositRepository>();
            depositRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Deposit>()))
                .ReturnsAsync(DepositRepositoryAddReturnValue)
                .Callback((Deposit depHistory) => deposit = depHistory);

            DepositService depositService = new DepositService(_mapper, depositRepositoryMock.Object);
            int response = await depositService.CalculateAsync(inputServiceModel);

            response.Should().Be(DepositRepositoryAddReturnValue);
            deposit
                .Should().NotBeNull().And
                .BeEquivalentTo(inputServiceModel, options => options.ExcludingMissingMembers());
            deposit.MonthlyPayments.Should().NotBeNull().And.BeEquivalentTo(validMonthlyPayments);
        }

        [Test]
        public async Task Calculate_CompoundInterestCalculationFormula_ReturnsExpectedIdAndMappingHappensCorrectly()
        {
            var inputServiceModel = GetValidCompoundInterestCalculateDepositeView();
            var validMonthlyPayments = GetValidCompoundInterestMonthlyPayments();

            Deposit deposit = null;

            var depositRepositoryMock = new Mock<IDepositRepository>();
            depositRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Deposit>()))
                .ReturnsAsync(DepositRepositoryAddReturnValue)
                .Callback((Deposit depHistory) => deposit = depHistory);

            DepositService depositService = new DepositService(_mapper, depositRepositoryMock.Object);
            int response = await depositService.CalculateAsync(inputServiceModel);

            response.Should().Be(DepositRepositoryAddReturnValue);
            deposit
                .Should().NotBeNull().And
                .BeEquivalentTo(inputServiceModel, options => options.ExcludingMissingMembers());
            deposit.MonthlyPayments.Should().NotBeNull().And.BeEquivalentTo(validMonthlyPayments);
        }

        [Test]
        public async Task Calculate_ValidDataPasses_MonthCountEqualsToMonthlyPaymentsCount()
        {
            var inputServiceModel = GetValidCompoundInterestCalculateDepositeView();
            var validMonthlyPayments = GetValidCompoundInterestMonthlyPayments();

            Deposit deposit = null;

            var depositRepositoryMock = new Mock<IDepositRepository>();
            depositRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Deposit>()))
                .ReturnsAsync(DepositRepositoryAddReturnValue)
                .Callback((Deposit depHistory) => deposit = depHistory);

            DepositService depositService = new DepositService(_mapper, depositRepositoryMock.Object);
            await depositService.CalculateAsync(inputServiceModel);

            deposit.Should().NotBeNull();
            deposit.MonthlyPayments.Count.Should().Be(validMonthlyPayments.Count);
        }

        [Test]
        public async Task GetAllAsync_CallGetAllMethod_ReturnsNotNullModelContainingCorrectlyMappedList()
        {
            var depositsResponseOfRepoistory = GetValidDeposits();

            var depositRepositoryMock = new Mock<IDepositRepository>();
            depositRepositoryMock
                .Setup(x => x.GetAsync())
                .ReturnsAsync(depositsResponseOfRepoistory);
            DepositService depositService = new DepositService(_mapper, depositRepositoryMock.Object);

            var resCalculationHistory = await depositService.GetAllAsync();

            resCalculationHistory
                .Should().NotBeNull().And
                .BeOfType<GetAllDepositView>()
                .Which.DepositItems.Should().NotBeNull();

            depositsResponseOfRepoistory
                .Should().BeEquivalentTo(resCalculationHistory.DepositItems,
                    options => options.ExcludingMissingMembers().Excluding(x => x.CalculationFormula));

            for (int i = 0; i < depositsResponseOfRepoistory.Count; i++)
            {
                ((DepositCalculationFormulaEnumView) depositsResponseOfRepoistory[i].CalculationFormula).GetDisplayValue()
                .Should().Be(resCalculationHistory.DepositItems[i].CalculationFormula);
            }
        }

        [Test]
        public async Task GetByIdAsync_CallGetByIdMethodPassingValidId_ReturnsNotNullModelContainingCorrectlyMappedList()
        {
            const int ValidDepositeHistoryId = 1;
            var depositsResponseOfRepository = GetValidDepositWithMonthlyPaymentItems();

            var depositRepositoryMock = new Mock<IDepositRepository>();
            depositRepositoryMock
                .Setup(x => x.GetDepositWithItemsByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(depositsResponseOfRepository);
            DepositService depositService = new DepositService(_mapper, depositRepositoryMock.Object);

            var responseGetByIdDepositView = await depositService.GetByIdAsync(ValidDepositeHistoryId);

            responseGetByIdDepositView
               .Should().NotBeNull().And
               .BeOfType<GetByIdDepositView>()
               .Which.MonthlyPaymentItems.Should().NotBeNull();

            depositsResponseOfRepository
                .Should().BeEquivalentTo(responseGetByIdDepositView, 
                    options => options
                        .ExcludingMissingMembers().Excluding(x => x.CalculationFormula));

            ((DepositCalculationFormulaEnumView) depositsResponseOfRepository.CalculationFormula).GetDisplayValue()
                .Should().Be(responseGetByIdDepositView.CalculationFormula);
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

        private CalculateDepositView GetValidSimpleInterestCalculateDepositeView()
        { 
            return new CalculateDepositView
            {
                DepositSum = 100.00m,
                Percents = 5,
                CalculationFormula = DepositCalculationFormulaEnumView.SimpleInterest,
                MonthsCount = 2
            };
        }

        private CalculateDepositView GetValidCompoundInterestCalculateDepositeView()
        {
            return new CalculateDepositView
            {
                DepositSum = 300.00m,
                Percents = 6,
                CalculationFormula = DepositCalculationFormulaEnumView.CompoundInterest,
                MonthsCount = 2
            };
        }

        private IList<MonthlyPayment> GetValidSimpleInterestMonthlyPayments()
        {
            return new List<MonthlyPayment>
            {
                new MonthlyPayment
                { 
                    MonthNumber = 1,
                    TotalMonthSum = 100.42m,
                    Percents = 0.42f
                },
                new MonthlyPayment
                {
                    MonthNumber = 2,
                    TotalMonthSum = 100.83m,
                    Percents = 0.83f
                }
            };
        }

        private IList<MonthlyPayment> GetValidCompoundInterestMonthlyPayments()
        {
            return new List<MonthlyPayment>
            {
                new MonthlyPayment
                {
                    MonthNumber = 1,
                    TotalMonthSum = 301.50m,
                    Percents = 0.5f
                },
                new MonthlyPayment
                {
                    MonthNumber = 2,
                    TotalMonthSum = 303.01m,
                    Percents = 1f
                }
            };
        }

        private IList<Deposit> GetValidDeposits()
        { 
            return new List<Deposit>
            {
                new Deposit {
                    Id = 1,
                    DepositSum = 100,
                    CalculationFormula = Entities.Enums.CalculationFormulaEnum.SimpleInterest,
                    Percents = 2.45f
                },
                new Deposit {
                    Id = 2,
                    DepositSum = 200,
                    CalculationFormula = Entities.Enums.CalculationFormulaEnum.CompoundInterest,
                    Percents = 5
                },
                new Deposit {
                    Id = 3,
                    DepositSum = 300,
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
                CalсulationDateTime = System.DateTime.Now,
                DepositSum = 100.00m,
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