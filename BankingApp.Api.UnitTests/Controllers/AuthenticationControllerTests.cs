using BankingApp.Api.Controllers;
using BankingApp.BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using BankingApp.ViewModels.ViewModels.Authentication;

namespace BankingApp.Api.UnitTests.Controllers
{
    [TestFixture]
    public class AuthenticationControllerTests
    {
        private AuthenticationController _authenticationController;
        private Mock<IAuthenticationService> _authenticationServiceMock;

        [SetUp]
        public void SetUp()
        {
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _authenticationServiceMock.Setup(x => x.SignUpAsync(It.IsAny<SignUpAuthenticationView>()));
            _authenticationServiceMock.Setup(x => x.SignInAsync(It.IsAny<SignInAuthenticationView>())).ReturnsAsync(GetValidTokensView());
            _authenticationServiceMock.Setup(x => x.ForgotPasswordAsync(It.IsAny<ForgotPasswordAuthenticationView>()));
            _authenticationServiceMock.Setup(x => x.ResetPasswordAsync(It.IsAny<ResetPasswordAuthenticationView>()));

            _authenticationController = new AuthenticationController(_authenticationServiceMock.Object);
        }

        [Test]
        public async Task SignUp_СorrectInputData_ExpectedResults()
        {
            var validSignUpView = GetValidSignUpView();

            var controllerResult = await _authenticationController.SignUp(validSignUpView);
            controllerResult.Should().NotBeNull().And.BeOfType<OkResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Test]
        public async Task SignIn_СorrectInputData_ExpectedResults()
        {
            var validSignInView = GetValidSignInView();
            var validTokensView = GetValidTokensView();
            var controllerResult = await _authenticationController.SignIn(validSignInView);

            var resultOfOkObjectResultValidation = controllerResult.Should().NotBeNull().And.BeOfType<OkObjectResult>();
            resultOfOkObjectResultValidation.Which.Value.Should().BeOfType<TokensView>().And.BeEquivalentTo(validTokensView);
            resultOfOkObjectResultValidation.Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Test]
        public async Task ConfirmEmail_СorrectInputData_ExpectedResults()
        {
            var confirmEmailView = GetValidConfirmEmailView();
            var controllerResult = await _authenticationController.ConfirmEmail(confirmEmailView);

            controllerResult.Should().NotBeNull().And.BeOfType<OkResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Test]
        public async Task ResetPassword_СorrectInputData_ExpectedResults()
        {
            var validResetPasswordView = GetValidResetPasswordView();
            var controllerResult = await _authenticationController.ResetPassword(validResetPasswordView);

            controllerResult.Should().NotBeNull().And.BeOfType<OkResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Test]
        public async Task ForgotPassword_СorrectInputData_ExpectedResults()
        {
            var validForgotPassword = GetValidForgotPasswordView();
            var controllerResult = await _authenticationController.ForgotPassword(validForgotPassword);

            controllerResult.Should().NotBeNull().And.BeOfType<OkResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
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

        private SignInAuthenticationView GetValidSignInView()
        {
            return new SignInAuthenticationView
            {
                Email = "rusland610@gmail.com",
                Password = "qwerty12345AAA"
            };
        }

        private ConfirmEmailAuthenticationView GetValidConfirmEmailView()
        {
            return new ConfirmEmailAuthenticationView
            {
                Email = "rusland610@gmail.com",
                Code = "here_is_password_reset_code"
            };
        } 
        
        private ResetPasswordAuthenticationView GetValidResetPasswordView()
        {
            return new ResetPasswordAuthenticationView
            {
                Password = "qwerty12345AAA",
                ConfirmPassword = "qwerty12345AAA"
            };
        }

        private ForgotPasswordAuthenticationView GetValidForgotPasswordView()
        {
            return new ForgotPasswordAuthenticationView
            {
                Email = "rusland610@gmail.com"
            };
        }

        private TokensView GetValidTokensView()
        {
            return new TokensView
            {
                  AccessToken = "here_is_token"
            };
        }
    }
}
