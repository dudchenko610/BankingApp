﻿using BankingApp.BusinessLogicLayer.Services;
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
    public class BankingServiceTests
    {
        private const int DepositeRepositoryAddAsyncReturnValue = 1;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(config => { config.AddProfile(new MapperProfile()); });
            _mapper = mapperConfig.CreateMapper();
        }

        [Test]
        public async Task CalculateDeposite_ValidDataPasses_ReturnsExpectedIdAndMappingHappensCorrectly()
        {
            DepositeHistory depositeHistory = null;

            var depositeHistoryRepositoryMock = new Mock<IDepositeHistoryRepository>();
            depositeHistoryRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<DepositeHistory>()))
                .ReturnsAsync(DepositeRepositoryAddAsyncReturnValue)
                .Callback((DepositeHistory depHistory) => depositeHistory = depHistory);

            BankingService bankingService = new BankingService(_mapper, depositeHistoryRepositoryMock.Object);
            int response = await bankingService.CalculateDepositeAsync(GetValidCalculateDepositeRequest());

            response.Should().Be(DepositeRepositoryAddAsyncReturnValue);
            depositeHistory
                .Should().NotBeNull().And
                .BeEquivalentTo(GetValidCalculateDepositeRequest(), options => options.ExcludingMissingMembers());
        }

        [Test]
        public async Task GetDepositesCalculationHistory_CallGetHistoryMethod_ReturnsNotNullModelContainingCorrectlyMappedList()
        {
            var depositeHistoryRepositoryMock = new Mock<IDepositeHistoryRepository>();
            depositeHistoryRepositoryMock
                .Setup(x => x.GetAsync())
                .ReturnsAsync(GetValidDepositesHistory());
            BankingService bankingService = new BankingService(_mapper, depositeHistoryRepositoryMock.Object);

            var resCalculationHistory = await bankingService.GetDepositesCalculationHistoryAsync();

            resCalculationHistory
                .Should().NotBeNull().And
                .BeOfType<ResponseCalculationHistoryBankingView>()
                .Which.DepositesHistory.Should().NotBeNull();

            resCalculationHistory.DepositesHistory
                .Should().BeEquivalentTo(GetValidDepositesHistory(), options => options.ExcludingMissingMembers());
        }

        [Test]
        public async Task GetDepositeCalculationHistoryDetails_CallGetHistoryDetailsMethodPassingValidId_ReturnsNotNullModelContainingCorrectlyMappedList()
        {
            const int ValidDepositeHistoryId = 1;

            var depositeHistoryRepositoryMock = new Mock<IDepositeHistoryRepository>();
            depositeHistoryRepositoryMock
                .Setup(x => x.GetDepositeHistoryWithItemsAsync(It.IsAny<int>()))
                .ReturnsAsync(GetValidDepositeHistoryWithItems());
            BankingService bankingService = new BankingService(_mapper, depositeHistoryRepositoryMock.Object);

            var resCalculationHistoryDetails = await bankingService.GetDepositeCalculationHistoryDetailsAsync(ValidDepositeHistoryId);

            resCalculationHistoryDetails
               .Should().NotBeNull().And
               .BeOfType<ResponseCalculationHistoryDetailsBankingView>()
               .Which.DepositePerMonthInfo.Should().NotBeNull();

            resCalculationHistoryDetails
                .Should().BeEquivalentTo(GetValidDepositeHistoryWithItems(), options => options.ExcludingMissingMembers());
        }

        [Test]
        public void GetDepositeCalculationHistoryDetails_CallGetHistoryDetailsMethodPassingInvalidId_ThrowsExceptionWithCorrespondingMessage()
        {
            const int InvalidDepositeHistoryId = -1;

            var depositeHistoryRepositoryMock = new Mock<IDepositeHistoryRepository>();
            depositeHistoryRepositoryMock
                .Setup(x => x.GetDepositeHistoryWithItemsAsync(It.IsAny<int>()))
                .ReturnsAsync(() => { return null; });
            BankingService bankingService = new BankingService(_mapper, depositeHistoryRepositoryMock.Object);

            FluentActions.Awaiting(() => bankingService.GetDepositeCalculationHistoryDetailsAsync(InvalidDepositeHistoryId))
                .Should().Throw<Exception>().WithMessage(Constants.Errors.Banking.IncorrectDepositeHistoryId);
        }

        private CalculateDepositeBankingView GetValidCalculateDepositeRequest()
        { 
            return new CalculateDepositeBankingView
            {
                DepositeSum = 100.00m,
                Percents = 5,
                CalculationFormula = DepositeCalculationFormulaEnumView.SimpleInterest,
                MonthsCount = 2
            };
        }

        private IList<DepositeHistory> GetValidDepositesHistory()
        { 
            return new List<DepositeHistory>
            {
                new DepositeHistory {
                    Id = 1,
                    DepositeSum = 100,
                    CalculationFormula = Entities.Enums.CalculationFormulaEnum.SimpleInterest,
                    Percents = 2.45f
                },
                new DepositeHistory {
                    Id = 2,
                    DepositeSum = 200,
                    CalculationFormula = Entities.Enums.CalculationFormulaEnum.CompoundInterest,
                    Percents = 5
                },
                new DepositeHistory {
                    Id = 3,
                    DepositeSum = 300,
                    CalculationFormula = Entities.Enums.CalculationFormulaEnum.SimpleInterest,
                    Percents = 6
                }
            };
        }

        private DepositeHistory GetValidDepositeHistoryWithItems()
        { 
            return new DepositeHistory
            {
                Id = 1,
                CalculationFormula = Entities.Enums.CalculationFormulaEnum.SimpleInterest,
                CalulationDateTime = System.DateTime.Now,
                DepositeSum = 100.00m,
                MonthsCount = 4,
                Percents = 2.5f,
                DepositeHistoryItems = new List<DepositeHistoryItem>
                {
                    new DepositeHistoryItem
                    {
                        Id = 1,
                        DepositeHistoryId = 1,
                        MonthNumber = 1,
                        Percents = 0.5f,
                        TotalMonthSum = 30.00m
                    },
                    new DepositeHistoryItem
                    {
                        Id = 2,
                        DepositeHistoryId = 1,
                        MonthNumber = 2,
                        Percents = 1.5f,
                        TotalMonthSum = 70.00m
                    }
                }
            };
        }
    }
}
