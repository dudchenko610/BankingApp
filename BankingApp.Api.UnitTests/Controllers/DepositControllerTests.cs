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
using BankingApp.ViewModels.Banking.Deposit;

namespace BankingApp.Api.UnitTests.Controllers
{
    [TestFixture]
    public class DepositControllerTests
    {
        private const int DepositeServiceCalculateReturnValue = 1;

        [Test]
        public async Task Calculate_СorrectInputData_ReturnsOkResultAndBankingServiceReceivesValidModel()
        {
            var validCalculateDepositViw = GetValidCalculateDepositView();
            CalculateDepositView inputModelOfCalculateDepositeMethod = null;

            var depositServiceMock = new Mock<IDepositService>();
            depositServiceMock
                .Setup(x => x.CalculateAsync(It.IsAny<CalculateDepositView>()))
                .ReturnsAsync(DepositeServiceCalculateReturnValue)
                .Callback((CalculateDepositView x) => inputModelOfCalculateDepositeMethod = x);
            var depositController = new DepositController(depositServiceMock.Object);

            var controllerResult = await depositController.Calculate(validCalculateDepositViw);

            inputModelOfCalculateDepositeMethod
                .Should().NotBeNull()
                .And.BeEquivalentTo(validCalculateDepositViw);

            var resultOfOkObjectResultValidation = controllerResult.Should().NotBeNull().And.BeOfType<OkObjectResult>();
            resultOfOkObjectResultValidation.Which.Value.Should().BeOfType<int>().And.Be(DepositeServiceCalculateReturnValue);
            resultOfOkObjectResultValidation.Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Test]
        public async Task GetById_ValidDepositIdPassed_ReturnsValidResultFromService()
        {
            const int DepositeHistoryId = 1;

            var getByIdDepositeViewResponseFromService = GetValidGetByIdDepositView();
            int inputOfBankingServiceDepositeHistoryId = -1;

            var depositServiceMock = new Mock<IDepositService>();
            depositServiceMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(getByIdDepositeViewResponseFromService)
                .Callback((int depositeHistoryId) => inputOfBankingServiceDepositeHistoryId = depositeHistoryId);
            var depositController = new DepositController(depositServiceMock.Object);

            var controllerResult = await depositController.GetById(DepositeHistoryId);

            inputOfBankingServiceDepositeHistoryId.Should().Be(DepositeHistoryId);

            var okResult = controllerResult as ObjectResult;
            var resultOfOkObjectResultValidation = okResult.Should().NotBeNull().And.BeOfType<OkObjectResult>();
            resultOfOkObjectResultValidation.Which.Value.Should().BeOfType<GetByIdDepositView>();
            resultOfOkObjectResultValidation.Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            okResult.Value.Should().BeEquivalentTo(getByIdDepositeViewResponseFromService);
        }

        [Test]
        public async Task GetAll_CallGetAllMethod_ReturnsNotNullModelWithNotNullMemberList()
        {
            var getAllDepositViewResponseFromService = GetValidGetAllDepositViewList();
            var depositServiceMock = new Mock<IDepositService>();
            depositServiceMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(getAllDepositViewResponseFromService);

            var depositController = new DepositController(depositServiceMock.Object);

            var controllerResult = await depositController.GetAll();
            var okResult = controllerResult as ObjectResult;

            var resultOfOkObjectResultValidation = okResult.Should().NotBeNull().And.BeOfType<OkObjectResult>();
            resultOfOkObjectResultValidation.Which.Value.Should().BeOfType<GetAllDepositView>();
            resultOfOkObjectResultValidation.Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            okResult.Value.Should().NotBeNull().And.BeEquivalentTo(getAllDepositViewResponseFromService);
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

        private GetAllDepositView GetValidGetAllDepositViewList()
        {
            return new GetAllDepositView
            {
                DepositItems =
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
                    },
                }
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
