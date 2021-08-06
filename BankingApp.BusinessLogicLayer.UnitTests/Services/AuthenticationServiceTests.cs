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

        private IMapper _mapper;
        private AuthenticationService _authenticationService;
        private Mock<IUserStore<User>> _userStoreMock;
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<IEmailService> _emailServiceMock;
        private Mock<IJwtService> _jwtServiceMock;
        private Mock<IOptions<ClientConnectionOptions>> _clientConnectionOptionsMock;

        [SetUp]
        public void SetUp()
        {
            var mapperConfig = new MapperConfiguration(config => { config.AddProfile(new MapperProfile()); });
            _mapper = mapperConfig.CreateMapper();

            _userStoreMock = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(_userStoreMock.Object, null, null, null, null, null, null, null, null);
            _emailServiceMock = new Mock<IEmailService>();

            _jwtServiceMock = new Mock<IJwtService>();
            _jwtServiceMock.Setup(x => x.GetUserClaimsAsync(It.IsAny<string>())).ReturnsAsync(new List<Claim>());
            _jwtServiceMock.Setup(x => x.GenerateAccessToken(It.IsAny<List<Claim>>())).Returns(ValidAccessToken);

            _clientConnectionOptionsMock = new Mock<IOptions<ClientConnectionOptions>>();
            _clientConnectionOptionsMock.Setup(x => x.Value).Returns(new ClientConnectionOptions());

            _authenticationService = new AuthenticationService(
                _userManagerMock.Object,
                _emailServiceMock.Object,
                _mapper,
                _jwtServiceMock.Object,
                _clientConnectionOptionsMock.Object
            );
        }

        [Test]
        public async Task ConfirmEmail_ValidModel_UpdateAsyncInvoked()
        {
            var validUser = GetValidUser();
            User inputOfUserManagerUpdateAsync = null;

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);
            _userManagerMock.Setup(x => x.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .Callback((User user) => { inputOfUserManagerUpdateAsync = user; })
                .ReturnsAsync(IdentityResult.Success);

            var validConfirmEmailView = GetValidConfirmEmailView();
            await _authenticationService.ConfirmEmailAsync(validConfirmEmailView);

            inputOfUserManagerUpdateAsync.Should().NotBeNull().And.BeEquivalentTo(validUser);
        }

        [Test]
        public void ConfirmEmail_ThereIsNoSuchEmail_ThrowsException()
        {
            var validUser = GetValidUser();

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User) null);
            _userManagerMock.Setup(x => x.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            var validConfirmEmailView = GetValidConfirmEmailView();

            FluentActions.Awaiting(() => _authenticationService.ConfirmEmailAsync(validConfirmEmailView))
               .Should().Throw<Exception>().WithMessage(Constants.Errors.Authentication.UserWasNotFound);
        }

        [Test]
        public void ConfirmEmail_ConfirmEmailFailed_ThrowsException()
        {
            var validUser = GetValidUser();

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);
            _userManagerMock.Setup(x => x.ConfirmEmailAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(new IdentityResult());

            var validConfirmEmailView = GetValidConfirmEmailView();

            FluentActions.Awaiting(() => _authenticationService.ConfirmEmailAsync(validConfirmEmailView))
               .Should().Throw<Exception>().WithMessage(Constants.Errors.Authentication.EmailWasNotConfirmed);
        }

        [Test]
        public async Task SignIn_ValidSignInView_ExpectedResults()
        {
            var validUser = GetValidUser();

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);

            var validSignInview = GetValidSignInView();
            var tokensView = await _authenticationService.SignInAsync(validSignInview);

            tokensView.Should().NotBeNull().And.BeOfType<TokensView>().Which.AccessToken.Should().Be(ValidAccessToken);
        }

        [Test]
        public void SignIn_ThereIsNoUserWithSuchEmail_ThrowsException()
        {
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User) null);

            var validSignInView = GetValidSignInView();
            FluentActions.Awaiting(() => _authenticationService.SignInAsync(validSignInView))
               .Should().Throw<Exception>().WithMessage(Constants.Errors.Authentication.InvalidNicknameOrPassword);
        }

        [Test]
        public void SignIn_EmailIsNotConfirmed_ThrowsException()
        {
            var userWithEmailNotConfirmed = GetUserWithEmailNotConfirmed();

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(userWithEmailNotConfirmed);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);

            var validSignInView = GetValidSignInView();
            FluentActions.Awaiting(() => _authenticationService.SignInAsync(validSignInView))
               .Should().Throw<Exception>().WithMessage(Constants.Errors.Authentication.InvalidNicknameOrPassword);
        }

        [Test]
        public void SignIn_FoundUserIsBlocked_ThrowsException()
        {
            var blockedUser = GetBlockedUser();

            _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(blockedUser);

            var validSignInView = GetValidSignInView();
            FluentActions.Awaiting(() => _authenticationService.SignInAsync(validSignInView))
               .Should().Throw<Exception>().WithMessage(Constants.Errors.Authentication.UserIsBlocked);
        }

        [Test]
        public void SignIn_CheckPasswordFailed_ThrowsException()
        {
            var validUser = GetValidUser();

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(false);

            var validSignInView = GetValidSignInView();
            FluentActions.Awaiting(() => _authenticationService.SignInAsync(validSignInView))
               .Should().Throw<Exception>().WithMessage(Constants.Errors.Authentication.InvalidNicknameOrPassword);
        }

        [Test]
        public async Task SignUp_ValidSignUpView_SendEmailAsyncInvoked()
        {
            string emailPassedToSendEmailMethod = null;
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User) null);
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<User>())).ReturnsAsync(ValidPasswordResetCode);

            _emailServiceMock.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true)
                .Callback((string emailTo, string caption, string textMessage) => { emailPassedToSendEmailMethod = emailTo; });

            var validSignUpView = GetValidSignUpView();
            await _authenticationService.SignUpAsync(validSignUpView);

            emailPassedToSendEmailMethod.Should().NotBeNull().And.Be(validSignUpView.Email);
        }

        [Test]
        public void SignUp_UserWithSuchEmailAlreadyExists_ThrowsException()
        {
            var validUser = GetValidUser();

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);

            var validSignUpView = GetValidSignUpView();
            FluentActions.Awaiting(() => _authenticationService.SignUpAsync(validSignUpView))
               .Should().Throw<Exception>().WithMessage(Constants.Errors.Authentication.UserAlreadyExists);
        }

        [Test]
        public void SignUp_CreateAsyncDoesNotSuccesses_ThrowsException()
        {
            User userToDelete = null;

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(new IdentityResult());
            _userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success)
                .Callback((User user) => { userToDelete = user; });

            var validSignUpView = GetValidSignUpView();
            FluentActions.Awaiting(() => _authenticationService.SignUpAsync(validSignUpView)).Should().Throw<Exception>();

            userToDelete.Should().NotBeNull().And.BeEquivalentTo(validSignUpView, options => options.ExcludingMissingMembers());
        }

        [Test]
        public void SignUp_AddToRoleAsyncDoesNotSuccesses_ThrowsException()
        {
            User userToDelete = null;

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(new IdentityResult());
            _userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success)
                .Callback((User user) => { userToDelete = user; });

            var validSignUpView = GetValidSignUpView();
            FluentActions.Awaiting(() => _authenticationService.SignUpAsync(validSignUpView)).Should().Throw<Exception>()
                .WithMessage(Constants.Errors.Authentication.ClientUserWasNotAddedToClientRole);

            userToDelete.Should().NotBeNull().And.BeEquivalentTo(validSignUpView, options => options.ExcludingMissingMembers());
        }

        [Test]
        public void SignUp_GenerateEmailConfirmationTokenAsyncFailed_ThrowsException()
        {
            User userToDelete = null;

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<User>()))
                .Callback((User user) => { throw new Exception(); });
            _userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success)
                .Callback((User user) => { userToDelete = user; });

            var validSignUpView = GetValidSignUpView();
            FluentActions.Awaiting(() => _authenticationService.SignUpAsync(validSignUpView)).Should().Throw<Exception>();

            userToDelete.Should().NotBeNull().And.BeEquivalentTo(validSignUpView, options => options.ExcludingMissingMembers());
        }

        [Test]
        public void SignUp_SendEmailFailed_ThrowsException()
        {
            User userToDelete = null;

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<User>())).ReturnsAsync(ValidEmailConfirmationCode);
            _userManagerMock.Setup(x => x.DeleteAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success)
                .Callback((User user) => { userToDelete = user; });

            _emailServiceMock.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            var validSignUpView = GetValidSignUpView();
            FluentActions.Awaiting(() => _authenticationService.SignUpAsync(validSignUpView)).Should()
                .Throw<Exception>().WithMessage(Constants.Errors.Authentication.UserWasNotRegistered);

            userToDelete.Should().NotBeNull().And.BeEquivalentTo(validSignUpView, options => options.ExcludingMissingMembers());
        }

        [Test]
        public async Task ForgotPassword_ValidForgotPasswordView_SendEmailAsyncInvoked()
        {
            var validUser = GetValidUser();
            string emailPassedToSendEmailMethod = null;

            _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(It.IsAny<User>())).ReturnsAsync(ValidPasswordResetCode);
            _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithClientRole());
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);

            _emailServiceMock.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true)
                .Callback((string emailTo, string caption, string textMessage) => { emailPassedToSendEmailMethod = emailTo; });

            var validForgotPasswordView = GetValidForgotPasswordView();
            await _authenticationService.ForgotPasswordAsync(validForgotPasswordView);

            emailPassedToSendEmailMethod.Should().NotBeNull().And.Be(validUser.Email);
        }

        [Test]
        public void ForgotPassword_ThereIsNoUserWithSuchEmail_ThrowsException()
        {
            var validUser = GetValidUser();
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);

            var validForgotPasswordView = GetValidForgotPasswordView();
            FluentActions.Awaiting(() => _authenticationService.ForgotPasswordAsync(validForgotPasswordView)).Should().Throw<Exception>()
                .WithMessage(Constants.Errors.Authentication.ErrorWhileSendingMessage);
        }

        [Test]
        public void ForgotPassword_ButUserIsOfAdminRole_ThrowsException()
        {
            var validUser = GetValidUser();
            var rolesListWithAdmin = GetRolesWithAdminRole();

            _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(rolesListWithAdmin);
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);

            var validForgotPasswordView = GetValidForgotPasswordView();
            FluentActions.Awaiting(() => _authenticationService.ForgotPasswordAsync(validForgotPasswordView)).Should().Throw<Exception>()
                .WithMessage(Constants.Errors.Authentication.ErrorWhileSendingMessage);
        }

        [Test]
        public void ForgotPassword_SendEmailFailed_ThrowsException()
        {
            var validUser = GetValidUser();

            _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(It.IsAny<User>())).ReturnsAsync(ValidPasswordResetCode);
            _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetRolesWithClientRole());
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);

            var emailProviderMock = new Mock<IEmailService>();
            emailProviderMock.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            var validForgotPasswordView = GetValidForgotPasswordView();
            FluentActions.Awaiting(() => _authenticationService.ForgotPasswordAsync(validForgotPasswordView))
               .Should().Throw<Exception>().WithMessage(Constants.Errors.Authentication.ErrorWhileSendingMessage);
        }

        [Test]
        public void ResetPassword_ValidResetPasswordView_ResetPasswordAsyncInvoked()
        {
            var validUser = GetValidUser();
            User userPassedToResetPasswordAsync = null;

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);
            _userManagerMock.Setup(x => x.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success)
                .Callback((User user, string code, string password) => { userPassedToResetPasswordAsync = user;  });

            var validResetPasswordView = GetValidResetPasswordView();
            FluentActions.Awaiting(() => _authenticationService.ResetPasswordAsync(validResetPasswordView)).Should().NotThrow<Exception>();

            userPassedToResetPasswordAsync.Should().NotBeNull().And.BeEquivalentTo(validUser);
        }

        [Test]
        public void ResetPassword_ThereIsNoUserWithSuchEmail_ThrowsException()
        {
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User) null);

            var validResetPasswordView = GetValidResetPasswordView();
            FluentActions.Awaiting(() => _authenticationService.ResetPasswordAsync(validResetPasswordView)).Should()
                .Throw<Exception>().WithMessage(Constants.Errors.Authentication.UserWasNotFound);
        }

        [Test]
        public void ResetPassword_ResetPasswordAsyncFails_ThrowsException()
        {
            var validUser = GetValidUser();

            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);
            _userManagerMock.Setup(x => x.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new IdentityResult());

            var validResetPasswordView = GetValidResetPasswordView();
            FluentActions.Awaiting(() => _authenticationService.ResetPasswordAsync(validResetPasswordView)).Should().Throw<Exception>();
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
