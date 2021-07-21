using AutoMapper;
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.BusinessLogicLayer.Mapper;
using BankingApp.BusinessLogicLayer.Services;
using BankingApp.Entities.Entities;
using BankingApp.Shared;
using BankingApp.Shared.Options;
using BankingApp.ViewModels.Banking.Authentication;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.UnitTests.Services
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private IUserStore<User> _userStore;
        private UserManager<User> _userManager;
        private IEmailProvider _emailProvider;
        private IMapper _mapper;
        private IJwtProvider _jwtProvider;
        private IUserService _userService;
        private IOptions<ClientConnectionOptions> _clientConnectionOptions;

        [SetUp]
        public void SetUp()
        {
            // mapper
            var mapperConfig = new MapperConfiguration(config => { config.AddProfile(new MapperProfile()); });
            _mapper = mapperConfig.CreateMapper();

            // user store
            var store = new Mock<IUserStore<User>>();
            _userStore = store.Object;

            // user manager
            var userManagerMock = new Mock<UserManager<User>>(_userStore, null, null, null, null, null, null, null, null);
            _userManager = userManagerMock.Object;

            // email provider
            var emailProvider = new Mock<IEmailProvider>();
            _emailProvider = emailProvider.Object;

            // jwt provider
            var jwtProviderMock = new Mock<IJwtProvider>();
            _jwtProvider = jwtProviderMock.Object;

            // user service
            var userServiceMock = new Mock<IUserService>();
            _userService = userServiceMock.Object;

            // client connection options
            var clientConnectionOptionsMock = new Mock<IOptions<ClientConnectionOptions>>();
            clientConnectionOptionsMock.Setup(x => x.Value).Returns(new ClientConnectionOptions());
            _clientConnectionOptions = clientConnectionOptionsMock.Object;
        }

        [Test]
        public async Task ConfirmEmail_PassedValidModel_CallsUpdateAsyncMethodOfUserManagerPassingInParameterReturnedByFindByEmailAsyncMethod()
        {
            var validUser = GetValidUser();
            User inputOfUserManagerUpdateAsync = null;

            var userManagerMock = new Mock<UserManager<User>>(_userStore, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);
            userManagerMock.Setup(x => x.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .Callback((User user) => { inputOfUserManagerUpdateAsync = user; })
                .ReturnsAsync(IdentityResult.Success);

            var authenticationService = new AuthenticationService(userManagerMock.Object, _emailProvider, _mapper, _jwtProvider, _userService, _clientConnectionOptions);

            var validConfirmEmailView = GetValidConfirmEmailView();
            await authenticationService.ConfirmEmailAsync(validConfirmEmailView);

            inputOfUserManagerUpdateAsync.Should().NotBeNull().And.BeEquivalentTo(validUser);
        }

        [Test]
        public void ConfirmEmail_PassedEmailIsEmpty_ThrowsExceptionWithCorrespondingMessage()
        {
            var authenticationService = new AuthenticationService(_userManager, _emailProvider, _mapper, _jwtProvider, _userService, _clientConnectionOptions);

            FluentActions.Awaiting(() => authenticationService.ConfirmEmailAsync(GetConfirmEmailViewWithEmptyEmail()))
                .Should().Throw<Exception>().WithMessage(Constants.Errors.Authentication.UserWasNotFound);
        }

        [Test]
        public void ConfirmEmail_PassedCodeIsEmpty_ThrowsExceptionWithCorrespondingMessage()
        {
            var authenticationService = new AuthenticationService(_userManager, _emailProvider, _mapper, _jwtProvider, _userService, _clientConnectionOptions);

            FluentActions.Awaiting(() => authenticationService.ConfirmEmailAsync(GetConfirmEmailViewWithEmptyCode()))
                .Should().Throw<Exception>().WithMessage(Constants.Errors.Authentication.UserWasNotFound);
        }

        [Test]
        public void ConfirmEmail_PassedValidModelConfirmEmailReturnsNotSuccessIdentityResponse_ThrowsExceptionWithCorrespondingMessage()
        {
            var validUser = GetValidUser();

            var userManagerMock = new Mock<UserManager<User>>(_userStore, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);
            userManagerMock.Setup(x => x.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(new IdentityResult());

            var authenticationService = new AuthenticationService(userManagerMock.Object, _emailProvider, _mapper, _jwtProvider, _userService, _clientConnectionOptions);

            var validConfirmEmailView = GetValidConfirmEmailView();

            FluentActions.Awaiting(() => authenticationService.ConfirmEmailAsync(validConfirmEmailView))
               .Should().Throw<Exception>().WithMessage(Constants.Errors.Authentication.EmailWasNotConfirmed);
        }

        private ConfirmEmailAuthenticationView GetConfirmEmailViewWithEmptyEmail()
        {
            return new ConfirmEmailAuthenticationView
            {
                Email = "",
                Code = "the_code_here"
            };
        }

        private ConfirmEmailAuthenticationView GetConfirmEmailViewWithEmptyCode()
        {
            return new ConfirmEmailAuthenticationView
            {
                Email = "a@a.com",
                Code = ""
            };
        }

        private ConfirmEmailAuthenticationView GetValidConfirmEmailView()
        {
            return new ConfirmEmailAuthenticationView
            {
                Email = "a@a.com",
                Code = "the_code_heregdfsgdfgdfgkdjfgjdfjgdfhjgdfdfgdfgdfdhfjgdfhjgdfjgdfgdjfgdhfjghdfghdgdjrheureutuetuyertyureyutyur" // should be long
            };
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
    }
}
