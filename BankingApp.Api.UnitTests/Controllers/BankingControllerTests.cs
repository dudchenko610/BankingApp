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

namespace BankingApp.Api.UnitTests.Controllers
{
    [TestFixture]
    public class BankingControllerTests
    {
        private const int BankingServiceCalculateDepositeAsyncReturnValue = 1;

        [Test]
        public async Task CalculateDeposite_СorrectInputData_ReturnsOkResultAndBankingServiceReceivesValidModel()
        {
            CalculateDepositeBankingView inputModelOfCalculateDepositeMethod = null;

            var bankingServiceMock = new Mock<IBankingService>();
            bankingServiceMock
                .Setup(x => x.CalculateDepositeAsync(It.IsAny<CalculateDepositeBankingView>()))
                .ReturnsAsync(BankingServiceCalculateDepositeAsyncReturnValue)
                .Callback((CalculateDepositeBankingView x) => inputModelOfCalculateDepositeMethod = x);
            var bankingController = new BankingController(bankingServiceMock.Object);

            var controllerResult = await bankingController.CalculateDeposite(GetValidCalculateDepositeRequest());

            inputModelOfCalculateDepositeMethod
                .Should().NotBeNull()
                .And.BeEquivalentTo(GetValidCalculateDepositeRequest());

            var resultOfOkObjectResultValidation = controllerResult.Should().NotBeNull().And.BeOfType<OkObjectResult>();
            resultOfOkObjectResultValidation.Which.Value.Should().BeOfType<int>().And.Be(BankingServiceCalculateDepositeAsyncReturnValue);
            resultOfOkObjectResultValidation.Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Test]
        public async Task CalculationHistoryDetails_ValidDepositeHistoryIdPassed_ReturnsObjectThatReturnsService()
        {
            const int DepositeHistoryId = 1;
            int inputOfBankingServiceDepositeHistoryId = -1;

            var bankingServiceMock = new Mock<IBankingService>();
            bankingServiceMock
                .Setup(x => x.GetDepositeCalculationHistoryDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(GetValidCalculationHistoryDetails())
                .Callback((int depositeHistoryId) => inputOfBankingServiceDepositeHistoryId = depositeHistoryId);
            var bankingController = new BankingController(bankingServiceMock.Object);

            var controllerResult = await bankingController.CalculationHistoryDetails(DepositeHistoryId);

            inputOfBankingServiceDepositeHistoryId.Should().Be(DepositeHistoryId);

            var okResult = controllerResult as ObjectResult;
            var resultOfOkObjectResultValidation = okResult.Should().NotBeNull().And.BeOfType<OkObjectResult>();
            resultOfOkObjectResultValidation.Which.Value.Should().BeOfType<ResponseCalculationHistoryDetailsBankingView>();
            resultOfOkObjectResultValidation.Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            okResult.Value.Should().BeEquivalentTo(GetValidCalculationHistoryDetails());
        }

        [Test]
        public async Task CalculationHistory_CallCalculationHistoryMethod_ReturnsNotNullNodelWithNotNullMemberList()
        {
            var bankingServiceMock = new Mock<IBankingService>();
            bankingServiceMock
                .Setup(x => x.GetDepositesCalculationHistoryAsync())
                .ReturnsAsync(GetValidResponseCalculationHistory());

            var bankingController = new BankingController(bankingServiceMock.Object);

            var controllerResult = await bankingController.CalculationHistory();
            var okResult = controllerResult as ObjectResult;

            var resultOfOkObjectResultValidation = okResult.Should().NotBeNull().And.BeOfType<OkObjectResult>();
            resultOfOkObjectResultValidation.Which.Value.Should().BeOfType<ResponseCalculationHistoryDetailsBankingView>();
            resultOfOkObjectResultValidation.Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            okResult.Value.Should().NotBeNull().And.BeEquivalentTo(GetValidResponseCalculationHistory());
        }

        private CalculateDepositeBankingView GetValidCalculateDepositeRequest()
        { 
            return new CalculateDepositeBankingView
            {
                DepositeSum = 100,
                MonthsCount = 12,
                Percents = 10,
                CalculationFormula = DepositeCalculationFormulaEnumView.CompoundInterest
            };
        }

        private ResponseCalculationHistoryBankingView GetValidResponseCalculationHistory()
        {
            return new ResponseCalculationHistoryBankingView
            {
                DepositesHistory =
                {
                    new DepositeInfoResponseCalculationHistoryBankingViewItem
                    {
                        Id = 1,
                        Percents = 2.4f,
                        DepositeSum = 100m,
                        MonthsCount = 2,
                        CalulationDateTime = System.DateTime.Now,
                        CalculationFormula = "some formula"
                    },
                    new DepositeInfoResponseCalculationHistoryBankingViewItem
                    {
                        Id = 2,
                        Percents = 5.4f,
                        DepositeSum = 200m,
                        MonthsCount = 4,
                        CalulationDateTime = System.DateTime.Now,
                        CalculationFormula = "some formula"
                    },
                    new DepositeInfoResponseCalculationHistoryBankingViewItem
                    {
                        Id = 5,
                        Percents = 10.4f,
                        DepositeSum = 100m,
                        MonthsCount = 2,
                        CalulationDateTime = System.DateTime.Now,
                        CalculationFormula = "some formula"
                    },
                }
            };
        }

        private ResponseCalculationHistoryDetailsBankingView GetValidCalculationHistoryDetails()
        {
            return new ResponseCalculationHistoryDetailsBankingView
            {
                DepositePerMonthInfo = new List<MonthlyPaymentResponseCalculationHistoryDetailsBankingViewItem>
                {
                    new MonthlyPaymentResponseCalculationHistoryDetailsBankingViewItem(),
                    new MonthlyPaymentResponseCalculationHistoryDetailsBankingViewItem(),
                    new MonthlyPaymentResponseCalculationHistoryDetailsBankingViewItem(),
                    new MonthlyPaymentResponseCalculationHistoryDetailsBankingViewItem(),
                    new MonthlyPaymentResponseCalculationHistoryDetailsBankingViewItem()
                }
            };
        }
    }
}
