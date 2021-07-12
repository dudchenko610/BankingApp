using BankingApp.Api.Controllers;
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.ViewModels.Banking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using FluentAssertions;
using BankingApp.ViewModels.Enums;
using System.Threading.Tasks;
using BankingApp.ViewModels.Banking.History;
using AutoMapper;
using BankingApp.BusinessLogicLayer.Mapper;
using System.Collections.Generic;

namespace BankingApp.Api.UnitTests.Controllers
{
    [TestFixture]
    public class BankingControllerTests
    {
        private readonly ResponseCalculationHistoryBankingViewItem _detailsHistoryResponseData =
            new ResponseCalculationHistoryBankingViewItem
            {
                DepositePerMonthInfo = new List<ResponseCalculateDepositeBankingViewItem>
                {
                    new ResponseCalculateDepositeBankingViewItem(),
                    new ResponseCalculateDepositeBankingViewItem(),
                    new ResponseCalculateDepositeBankingViewItem(),
                    new ResponseCalculateDepositeBankingViewItem(),
                    new ResponseCalculateDepositeBankingViewItem()
                }
            };

        private BankingController _bankingController;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var bankingCalcukationServiceMock = new Mock<IBankingService>();
            bankingCalcukationServiceMock
                .Setup(bsm => bsm.CalculateDeposite(It.IsAny<RequestCalculateDepositeBankingView>()))
                .Returns(new ResponseCalculateDepositeBankingView());

            var bankingHistoryServiceMock = new Mock<IBankingHistoryService>();
            bankingHistoryServiceMock
                .Setup(bhs => bhs.GetDepositeCalculationHistoryDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(new ResponseCalculationHistoryBankingViewItem());

            bankingHistoryServiceMock
                .Setup(
                    bhs => bhs.SaveDepositeCalculationAsync(It.IsAny<RequestCalculateDepositeBankingView>(),
                                It.IsAny<ResponseCalculateDepositeBankingView>())
                )
                .ReturnsAsync(0);


            bankingHistoryServiceMock
                .Setup(bhs => bhs.GetDepositeCalculationHistoryDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(_detailsHistoryResponseData);

            bankingHistoryServiceMock
                .Setup(bhs => bhs.GetDepositesCalculationHistoryAsync())
                .ReturnsAsync(new ResponseCalculationHistoryBankingView());


            _bankingController = new BankingController(bankingCalcukationServiceMock.Object, bankingHistoryServiceMock.Object);

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new DepositeHistoryProfile());
                config.AddProfile(new DepositeHistoryItemProfile());
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Test]
        public async Task CalcaulateDeposite_СorrectInputData_ReturnsOkResult()
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
            whereAndConstr.Which.Value.Should().BeOfType<ResponseCalculationHistoryBankingViewItem>();
            whereAndConstr.Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            var payload = (ResponseCalculationHistoryBankingViewItem)okResult.Value;
            payload.DepositePerMonthInfo.Count.Should().Be(_detailsHistoryResponseData.DepositePerMonthInfo.Count);
        }
    }
}
