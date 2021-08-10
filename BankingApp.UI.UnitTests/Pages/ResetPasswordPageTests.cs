using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Pages.ResetPasswordPage;
using Blazored.Toast.Services;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using Xunit;
using FluentAssertions;
using static BankingApp.UI.Core.Constants.Constants;
using BankingApp.Shared;
using BankingApp.ViewModels.ViewModels.Authentication;

namespace BankingApp.UI.UnitTests.Pages
{
    public class ResetPasswordPageTests : TestContext
    {
        private const string ValidQueryUrl = "http://test.com?email=a@a.com&code=fdkfgdffkdsdsfdsfdsfdsfsd2342dfsf";

        private readonly Mock<IAuthenticationService> _authenticationServiceMock;
        private readonly Mock<INavigationWrapper> _navigationWrapperMock;
        private readonly Mock<ILoaderService> _loaderServiceMock;
        private readonly Mock<IToastService> _toastServiceMock;

        public ResetPasswordPageTests()
        {
            _authenticationServiceMock = new Mock<IAuthenticationService>();

            _loaderServiceMock = new Mock<ILoaderService>();
            _loaderServiceMock.Setup(x => x.SwitchOn());
            _loaderServiceMock.Setup(x => x.SwitchOff());

            _navigationWrapperMock = new Mock<INavigationWrapper>();
            _navigationWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), false)).Verifiable();
            _navigationWrapperMock.Setup(x => x.ToAbsoluteUri(It.IsAny<string>())).Returns(GetValidUri());

            _toastServiceMock = new Mock<IToastService>();

            Services.AddSingleton(_authenticationServiceMock.Object);
            Services.AddSingleton(_loaderServiceMock.Object);
            Services.AddSingleton(_navigationWrapperMock.Object);
            Services.AddSingleton(_toastServiceMock.Object);
        }

        [Fact]
        public void WhenTheFormIsSubmited_ValidData_ExpectedResults()
        {
            bool switchOnCalled = false;
            bool switchOffCalled = false;
            ResetPasswordAuthenticationView resetPasswordViewSentToServer = null;
            string navigateToUri = null;
            string notificationMessage = null;
            string toAbsoluteUriParameter = null;

            _authenticationServiceMock.Setup(x => x.ResetPasswordAsync(It.IsAny<ResetPasswordAuthenticationView>()))
                .Callback((ResetPasswordAuthenticationView view) => { resetPasswordViewSentToServer = view; })
                .ReturnsAsync(true);
            
            _loaderServiceMock.Setup(x => x.SwitchOn()).Callback(() => { switchOnCalled = true; });
            _loaderServiceMock.Setup(x => x.SwitchOff()).Callback(() => { switchOffCalled = true; });

            _navigationWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), false)).Callback((string uri, bool force) => { navigateToUri = uri; });
            _navigationWrapperMock.Setup(x => x.ToAbsoluteUri(It.IsAny<string>())).Callback((string uri) => { toAbsoluteUriParameter = uri; }).Returns(GetValidUri());
            _navigationWrapperMock.Setup(x => x.Uri).Returns(ValidQueryUrl);

            _toastServiceMock.Setup(x => x.ShowSuccess(It.IsAny<string>(), It.IsAny<string>())).Callback((string message, string heading) => { notificationMessage = message; });

            var validResetPasswordView = GetValidResetPasswordView();
            var resetPasswordForm = RenderComponent<ResetPasswordPage>();

            resetPasswordForm.Find("input[id=password]").Change(validResetPasswordView.Password.ToString());
            resetPasswordForm.Find("input[id=confirmPassword]").Change(validResetPasswordView.ConfirmPassword.ToString());
            resetPasswordForm.Find("form").Submit();

            toAbsoluteUriParameter.Should().BeEquivalentTo(_navigationWrapperMock.Object.Uri);
            switchOnCalled.Should().BeTrue();
            switchOffCalled.Should().BeTrue();
            resetPasswordViewSentToServer.Should().NotBeNull();
            resetPasswordViewSentToServer.Password.Should().Be(validResetPasswordView.Password);
            resetPasswordViewSentToServer.ConfirmPassword.Should().Be(validResetPasswordView.ConfirmPassword);
            navigateToUri.Should().Be(Routes.SignInPage);
            notificationMessage.Should().Be(Notifications.PasswordResetSuccessfully);
        }

        [Fact]
        public void WhenTheFormIsSubmited_SendMessageFailure_ExpectedResults()
        {
            bool switchOnCalled = false;
            bool switchOffCalled = false;
            ResetPasswordAuthenticationView resetPasswordViewSentToServer = null;
            string navigateToUri = null;
            string notificationMessage = null;
            string toAbsoluteUriParameter = null;

            _authenticationServiceMock.Setup(x => x.ResetPasswordAsync(It.IsAny<ResetPasswordAuthenticationView>()))
                .Callback((ResetPasswordAuthenticationView view) => { resetPasswordViewSentToServer = view; })
                .ReturnsAsync(false);

            _loaderServiceMock.Setup(x => x.SwitchOn()).Callback(() => { switchOnCalled = true; });
            _loaderServiceMock.Setup(x => x.SwitchOff()).Callback(() => { switchOffCalled = true; });

            _navigationWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), false)).Callback((string uri, bool force) => { navigateToUri = uri; });
            _navigationWrapperMock.Setup(x => x.ToAbsoluteUri(It.IsAny<string>())).Callback((string uri) => { toAbsoluteUriParameter = uri; }).Returns(GetValidUri());
            _navigationWrapperMock.Setup(x => x.Uri).Returns(ValidQueryUrl);

            _toastServiceMock.Setup(x => x.ShowSuccess(It.IsAny<string>(), It.IsAny<string>())).Callback((string message, string heading) => { notificationMessage = message; });

            var validResetPasswordView = GetValidResetPasswordView();
            var resetPasswordForm = RenderComponent<ResetPasswordPage>();

            resetPasswordForm.Find("input[id=password]").Change(validResetPasswordView.Password.ToString());
            resetPasswordForm.Find("input[id=confirmPassword]").Change(validResetPasswordView.ConfirmPassword.ToString());
            resetPasswordForm.Find("form").Submit();

            toAbsoluteUriParameter.Should().BeEquivalentTo(_navigationWrapperMock.Object.Uri);
            switchOnCalled.Should().BeTrue();
            switchOffCalled.Should().BeTrue();
            resetPasswordViewSentToServer.Password.Should().Be(validResetPasswordView.Password);
            resetPasswordViewSentToServer.ConfirmPassword.Should().Be(validResetPasswordView.ConfirmPassword);
            navigateToUri.Should().BeNull();
            notificationMessage.Should().BeNull();
        }

        [Fact]
        public void WhenTheFormIsSubmited_ValidData_ExpectedMarkupRendered()
        {
            var validResetPasswordView = GetValidResetPasswordView();
            var resetPasswordForm = RenderComponent<ResetPasswordPage>();

            resetPasswordForm.Find("input[id=password]").Change(validResetPasswordView.Password.ToString());
            resetPasswordForm.Find("input[id=confirmPassword]").Change(validResetPasswordView.ConfirmPassword.ToString());
            resetPasswordForm.Find("form").Submit();

            var validationErrorMessage = resetPasswordForm.FindAll("div[class=validation-message]");
            validationErrorMessage.Should().BeEmpty();
        }

        [Fact]
        public void WhenTheFormIsSubmited_PasswwordIsTooShort_ExpectedMarkupRendered()
        {
            var validResetPasswordView = GetValidResetPasswordView();
            validResetPasswordView.Password = "abc";
            validResetPasswordView.ConfirmPassword = "abc";

            var resetPasswordForm = RenderComponent<ResetPasswordPage>();

            resetPasswordForm.Find("input[id=password]").Change(validResetPasswordView.Password.ToString());
            resetPasswordForm.Find("input[id=confirmPassword]").Change(validResetPasswordView.ConfirmPassword.ToString());
            resetPasswordForm.Find("form").Submit();

            var validationErrorMessage = resetPasswordForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Authentication.PasswordIsTooShort);
        }

        [Fact]
        public void WhenTheFormIsSubmited_PasswwordDoesNotMatchesRegularExpression_ExpectedMarkupRendered()
        {
            var validResetPasswordView = GetValidResetPasswordView();
            validResetPasswordView.Password = "abcdAAABBBBBBCCCCCC";
            validResetPasswordView.ConfirmPassword = "abcdAAABBBBBBCCCCCC";

            var resetPasswordForm = RenderComponent<ResetPasswordPage>();

            resetPasswordForm.Find("input[id=password]").Change(validResetPasswordView.Password.ToString());
            resetPasswordForm.Find("input[id=confirmPassword]").Change(validResetPasswordView.ConfirmPassword.ToString());
            resetPasswordForm.Find("form").Submit();

            var validationErrorMessage = resetPasswordForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Authentication.PasswordIsNotHardEnough);
        }

        [Fact]
        public void WhenTheFormIsSubmited_PasswwordDoesNotMatchWithConfirmPassword_ExpectedMarkupRendered()
        {
            var validResetPasswordView = GetValidResetPasswordView();
            validResetPasswordView.Password = "abcdAAABBBBBBCCCCCC12345";
            validResetPasswordView.ConfirmPassword = "abcdAAABBBBBBCCCCCC1234";

            var resetPasswordForm = RenderComponent<ResetPasswordPage>();

            resetPasswordForm.Find("input[id=password]").Change(validResetPasswordView.Password.ToString());
            resetPasswordForm.Find("input[id=confirmPassword]").Change(validResetPasswordView.ConfirmPassword.ToString());
            resetPasswordForm.Find("form").Submit();

            var validationErrorMessage = resetPasswordForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Authentication.ConfirmPasswordShouldMatchPassword);
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

        private Uri GetValidUri()
        {
            return new Uri(ValidQueryUrl);
        }
    }
}
