﻿using BankingApp.BusinessLogicLayer.Services;
using BankingApp.Entities.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;

namespace BankingApp.BusinessLogicLayer.UnitTests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private const string ValidUserId = "1";
        private const string InvalidUserId = "dds_123ds";

        private UserManager<User> _userManager;

        [SetUp]
        public void SetUp()
        {
            var validUser = GetValidUser();

            var store = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(validUser);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);

            _userManager = userManagerMock.Object;
        }

        [Test]
        public void GetSignedInUserId_InvalidHttpContextAccessorInjected_ReturnsMinusOne()
        {
            const int MinusOne = -1;

            var validHttpContextAccessor = GetMockedHttpContextAccessor(InvalidUserId);
            var userService = new UserService(validHttpContextAccessor, _userManager);

            int userId = userService.GetSignedInUserId();
            userId.Should().Be(MinusOne);
        }

        [Test]
        public void GetSignedInUserId_ValidHttpContextAccessorInjected_ReturnsValidId()
        {
            var validHttpContextAccessor = GetMockedHttpContextAccessor(ValidUserId);
            var userService = new UserService(validHttpContextAccessor, _userManager);

            int userId = userService.GetSignedInUserId();
            userId.Should().Be(int.Parse(ValidUserId));
        }

        [Test]
        public async Task GetUserByEmail_ValidEmailPassed_ReturnsValidUser()
        {
            const string ValidEmail = "a@a.com";

            var validHttpContextAccessor = GetMockedHttpContextAccessor(ValidUserId);
            var userService = new UserService(validHttpContextAccessor, _userManager);

            var userByEmail = await userService.GetUserByEmailAsync(ValidEmail);
            var userReturnedByUserManager = GetValidUser();

            userByEmail.Id.Should().Be(userReturnedByUserManager.Id);
            userByEmail.Email.Should().Be(userReturnedByUserManager.Email);
            userByEmail.UserName.Should().Be(userReturnedByUserManager.UserName);
            userByEmail.Deposits.Should().BeEquivalentTo(userReturnedByUserManager.Deposits);
        }

        [Test]
        public async Task GetUserByEmail_InvalidEmailPassed_ReturnsNull()
        {
            const string InvalidEmail = "aacom";

            var validHttpContextAccessor = GetMockedHttpContextAccessor(ValidUserId);
            var store = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User) null);

            var userService = new UserService(validHttpContextAccessor, userManagerMock.Object);
            var userByEmail = await userService.GetUserByEmailAsync(InvalidEmail);
            userByEmail.Should().BeNull();
        }

        private User GetValidUser()
        {
            return new User
            {
                Id = 1,
                Email = "a@a.com",
                UserName = "Me",
                EmailConfirmed = true,
                Deposits = new List<Deposit>()
            };
        }

        private IHttpContextAccessor GetMockedHttpContextAccessor(string userId)
        {
            var claimsPrincipialMock = new Mock<ClaimsPrincipal>();
            claimsPrincipialMock.Setup(x => x.FindFirst(It.IsAny<string>())).Returns(new Claim(string.Empty, userId));

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.User).Returns(claimsPrincipialMock.Object);

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);

            return httpContextAccessorMock.Object;
        }
    }
}
