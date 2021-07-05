﻿using BankingApp.Api.Controllers;
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.ViewModels.Banking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using FluentAssertions;
using BankingApp.ViewModels.Enums;
using System.Threading.Tasks;

namespace BankingApp.Api.UnitTests.Controllers
{
    [TestFixture]
    public class BankingControllerTests
    {
        private BankingController _bankingController;

        [SetUp]
        public void SetUp()
        {
            var bankingServiceMock = new Mock<IBankingCalculationService>();
            bankingServiceMock
                .Setup(bsm => bsm.CalculateDeposite(It.IsAny<RequestCalculateDepositeBankingView>()))
                .Returns(new ResponseCalculateDepositeBankingView());

            _bankingController = new BankingController(bankingServiceMock.Object, null);
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
            whereAndConstr.Which.Value.Should().BeOfType<ResponseCalculateDepositeBankingView>();
            whereAndConstr.Which.StatusCode.Should().Equals(StatusCodes.Status200OK);
        }
    }
}
