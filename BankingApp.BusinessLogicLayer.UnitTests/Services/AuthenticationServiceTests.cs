using AutoMapper;
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.BusinessLogicLayer.Mapper;
using BankingApp.BusinessLogicLayer.Services;
using BankingApp.Entities.Entities;
using BankingApp.Shared;
using BankingApp.Shared.Options;
using BankingApp.ViewModels.ViewModels.Authentication;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.UnitTests.Services
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private const string ValidAccessToken = "this_is_very_secret_token";
        private const string ValidPasswordResetCode = "this_is_very_secret_password_reset_code";
        private const string ValidEmailConfirmationCode = "this_is_very_secret_email_confirmation_code";

        private IUserStore<User> _userStore;
        private UserManager<User> _userManager;
        private IEmailService _emailService;
        private IMapper _mapper;
        private IJwtService _jwtService;
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
            var emailProvider = new Mock<IEmailService>();
            _emailService = emailProvider.Object;

            // jwt provider
            var jwtProviderMock = new Mock<IJwtService>();
            jwtProviderMock.Setup(x => x.GetUserClaimsAsync(It.IsAny<string>())).ReturnsAsync(new List<Claim>());
            jwtProviderMock.Setup(x => x.GenerateAccessToken(It.IsAny<List<Claim>>())).Returns(ValidAccessToken);
            _jwtService = jwtProviderMock.Object;

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

            var authenticationService = new AuthenticationService(userManagerMock.Object, _emailService, _mapper, _jwtService, _clientConnectionOptions);

            var validConfirmEmailView = GetValidConfirmEmailView();
            await authenticationService.ConfirmEmailAsync(validConfirmEmailView);

            inputOfUserManagerUpdateAsync.Should().NotBeNull().And.BeEquivalentTo(validUser);
        }

        [Test]
        public void ConfirmEmail_PassedValidModelButFindByEmailAsyncReturnsNull_ThrowsExceptionWithCorrespondingMessage()
        {
            var validUser = GetValidUser();

            var userManagerMock = new Mock<UserManager<User>>(_userStore, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User) null);
            userManagerMock.Setup(x => x.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            var authenticationService = new AuthenticationService(userManagerMock.Object, _emailService, _mapper, _jwtService, _clientConnectionOptions);

            var validConfirmEmailView = GetValidConfirmEmailView();

            FluentActions.Awaiting(() => authenticationService.ConfirmEmailAsync(validConfirmEmailView))
               .Should().Throw<Exception>().WithMessage(Constants.Errors.Authentication.UserWasNotFound);
        }

        [Test]
        public void ConfirmEmail_PassedValidModelButConfirmEmailReturnsNotSuccessIdentityResponse_ThrowsExceptionWithCorrespondingMessage()
        {
            var validUser = GetValidUser();

            var userManagerMock = new Mock<UserManager<User>>(_userStore, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);
            userManagerMock.Setup(x => x.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(new IdentityResult());

            var authenticationService = new AuthenticationService(userManagerMock.Object, _emailService, _mapper, _jwtService, _clientConnectionOptions);

            var validConfirmEmailView = GetValidConfirmEmailView();

            FluentActions.Awaiting(() => authenticationService.ConfirmEmailAsync(validConfirmEmailView))
               .Should().Throw<Exception>().WithMessage(Constants.Errors.Authentication.EmailWasNotConfirmed);
        }

        [Test]
        public async Task SignIn_PassedValidSignInView_ReturnsValidTokensView()
        {
            var validUser = GetValidUser();

            var userManagerMock = new Mock<UserManager<User>>(_userStore, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);
            userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);

            var authenticationService = new AuthenticationService(userManagerMock.Object, _emailService, _mapper, _jwtService, _clientConnectionOptions);

            var validSignInview = GetValidSignInView();
            var tokensView = await authenticationService.SignInAsync(validSignInview);

            tokensView.Should().NotBeNull().And.BeOfType<TokensView>().Which.AccessToken.Should().Be(ValidAccessToken);
        }

        [Test]
        public void SignIn_UserManagerFindByEmailAsyncReturnsNull_ThrowsExceptionWithCorrespondingMessage()
        {
            var userManagerMock = new Mock<UserManager<User>>(_userStore, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User) null);

            var authenticationService = new AuthenticationService(userManagerMock.Object, _emailService, _mapper, _jwtService, _clientConnectionOptions);

            var validSignInView = GetValidSignInView();
            FluentActions.Awaiting(() => authenticationService.SignInAsync(validSignInView))
               .Should().Throw<Exception>().WithMessage(Constants.Errors.Authentication.InvalidNicknameOrPassword);
        }

        [Test]
        public void SignIn_FoundUserHasNotEmailConfirmed_ThrowsExceptionWithCorrespondingMessage()
        {
            var userWithEmailNotConfirmed = GetUserWithEmailNotConfirmed();

            var userManagerMock = new Mock<UserManager<User>>(_userStore, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(userWithEmailNotConfirmed);
            userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);

            var authenticationService = new AuthenticationService(userManagerMock.Object, _emailService, _mapper, _jwtService, _clientConnectionOptions);

            var validSignInView = GetValidSignInView();
            FluentActions.Awaiting(() => authenticationService.SignInAsync(validSignInView))
               .Should().Throw<Exception>().WithMessage(Constants.Errors.Authentication.InvalidNicknameOrPassword);
        }

        [Test]
        public void SignIn_FoundUserIsBlocked_ThrowsExceptionWithCorrespondingMessage()
        {
            var blockedUser = GetBlockedUser();

            var userManagerMock = new Mock<UserManager<User>>(_userStore, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(blockedUser);

            var authenticationService = new AuthenticationService(userManagerMock.Object, _emailService, _mapper, _jwtService, _clientConnectionOptions);

            var validSignInView = GetValidSignInView();
            FluentActions.Awaiting(() => authenticationService.SignInAsync(validSignInView))
               .Should().Throw<Exception>().WithMessage(Constants.Errors.Authentication.UserIsBlocked);
        }

        [Test]
        public void SignIn_CheckPasswordFailed_ThrowsExceptionWithCorrespondingMessage()
        {
            var validUser = GetValidUser();

            var userManagerMock = new Mock<UserManager<User>>(_userStore, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);
            userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(false);

            var authenticationService = new AuthenticationService(userManagerMock.Object, _emailService, _mapper, _jwtService, _clientConnectionOptions);

            var validSignInView = GetValidSignInView();
            FluentActions.Awaiting(() => authenticationService.SignInAsync(validSignInView))
               .Should().Throw<Exception>().WithMessage(Constants.Errors.Authentication.InvalidNicknameOrPassword);
        }

        [Test]
        public async Task SignUp_PassedValidSignUpView_SendEmailAsyncOfEmailProviderGetsCalledWithCorrespondingEmailParameter()
        {
            string emailPassedToSendEmailMethod = null;

            var userManagerMock = new Mock<UserManager<User>>(_userStore, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User) null);
            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<User>())).ReturnsAsync(ValidPasswordResetCode);

            var emailProviderMock = new Mock<IEmailService>();
            emailProviderMock.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true)
                .Callback((string emailTo, string caption, string textMessage) => { emailPassedToSendEmailMethod = emailTo; });

            var authenticationService = new AuthenticationService(userManagerMock.Object, emailProviderMock.Object, _mapper, _jwtService, _clientConnectionOptions);

            var validSignUpView = GetValidSignUpView();
            await authenticationService.SignUpAsync(validSignUpView);

            emailPassedToSendEmailMethod.Should().NotBeNull().And.Be(validSignUpView.Email);
        }

        [Test]
        public void SignUp_PassedValidSignUpViewButUserWithSuchEmailAlreadyExists_ThrowsExceptionWithCorrespondingMessage()
        {
            var validUser = GetValidUser();

            var userManagerMock = new Mock<UserManager<User>>(_userStore, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);

            var authenticationService = new AuthenticationService(userManagerMock.Object, _emailService, _mapper, _jwtService, _clientConnectionOptions);

            var validSignUpView = GetValidSignUpView();
            FluentActions.Awaiting(() => authenticationService.SignUpAsync(validSignUpView))
               .Should().Throw<Exception>().WithMessage(Constants.Errors.Authentication.UserAlreadyExists);
        }

        [Test]
        public void SignUp_PassedValidSignUpViewButCreateAsyncDoesNotSuccesses_ThrowsExceptionAndDeleteAsyncShouldBeCalled()
        {
            User userToDelete = null;

            var userManagerMock = new Mock<UserManager<User>>(_userStore, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(new IdentityResult());
            userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success)
                .Callback((User user) => { userToDelete = user; });

            var authenticationService = new AuthenticationService(userManagerMock.Object, _emailService, _mapper, _jwtService, _clientConnectionOptions);

            var validSignUpView = GetValidSignUpView();
            FluentActions.Awaiting(() => authenticationService.SignUpAsync(validSignUpView)).Should().Throw<Exception>();

            userToDelete.Should().NotBeNull().And.BeEquivalentTo(validSignUpView, options => options.ExcludingMissingMembers());
        }

        [Test]
        public void SignUp_PassedValidSignUpViewButAddToRoleAsyncDoesNotSuccesses_ThrowsExceptionAndDeleteAsyncShouldBeCalled()
        {
            User userToDelete = null;

            var userManagerMock = new Mock<UserManager<User>>(_userStore, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(new IdentityResult());
            userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success)
                .Callback((User user) => { userToDelete = user; });

            var authenticationService = new AuthenticationService(userManagerMock.Object, _emailService, _mapper, _jwtService, _clientConnectionOptions);

            var validSignUpView = GetValidSignUpView();
            FluentActions.Awaiting(() => authenticationService.SignUpAsync(validSignUpView)).Should().Throw<Exception>()
                .WithMessage(Constants.Errors.Authentication.ClientUserWasNotAddedToClientRole);

            userToDelete.Should().NotBeNull().And.BeEquivalentTo(validSignUpView, options => options.ExcludingMissingMembers());
        }

        [Test]
        public void SignUp_PassedValidSignUpViewButGenerateEmailConfirmationTokenAsyncThrowsException_ThrowsExceptionAndDeleteAsyncShouldBeCalled()
        {
            User userToDelete = null;

            var userManagerMock = new Mock<UserManager<User>>(_userStore, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()))
                .Callback((User user) => { throw new Exception(); });
            userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success)
                .Callback((User user) => { userToDelete = user; });

            var authenticationService = new AuthenticationService(userManagerMock.Object, _emailService, _mapper, _jwtService, _clientConnectionOptions);

            var validSignUpView = GetValidSignUpView();
            FluentActions.Awaiting(() => authenticationService.SignUpAsync(validSignUpView)).Should().Throw<Exception>();

            userToDelete.Should().NotBeNull().And.BeEquivalentTo(validSignUpView, options => options.ExcludingMissingMembers());
        }

        [Test]
        public void SignUp_PassedValidSignUpViewButSendEmailAsyncReturnsFalse_ThrowsExceptionAndDeleteAsyncShouldBeCalled()
        {
            User userToDelete = null;

            var userManagerMock = new Mock<UserManager<User>>(_userStore, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<User>())).ReturnsAsync(ValidEmailConfirmationCode);
            userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success)
                .Callback((User user) => { userToDelete = user; });

            var emailProviderMock = new Mock<IEmailService>();
            emailProviderMock.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            var authenticationService = new AuthenticationService(userManagerMock.Object, emailProviderMock.Object, _mapper, _jwtService, _clientConnectionOptions);

            var validSignUpView = GetValidSignUpView();
            FluentActions.Awaiting(() => authenticationService.SignUpAsync(validSignUpView)).Should()
                .Throw<Exception>().WithMessage(Constants.Errors.Authentication.UserWasNotRegistered);

            userToDelete.Should().NotBeNull().And.BeEquivalentTo(validSignUpView, options => options.ExcludingMissingMembers());
        }

        [Test]
        public async Task ForgotPassword_PassedValidForgotPasswordView_SendEmailAsyncOfEmailProviderGetsCalledWithCorrespondingEmailParameter()
        {
            var validUser = GetValidUser();
            string emailPassedToSendEmailMethod = null;

            var userManagerMock = new Mock<UserManager<User>>(_userStore, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(It.IsAny<User>())).ReturnsAsync(ValidPasswordResetCode);
            userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithClientRole());
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);

            var emailProviderMock = new Mock<IEmailService>();
            emailProviderMock.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true)
                .Callback((string emailTo, string caption, string textMessage) => { emailPassedToSendEmailMethod = emailTo; });


            var authenticationService = new AuthenticationService(userManagerMock.Object, emailProviderMock.Object, _mapper, _jwtService, _clientConnectionOptions);

            var validForgotPasswordView = GetValidForgotPasswordView();
            await authenticationService.ForgotPasswordAsync(validForgotPasswordView);

            emailPassedToSendEmailMethod.Should().NotBeNull().And.Be(validUser.Email);
        }

        [Test]
        public void ForgotPassword_PassedValidForgotPasswordViewButThereIsNoUserWithSuchEmail_ThrowsExceptionWithCorrepondingException()
        {
            var validUser = GetValidUser();

            var userManagerMock = new Mock<UserManager<User>>(_userStore, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);

            var authenticationService = new AuthenticationService(_userManager, _emailService, _mapper, _jwtService, _clientConnectionOptions);

            var validForgotPasswordView = GetValidForgotPasswordView();
            FluentActions.Awaiting(() => authenticationService.ForgotPasswordAsync(validForgotPasswordView)).Should().Throw<Exception>()
                .WithMessage(Constants.Errors.Authentication.ErrorWhileSendingMessage);
        }

        [Test]
        public void ForgotPassword_PassedValidForgotPasswordViewButUserIsOfAdminRole_ThrowsExceptionWithCorrespondingMessage()
        {
            var validUser = GetValidUser();
            var rolesListWithAdmin = GetRolesWithAdminRole();

            var userManagerMock = new Mock<UserManager<User>>(_userStore, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(rolesListWithAdmin);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);

            var authenticationService = new AuthenticationService(userManagerMock.Object, _emailService, _mapper, _jwtService, _clientConnectionOptions);

            var validForgotPasswordView = GetValidForgotPasswordView();
            FluentActions.Awaiting(() => authenticationService.ForgotPasswordAsync(validForgotPasswordView)).Should().Throw<Exception>()
                .WithMessage(Constants.Errors.Authentication.ErrorWhileSendingMessage);
        }

        [Test]
        public void ForgotPassword_PassedValidForgotPasswordViewButMessageWereNotSent_ThrowsExceptionWithCorrespondingMessage()
        {
            var validUser = GetValidUser();

            var userManagerMock = new Mock<UserManager<User>>(_userStore, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(It.IsAny<User>())).ReturnsAsync(ValidPasswordResetCode);
            userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithClientRole());
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);

            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);

            var emailProviderMock = new Mock<IEmailService>();
            emailProviderMock.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            var authenticationService = new AuthenticationService(userManagerMock.Object, emailProviderMock.Object, _mapper, _jwtService, _clientConnectionOptions);

            var validForgotPasswordView = GetValidForgotPasswordView();
            FluentActions.Awaiting(() => authenticationService.ForgotPasswordAsync(validForgotPasswordView))
               .Should().Throw<Exception>().WithMessage(Constants.Errors.Authentication.ErrorWhileSendingMessage);
        }

        [Test]
        public void ResetPassword_PassedValidResetPasswordView_CallsResetPasswordAsyncAndDoesNotThrowsExceptions()
        {
            var validUser = GetValidUser();
            User userPassedToResetPasswordAsync = null;

            var userManagerMock = new Mock<UserManager<User>>(_userStore, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);
            userManagerMock.Setup(x => x.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success)
                .Callback((User user, string code, string password) => { userPassedToResetPasswordAsync = user;  });

            var authenticationService = new AuthenticationService(userManagerMock.Object, _emailService, _mapper, _jwtService, _clientConnectionOptions);

            var validResetPasswordView = GetValidResetPasswordView();
            FluentActions.Awaiting(() => authenticationService.ResetPasswordAsync(validResetPasswordView)).Should().NotThrow<Exception>();

            userPassedToResetPasswordAsync.Should().NotBeNull().And.BeEquivalentTo(validUser);
        }

        [Test]
        public void ResetPassword_PassedValidResetPasswordViewButThereIsNoUserWithSuchEmail_ThrowsExceptionWithCorrespondingMessage()
        {
            var userManagerMock = new Mock<UserManager<User>>(_userStore, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User) null);

            var authenticationService = new AuthenticationService(userManagerMock.Object, _emailService, _mapper, _jwtService, _clientConnectionOptions);

            var validResetPasswordView = GetValidResetPasswordView();
            FluentActions.Awaiting(() => authenticationService.ResetPasswordAsync(validResetPasswordView)).Should()
                .Throw<Exception>().WithMessage(Constants.Errors.Authentication.UserWasNotFound);
        }

        [Test]
        public void ResetPassword_PassedValidResetPasswordViewButResetPasswordAsyncFails_ThrowsException()
        {
            var validUser = GetValidUser();

            var userManagerMock = new Mock<UserManager<User>>(_userStore, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);
            userManagerMock.Setup(x => x.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new IdentityResult());

            var authenticationService = new AuthenticationService(userManagerMock.Object, _emailService, _mapper, _jwtService, _clientConnectionOptions);

            var validResetPasswordView = GetValidResetPasswordView();
            FluentActions.Awaiting(() => authenticationService.ResetPasswordAsync(validResetPasswordView)).Should().Throw<Exception>();
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

        private User GetUserWithEmailNotConfirmed()
        {
            return new User
            {
                Id = 1,
                Email = "a@a.com",
                UserName = "Me",
                EmailConfirmed = false,
                Deposits = new List<Deposit>()
            };
        }

        private User GetBlockedUser()
        {
            return new User
            {
                Id = 1,
                Email = "rusland610@gmail.com",
                UserName = "Me",
                EmailConfirmed = true,
                Deposits = new List<Deposit>(),
                IsBlocked = true
            };
        }

        private SignInAuthenticationView GetValidSignInView()
        {
            return new SignInAuthenticationView
            {
                Email = "rusland610@gmail.com",
                Password = "qwerty12345AAA"
            };
        }

        private SignUpAuthenticationView GetValidSignUpView()
        {
            return new SignUpAuthenticationView
            {
                Email = "rusland610@gmail.com",
                Nickname = "Bobik",
                Password = "qwerty12345AAA",
                ConfirmPassword = "qwerty12345AAA"
            };
        }

        private ForgotPasswordAuthenticationView GetValidForgotPasswordView()
        {
            return new ForgotPasswordAuthenticationView
            {
                Email = "a@a.com"
            };
        }

        private ForgotPasswordAuthenticationView GetForgotPasswordViewWithEmptyEmail()
        {
            return new ForgotPasswordAuthenticationView
            {
                Email = ""
            };
        }

        private ResetPasswordAuthenticationView GetValidResetPasswordView()
        {
            return new ResetPasswordAuthenticationView
            { 
                Email = "rusland610@gmail.com",
                Code = "code_goes_here",
                Password = "qwerty12345AAA",
                ConfirmPassword = "qwerty12345AAA"
            };
        }

        private IList<string> GetRolesWithAdminRole()
        {
            return new List<string>
            {
                Constants.Roles.Admin
            };
        }

        private IList<string> GetRolesWithClientRole()
        {
            return new List<string>
            {
                Constants.Roles.Client
            };
        }
    }
}
