using BankingApp.Api.Controllers;
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.ViewModels.Banking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace BankingApp.Tests.UnitTests
{
    [TestFixture]
    public class BankingControllerTest
    {
        private BankingController _bankingController;

        [SetUp]
        public void SetUp()
        {
            var bankingServiceMock = new Mock<IBankingService>();
            bankingServiceMock
                .Setup(bsm => bsm.CalculateDeposite(It.IsAny<RequestCalculateDepositeBankingView>()))
                .Returns(new ResponseCalculateDepositeBankingView());

            _bankingController = new BankingController(bankingServiceMock.Object);
        }

        [Test]
        public void CalcaulateDeposite_Returns_ValidData()
        {
            var input = new RequestCalculateDepositeBankingView { DepositeSum = 0, MonthsCount = 0, Percents = 0 };

            var controllerResult = _bankingController.CalculateDeposite(input);
            var okResult = controllerResult as ObjectResult;

            Assert.NotNull(okResult);
            Assert.True(okResult is OkObjectResult);
            Assert.AreEqual(okResult.Value.GetType(), typeof(ResponseCalculateDepositeBankingView));
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        }
    }
}
