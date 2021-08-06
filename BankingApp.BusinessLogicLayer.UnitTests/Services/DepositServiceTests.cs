using BankingApp.BusinessLogicLayer.Services;
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
using BankingApp.Shared.Extensions;
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.ViewModels.ViewModels.Deposit;
using BankingApp.ViewModels.ViewModels.Pagination;
using BankingApp.DataAccessLayer.Models;
using BankingApp.Entities.Enums;

namespace BankingApp.BusinessLogicLayer.UnitTests.Services
{
    [TestFixture]
    public class DepositServiceTests
    {
        private const int DepositRepositoryAddReturnValue = 1;
        private const int ValidPageNumber = 1;
        private const int ValidPageSize = 1;
        private const int ValidUserId = 1;

        private DepositService _depositService;
        private IMapper _mapper;
        private Mock<IUserService> _userServiceMock;
        private Mock<IDepositRepository> _depositRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(config => { config.AddProfile(new MapperProfile()); });
            _mapper = mapperConfig.CreateMapper();

            _depositRepositoryMock = new Mock<IDepositRepository>();
            _userServiceMock = new Mock<IUserService>();
            _userServiceMock.Setup(x => x.GetSignedInUserId()).Returns(ValidUserId);

            _depositService = new DepositService(_mapper, _depositRepositoryMock.Object, _userServiceMock.Object);
        }

        [Test]
        public async Task Calculate_SimpleInterestCalculationFormula_ExpectedResults()
        {
            var inputServiceModel = GetValidSimpleInterestCalculateDepositeView();
            var validMonthlyPayments = GetValidSimpleInterestMonthlyPayments();

            await CalculateTestForFormulaLogicAsync(inputServiceModel, validMonthlyPayments);
        }

        [Test]
        public async Task Calculate_CompoundInterestCalculationFormula_ExpectedResults()
        {
            var inputServiceModel = GetValidCompoundInterestCalculateDepositeView();
            var validMonthlyPayments = GetValidCompoundInterestMonthlyPayments();

            await CalculateTestForFormulaLogicAsync(inputServiceModel, validMonthlyPayments);
        }
        
