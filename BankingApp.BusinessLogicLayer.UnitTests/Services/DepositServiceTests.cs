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
using BankingApp.DataAccessLayer.Models;
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.ViewModels.ViewModels.Deposit;
using BankingApp.ViewModels.ViewModels.Pagination;

namespace BankingApp.BusinessLogicLayer.UnitTests.Services
{
    [TestFixture]
    public class DepositServiceTests
    {
        private const int DepositRepositoryAddReturnValue = 1;
        private const int ValidPageNumber = 1;
        private const int ValidPageSize = 1;
        private const int ValidUserId = 1;

        private IMapper _mapper;
        private IUserService _userService;

        [SetUp]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(config => { config.AddProfile(new MapperProfile()); });
            _mapper = mapperConfig.CreateMapper();

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetSignedInUserId()).Returns(ValidUserId);
            _userService = userServiceMock.Object;
        }

        [Test]
        public async Task Calculate_SimpleInterestCalculationFormula_ReturnsExpectedIdAndMappingHappensCorrectly()
        {
            var inputServiceModel = GetValidSimpleInterestCalculateDepositeView();
            var validMonthlyPayments = GetValidSimpleInterestMonthlyPayments();

            await CalculateTestForFormulaLogicAsync(inputServiceModel, validMonthlyPayments);
        }

        [Test]
        public async Task Calculate_CompoundInterestCalculationFormula_ReturnsExpectedIdAndMappingHappensCorrectly()
        {
            var inputServiceModel = GetValidCompoundInterestCalculateDepositeView();
            var validMonthlyPayments = GetValidCompoundInterestMonthlyPayments();

            await CalculateTestForFormulaLogicAsync(inputServiceModel, validMonthlyPayments);
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

            DepositService depositService = new DepositService(_mapper, depositRepositoryMock.Object, _userService);
            await depositService.CalculateAsync(inputServiceModel);

            deposit.Should().NotBeNull();
            deposit.MonthlyPayments.Count.Should().Be(validMonthlyPayments.Count);
        }

        [Test]
        public async Task GetAllAsync_CallGetAllMethodPassingValidParameters_ReturnsNotNullModelContainingCorrectlyMappedList()
        {
            var depositsResponseOfRepository = GetValidDepositsAndTotalCount();

            var depositRepositoryMock = new Mock<IDepositRepository>();
            depositRepositoryMock
              .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(depositsResponseOfRepository);

            DepositService depositService = new DepositService(_mapper, depositRepositoryMock.Object, _userService);
            var resPagedDeposits = await depositService.GetAllAsync(ValidPageNumber, ValidPageSize);

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
        public void GetAllAsync_CallGetAllMethodPassingInvalidPageNumber_ThrowsExceptionWithCorrespondingMessage()
        {
            const int InvalidPageNumber = -1;
            var depositsResponseOfRepository = GetValidDepositsAndTotalCount();

            var depositRepositoryMock = new Mock<IDepositRepository>();
            depositRepositoryMock
                .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(depositsResponseOfRepository);

            DepositService depositService = new DepositService(_mapper, depositRepositoryMock.Object, _userService);
            
            FluentActions.Awaiting(() => depositService.GetAllAsync(InvalidPageNumber, ValidPageSize))
                .Should().Throw<Exception>().WithMessage(Constants.Errors.Page.IncorrectPageNumberFormat);
        }

        [Test]
        public void GetAllAsync_CallGetAllMethodPassingInvalidPageSize_ThrowsExceptionWithCorrespondingMessage()
        {
            const int InvalidPageSize = -1;
            var depositsResponseOfRepository = GetValidDepositsAndTotalCount();

            var depositRepositoryMock = new Mock<IDepositRepository>();
            depositRepositoryMock
                .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(depositsResponseOfRepository);

            DepositService depositService = new DepositService(_mapper, depositRepositoryMock.Object, _userService);

            FluentActions.Awaiting(() => depositService.GetAllAsync(ValidPageSize, InvalidPageSize))
                .Should().Throw<Exception>().WithMessage(Constants.Errors.Page.IncorrectPageSizeFormat);
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

            DepositService depositService = new DepositService(_mapper, depositRepositoryMock.Object, _userService);

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
        public void GetByIdAsync_CallGetByIdMethodPassingValidIdAndUserServiceReturnsInvalidUserId_ThrowsExceptionWithCorrespondingMessage()
        {
            const int ValidDepositeHistoryId = 1;
            const int InvalidUserId = -1;
            var depositsResponseOfRepository = GetValidDepositWithMonthlyPaymentItems();

            var depositRepositoryMock = new Mock<IDepositRepository>();
            depositRepositoryMock
                .Setup(x => x.GetDepositWithItemsByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(depositsResponseOfRepository);

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetSignedInUserId()).Returns(InvalidUserId);

            DepositService depositService = new DepositService(_mapper, depositRepositoryMock.Object, userServiceMock.Object);

            FluentActions.Awaiting(() => depositService.GetByIdAsync(ValidDepositeHistoryId))
               .Should().Throw<Exception>().WithMessage(Constants.Errors.Deposit.DepositDoesNotExistsOrYouHaveNoAccess);
        }

        [Test]
        public void GetByIdAsync_CallGetByIdMethodPassingInvalidId_ThrowsExceptionWithCorrespondingMessage()
        {
            const int InvalidDepositeHistoryId = -1;

            var depositRepositoryMock = new Mock<IDepositRepository>();
            depositRepositoryMock
                .Setup(x => x.GetDepositWithItemsByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => { return null; });

            DepositService depositService = new DepositService(_mapper, depositRepositoryMock.Object, _userService);

            FluentActions.Awaiting(() => depositService.GetByIdAsync(InvalidDepositeHistoryId))
                .Should().Throw<Exception>().WithMessage(Constants.Errors.Deposit.IncorrectDepositeHistoryId);
        }

        private async Task CalculateTestForFormulaLogicAsync(CalculateDepositView calculateDepositView, IList<MonthlyPayment> monthlyPayments)
        {
            Deposit deposit = null;

            var depositRepositoryMock = new Mock<IDepositRepository>();
            depositRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Deposit>()))
                .ReturnsAsync(DepositRepositoryAddReturnValue)
                .Callback((Deposit depHistory) => deposit = depHistory);

            var userServiceMock = new Mock<IUserService>();
            DepositService depositService = new DepositService(_mapper, depositRepositoryMock.Object, userServiceMock.Object);
            int response = await depositService.CalculateAsync(calculateDepositView);

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
