using BankingApp.Api.Controllers;
using BankingApp.BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using BankingApp.ViewModels.Enums;
using System.Threading.Tasks;
using BankingApp.ViewModels.Banking.History;
using System.Collections.Generic;
using BankingApp.ViewModels.Banking.Calculate;
using NUnit.Framework;
using BankingApp.ViewModels.Pagination;
using System;
using BankingApp.Shared;

namespace BankingApp.Api.UnitTests.Controllers
{
    [TestFixture]
    public class BankingControllerTests
    {
        private const int BankingServiceCalculateDepositeAsyncReturnValue = 1;
        private BankingController _bankingController;

        [SetUp]
        public void SetUp()
        {
            var bankingServiceMock = new Mock<IBankingService>();
            bankingServiceMock
                .Setup(bsm => bsm.CalculateDepositeAsync(It.IsAny<RequestCalculateDepositeBankingView>()))
                .ReturnsAsync(BankingServiceCalculateDepositeAsyncReturnValue);

            bankingServiceMock
                .Setup(bhs => bhs.GetDepositeCalculationHistoryDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(GetTestDetailsHistoryResponseData());

            bankingServiceMock
                .Setup(bhs => bhs.GetDepositesCalculationHistoryAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new ResponsePagedDataView<ResponseCalculationHistoryBankingViewItem>());
            
            _bankingController = new BankingController(bankingServiceMock.Object);
        }

        [Test]
        public async Task CalculateDeposite_СorrectInputData_ReturnsOkResult()
        {
            var input = new RequestCalculateDepositeBankingView
            {
                DepositeSum = 100,
                MonthsCount = 12,
                Percents = 10,
                CalculationFormula = DepositeCalculationFormulaEnumView.CompoundInterest
            };

            var controllerResult = await _bankingController.CalculateDeposite(input);
            var okResult = controllerResult as ObjectResult;

            var whereAndConstr = okResult.Should().NotBeNull().And.BeOfType<OkObjectResult>();
            whereAndConstr.Which.Value.Should().BeOfType<int>();
            whereAndConstr.Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Test]
        public async Task CalculationHistoryDetails_DepositeHistoryIdPassed_ReturnsListWithValidSize()
        {
            const int DepositeHistoryId = 1;
            var controllerResult = await _bankingController.CalculationHistoryDetails(DepositeHistoryId);

            var okResult = controllerResult as ObjectResult;

            var whereAndConstr = okResult.Should().NotBeNull().And.BeOfType<OkObjectResult>();
            whereAndConstr.Which.Value.Should().BeOfType<ResponseCalculationHistoryDetailsBankingView>();
            whereAndConstr.Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            var payload = (ResponseCalculationHistoryDetailsBankingView)okResult.Value;
            payload.DepositePerMonthInfo.Count.Should().Be(GetTestDetailsHistoryResponseData().DepositePerMonthInfo.Count);
        }

        [Test]
        public async Task CalculationHistory_CallCalculationHistoryMethod_ReturnsNotNullNodelWithNotNullMemberList()
        {
            const int PageNumber = 1;
            const int PageSize = 1;

            var controllerResult = await _bankingController.CalculationHistory(PageNumber, PageSize);
            var okResult = controllerResult as ObjectResult;

            var whereAndConstr = okResult.Should().NotBeNull().And.BeOfType<OkObjectResult>();
            whereAndConstr.Which.Value.Should().BeOfType<ResponseCalculationHistoryDetailsBankingView>();
            whereAndConstr.Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            var payload = (ResponsePagedDataView<ResponseCalculationHistoryBankingViewItem>)okResult.Value;
            payload.Should().NotBeNull().And
                .BeOfType<ResponsePagedDataView<ResponseCalculationHistoryBankingViewItem>>()
                .Which.Data.Should().NotBeNull();
        }
        
        private ResponseCalculationHistoryDetailsBankingView GetTestDetailsHistoryResponseData()
        {
            return new ResponseCalculationHistoryDetailsBankingView
            {
                DepositePerMonthInfo = new List<ResponseCalculationHistoryDetailsBankingViewItem>
                {
                    new ResponseCalculationHistoryDetailsBankingViewItem(),
                    new ResponseCalculationHistoryDetailsBankingViewItem(),
                    new ResponseCalculationHistoryDetailsBankingViewItem(),
                    new ResponseCalculationHistoryDetailsBankingViewItem(),
                    new ResponseCalculationHistoryDetailsBankingViewItem()
                }
            };
        }
    }
}