        [Test]
        public async Task Calculate_ValidData_AddAsyncInvoked()
        {
            var inputServiceModel = GetValidCompoundInterestCalculateDepositeView();
            var validMonthlyPayments = GetValidCompoundInterestMonthlyPayments();

            Deposit deposit = null;

            _depositRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Deposit>()))
                .ReturnsAsync(DepositRepositoryAddReturnValue)
                .Callback((Deposit depHistory) => deposit = depHistory);

            await _depositService.CalculateAsync(inputServiceModel);

            deposit.Should().NotBeNull();
            deposit.MonthlyPayments.Count.Should().Be(validMonthlyPayments.Count);
        }

        [Test]
        public async Task GetAllAsync_ValidParameters_ExpectedResults()
        {
            var depositsResponseOfRepository = GetValidDepositsAndTotalCount();

            _depositRepositoryMock
              .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(depositsResponseOfRepository);

            var resPagedDeposits = await _depositService.GetAllAsync(ValidPageNumber, ValidPageSize);

            resPagedDeposits
                .Should().NotBeNull().And
                .BeOfType<PagedDataView<DepositGetAllDepositViewItem>>()
                .Which.Items.Should().NotBeNull();

             depositsResponseOfRepository.Items
                .Should().BeEquivalentTo(resPagedDeposits.Items,
                    options => options.ExcludingMissingMembers().Excluding(x => x.CalculationFormula));

            for (int i = 0; i < depositsResponseOfRepository.Items.Count; i++)
            {
                ((DepositCalculationFormulaEnumView)depositsResponseOfRepository.Items[i].CalculationFormula).GetDisplayValue()
                    .Should().Be(resPagedDeposits.Items[i].CalculationFormula);
            }
        }

        [Test]
        public void GetAllAsync_InvalidPageNumber_ThrowsException()
        {
            const int InvalidPageNumber = -1;
            var depositsResponseOfRepository = GetValidDepositsAndTotalCount();

            _depositRepositoryMock
                .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(depositsResponseOfRepository);

            FluentActions.Awaiting(() => _depositService.GetAllAsync(InvalidPageNumber, ValidPageSize))
                .Should().Throw<Exception>().WithMessage(Constants.Errors.Page.IncorrectPageNumberFormat);
        }

        [Test]
        public void GetAllAsync_InvalidPageSize_ThrowsException()
        {
            const int InvalidPageSize = -1;
            var depositsResponseOfRepository = GetValidDepositsAndTotalCount();

            _depositRepositoryMock
                .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(depositsResponseOfRepository);

            FluentActions.Awaiting(() => _depositService.GetAllAsync(ValidPageSize, InvalidPageSize))
                .Should().Throw<Exception>().WithMessage(Constants.Errors.Page.IncorrectPageSizeFormat);
        }

        [Test]
        public async Task GetByIdAsync_ValidId_ExpectedResults()
        {
            const int ValidDepositeHistoryId = 1;
            var depositsResponseOfRepository = GetValidDepositWithMonthlyPaymentItems();

            _depositRepositoryMock
                .Setup(x => x.GetDepositWithItemsByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(depositsResponseOfRepository);

            var responseGetByIdDepositView = await _depositService.GetByIdAsync(ValidDepositeHistoryId);

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
        public void GetByIdAsync_ThereIsNoSuchUser_ThrowsException()
        {
            const int ValidDepositeHistoryId = 1;
            const int InvalidUserId = -1;
            var depositsResponseOfRepository = GetValidDepositWithMonthlyPaymentItems();

            _depositRepositoryMock
                .Setup(x => x.GetDepositWithItemsByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(depositsResponseOfRepository);

            _userServiceMock.Setup(x => x.GetSignedInUserId()).Returns(InvalidUserId);

            FluentActions.Awaiting(() => _depositService.GetByIdAsync(ValidDepositeHistoryId))
               .Should().Throw<Exception>().WithMessage(Constants.Errors.Deposit.DepositDoesNotExistsOrYouHaveNoAccess);
        }

        [Test]
        public void GetByIdAsync_InvalidId_ThrowsException()
        {
            const int InvalidDepositeHistoryId = -1;

            _depositRepositoryMock
                .Setup(x => x.GetDepositWithItemsByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => { return null; });

            FluentActions.Awaiting(() => _depositService.GetByIdAsync(InvalidDepositeHistoryId))
                .Should().Throw<Exception>().WithMessage(Constants.Errors.Deposit.IncorrectDepositHistoryId);
        }

        private async Task CalculateTestForFormulaLogicAsync(CalculateDepositView calculateDepositView, IList<MonthlyPayment> monthlyPayments)
        {
            Deposit deposit = null;

            _depositRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Deposit>()))
                .ReturnsAsync(DepositRepositoryAddReturnValue)
                .Callback((Deposit depHistory) => deposit = depHistory);

            int response = await _depositService.CalculateAsync(calculateDepositView);

            response.Should().Be(DepositRepositoryAddReturnValue);
            deposit
                .Should().NotBeNull().And
                .BeEquivalentTo(calculateDepositView, options => options.ExcludingMissingMembers());
            deposit.MonthlyPayments.Should().NotBeNull().And.BeEquivalentTo(monthlyPayments);
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

        private PaginationModel<Deposit> GetValidDepositsAndTotalCount()
        {
            return new PaginationModel<Deposit>
            {
                Items = new List<Deposit>
                {
                    new Deposit {
                        Id = 1,
                        DepositSum = 100,
                        CalculationFormula = CalculationFormulaEnum.SimpleInterest,
                        Percents = 2.45f
                    },
                    new Deposit {
                        Id = 2,
                        DepositSum = 200,
                        CalculationFormula = CalculationFormulaEnum.CompoundInterest,
                        Percents = 5
                    },
                    new Deposit {
                        Id = 3,
                        DepositSum = 300,
                        CalculationFormula = CalculationFormulaEnum.SimpleInterest,
                        Percents = 6
                    }
                },
              TotalCount = 5
            };
        }

        private Deposit GetValidDepositWithMonthlyPaymentItems()
        { 
            return new Deposit
            {
                Id = 1,
                UserId = ValidUserId,
                CalculationFormula = CalculationFormulaEnum.SimpleInterest,
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
