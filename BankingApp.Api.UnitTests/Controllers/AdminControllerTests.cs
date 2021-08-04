using BankingApp.Api.Controllers;
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.ViewModels.Banking.Admin;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace BankingApp.Api.UnitTests.Controllers
{
    [TestFixture]
    public class AdminControllerTests
    {
        private Mock<IUserService> _userServiceMock;

        [SetUp]
        public void SetUp()
        {
            _userServiceMock = new Mock<IUserService>();
        }

        [Test]
        public async Task GetAll_СorrectInputData_ReturnsOkObjectResult()
        {
            const int ValidPageNumber = 1;
            const int ValidPageSize = 1;

            _userServiceMock.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()));

            var adminController = new AdminController(_userServiceMock.Object);

            var controllerResult = await adminController.GetAll(ValidPageNumber, ValidPageSize);
            controllerResult.Should().NotBeNull().And.BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Test]
        public async Task BlockUser_СorrectInputData_ReturnsOkResult()
        {
            var validBlockUserView = GetValidBlockUserAdminView();
            BlockUserAdminView inputOfblockAsyncMethod = null;

            _userServiceMock.Setup(x => x.BlockAsync(It.IsAny<BlockUserAdminView>()))
                .Callback((BlockUserAdminView x) => { inputOfblockAsyncMethod = x; });

            var adminController = new AdminController(_userServiceMock.Object);

            var controllerResult = await adminController.BlockUser(validBlockUserView);
            controllerResult.Should().NotBeNull().And.BeOfType<OkResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            inputOfblockAsyncMethod.Should().NotBeNull().And.BeEquivalentTo(validBlockUserView);
        }

        private BlockUserAdminView GetValidBlockUserAdminView()
        {
            return new BlockUserAdminView
            {
                UserId = 1,
                Block = false
            };
        }
    }
}
