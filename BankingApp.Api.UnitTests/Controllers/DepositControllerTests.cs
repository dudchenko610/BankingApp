using BankingApp.Api.Controllers;
using BankingApp.BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using BankingApp.ViewModels.Enums;
using System.Threading.Tasks;
using System.Collections.Generic;
using NUnit.Framework;
using BankingApp.ViewModels.ViewModels.Deposit;
using BankingApp.ViewModels.ViewModels.Pagination;

namespace BankingApp.Api.UnitTests.Controllers
{
    [TestFixture]
    public class DepositControllerTests
    {
        private const int DepositeServiceCalculateReturnValue = 1;
        private const int ValidPageNumber = 1;
        private const int ValidPageSize = 1;

        private DepositController _depositController;
        private Mock<IDepositService> _depositServiceMock;

        [SetUp]
        public void SetUp()
        {
            _depositServiceMock = new Mock<IDepositService>();
            _depositController = new DepositController(_depositServiceMock.Object);
        }

        [Test]
        public async Task Calculate_СorrectInputData_ExpectedResults()
        {
            var validCalculateDepositViw = GetValidCalculateDepositView();
            CalculateDepositView inputModelOfCalculateDepositeMethod = null;

            _depositServiceMock
                .Setup(x => x.CalculateAsync(It.IsAny<CalculateDepositView>()))
                .ReturnsAsync(DepositeServiceCalculateReturnValue)
                .Callback((CalculateDepositView x) => inputModelOfCalculateDepositeMethod = x);

            var controllerResult = await _depositController.Calculate(validCalculateDepositViw);

            inputModelOfCalculateDepositeMethod
                .Should().NotBeNull()
                .And.BeEquivalentTo(validCalculateDepositViw);

            var resultOfOkObjectResultValidation = controllerResult.Should().NotBeNull().And.BeOfType<OkObjectResult>();
            resultOfOkObjectResultValidation.Which.Value.Should().BeOfType<int>().And.Be(DepositeServiceCalculateReturnValue);
            resultOfOkObjectResultValidation.Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Test]
        public async Task GetById_СorrectInputData_ExpectedResults()
        {
            const int DepositeHistoryId = 1;

            var getByIdDepositeViewResponseFromService = GetValidGetByIdDepositView();
            int inputOfBankingServiceDepositeHistoryId = -1;

            _depositServiceMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(getByIdDepositeViewResponseFromService)
                .Callback((int depositeHistoryId) => inputOfBankingServiceDepositeHistoryId = depositeHistoryId);

            var controllerResult = await _depositController.GetById(DepositeHistoryId);

            inputOfBankingServiceDepositeHistoryId.Should().Be(DepositeHistoryId);

            var okResult = controllerResult as ObjectResult;
            var resultOfOkObjectResultValidation = okResult.Should().NotBeNull().And.BeOfType<OkObjectResult>();
            resultOfOkObjectResultValidation.Which.Value.Should().BeOfType<GetByIdDepositView>();
            resultOfOkObjectResultValidation.Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            okResult.Value.Should().BeEquivalentTo(getByIdDepositeViewResponseFromService);
        }

        [Test]
        public async Task GetAll_СorrectInputData_ExpectedResults()
        {
            var getPagedAllDepositViewItemResponseFromService = GetValidPagedGetAllDepositViewItemList();

            _depositServiceMock
                .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(getPagedAllDepositViewItemResponseFromService);

            var controllerResult = await _depositController.GetAll(ValidPageNumber, ValidPageSize);
            var okResult = controllerResult as ObjectResult;

            var resultOfOkObjectResultValidation = okResult.Should().NotBeNull().And.BeOfType<OkObjectResult>();
            resultOfOkObjectResultValidation.Which.Value.Should().BeOfType<PagedDataView<DepositGetAllDepositViewItem>>();
            resultOfOkObjectResultValidation.Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            okResult.Value.Should().NotBeNull().And.BeEquivalentTo(getPagedAllDepositViewItemResponseFromService);
        }

        [Test]
        public async Task GetAll_СorrectInputData_GetAllAsyncInvoked()
        {
            int passedPageNumber = -1;
            int passedPageSize = -1;

            _depositServiceMock
                .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                .Callback(
                    (int pageNumber, int pageSize) => 
                    {
                        passedPageNumber = pageNumber;
                        passedPageSize = pageSize;
                    }
                );

            await _depositController.GetAll(ValidPageNumber, ValidPageSize);

            passedPageNumber.Should().Be(ValidPageNumber);
            passedPageSize.Should().Be(ValidPageSize);
        }

        private CalculateDepositView GetValidCalculateDepositView()
        { 
            return new CalculateDepositView
            {
                DepositSum = 100,
                MonthsCount = 12,
                Percents = 10,
                CalculationFormula = DepositCalculationFormulaEnumView.CompoundInterest
            };
        }

        private PagedDataView<DepositGetAllDepositViewItem> GetValidPagedGetAllDepositViewItemList()
        {
            return new PagedDataView<DepositGetAllDepositViewItem>
            {
                Items = new List<DepositGetAllDepositViewItem>
                {
                    new DepositGetAllDepositViewItem
                    {
                        Id = 1,
                        Percents = 2.4f,
                        DepositSum = 100m,
                        MonthsCount = 2,
                        CalсulationDateTime = System.DateTime.Now,
                        CalculationFormula = "some formula"
                    },
                    new DepositGetAllDepositViewItem
                    {
                        Id = 2,
                        Percents = 5.4f,
                        DepositSum = 200m,
                        MonthsCount = 4,
                        CalсulationDateTime = System.DateTime.Now,
                        CalculationFormula = "some formula"
                    },
                    new DepositGetAllDepositViewItem
                    {
                        Id = 5,
                        Percents = 10.4f,
                        DepositSum = 100m,
                        MonthsCount = 2,
                        CalсulationDateTime = System.DateTime.Now,
                        CalculationFormula = "some formula"
                    }
                },
                PageNumber = 1,
                PageSize = 1,
                TotalItems = 3
            };
        }

        private GetByIdDepositView GetValidGetByIdDepositView()
        {
            return new GetByIdDepositView
            {
                MonthlyPaymentItems = new List<MonthlyPaymentGetByIdDepositViewItem>
                {
                    new MonthlyPaymentGetByIdDepositViewItem(),
                    new MonthlyPaymentGetByIdDepositViewItem(),
                    new MonthlyPaymentGetByIdDepositViewItem(),
                    new MonthlyPaymentGetByIdDepositViewItem(),
                    new MonthlyPaymentGetByIdDepositViewItem()
                }
            };
        }
    }
}
