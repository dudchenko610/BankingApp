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

        private IAuthenticationService _authenticationService { get; set; }

        private INavigationWrapper _navigationWrapper { get; set; }

        private ILoaderService _loaderService { get; set; }

        private IToastService _toastService { get; set; }

        public ResetPasswordPageTests()
        {
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            _authenticationService = authenticationServiceMock.Object;

            var loaderServiceMock = new Mock<ILoaderService>();
            loaderServiceMock.Setup(x => x.SwitchOn());
            loaderServiceMock.Setup(x => x.SwitchOff());
            _loaderService = loaderServiceMock.Object;

            var navWrapperMock = new Mock<INavigationWrapper>();
            navWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), false)).Verifiable();
            navWrapperMock.Setup(x => x.ToAbsoluteUri(It.IsAny<string>())).Returns(GetValidUri());
            _navigationWrapper = navWrapperMock.Object;

            var toastServiceMock = new Mock<IToastService>();
            _toastService = toastServiceMock.Object;
        }

        [Fact]
        public void ResetPasswordPage_UserSubmitsValidData_CallbacksTriggerAndReturnValidData()
        {
            bool switchOnCalled = false;
            bool switchOffCalled = false;
            ResetPasswordAuthenticationView resetPasswordViewSentToServer = null;
            string navigateToUri = null;
            string notificationMessage = null;
            string toAbsoluteUriParameter = null;

            var authenticationServiceMock = new Mock<IAuthenticationService>();
            authenticationServiceMock.Setup(x => x.ResetPasswordAsync(It.IsAny<ResetPasswordAuthenticationView>()))
                .Callback((ResetPasswordAuthenticationView view) => { resetPasswordViewSentToServer = view; })
                .ReturnsAsync(true);

            var loaderServiceMock = new Mock<ILoaderService>();
            loaderServiceMock.Setup(x => x.SwitchOn()).Callback(() => { switchOnCalled = true; });
            loaderServiceMock.Setup(x => x.SwitchOff()).Callback(() => { switchOffCalled = true; });

            var navWrapperMock = new Mock<INavigationWrapper>();
            navWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), false)).Callback((string uri, bool force) => { navigateToUri = uri; });
            navWrapperMock.Setup(x => x.ToAbsoluteUri(It.IsAny<string>())).Callback((string uri) => { toAbsoluteUriParameter = uri; }).Returns(GetValidUri());
            navWrapperMock.Setup(x => x.Uri).Returns(ValidQueryUrl);

            var toastServiceMock = new Mock<IToastService>();
            toastServiceMock.Setup(x => x.ShowSuccess(It.IsAny<string>(), It.IsAny<string>())).Callback((string message, string heading) => { notificationMessage = message; });

            Services.AddSingleton(authenticationServiceMock.Object);
            Services.AddSingleton(loaderServiceMock.Object);
            Services.AddSingleton(navWrapperMock.Object);
            Services.AddSingleton(toastServiceMock.Object);

            var validResetPasswordView = GetValidResetPasswordView();
            var resetPasswordForm = RenderComponent<ResetPasswordPage>();

            resetPasswordForm.Find("input[id=password]").Change(validResetPasswordView.Password.ToString());
            resetPasswordForm.Find("input[id=confirmPassword]").Change(validResetPasswordView.ConfirmPassword.ToString());
            resetPasswordForm.Find("form").Submit();

            toAbsoluteUriParameter.Should().BeEquivalentTo(navWrapperMock.Object.Uri);
            switchOnCalled.Should().BeTrue();
            switchOffCalled.Should().BeTrue();
            resetPasswordViewSentToServer.Should().NotBeNull();
            resetPasswordViewSentToServer.Password.Should().Be(validResetPasswordView.Password);
            resetPasswordViewSentToServer.ConfirmPassword.Should().Be(validResetPasswordView.ConfirmPassword);
            navigateToUri.Should().Be(Routes.SignInPage);
            notificationMessage.Should().Be(Notifications.PasswordResetSuccessfully);
        }

        [Fact]
        public void ResetPasswordPage_UserSubmitsValidDataButResetPasswordAsyncDidNotSendMessage_CallbacksTriggerAndReturnValidData()
        {
            bool switchOnCalled = false;
            bool switchOffCalled = false;
            ResetPasswordAuthenticationView resetPasswordViewSentToServer = null;
            string navigateToUri = null;
            string notificationMessage = null;
            string toAbsoluteUriParameter = null;

            var authenticationServiceMock = new Mock<IAuthenticationService>();
            authenticationServiceMock.Setup(x => x.ResetPasswordAsync(It.IsAny<ResetPasswordAuthenticationView>()))
                .Callback((ResetPasswordAuthenticationView view) => { resetPasswordViewSentToServer = view; })
                .ReturnsAsync(false);

            var loaderServiceMock = new Mock<ILoaderService>();
            loaderServiceMock.Setup(x => x.SwitchOn()).Callback(() => { switchOnCalled = true; });
            loaderServiceMock.Setup(x => x.SwitchOff()).Callback(() => { switchOffCalled = true; });

            var navWrapperMock = new Mock<INavigationWrapper>();
            navWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), false)).Callback((string uri, bool force) => { navigateToUri = uri; });
            navWrapperMock.Setup(x => x.ToAbsoluteUri(It.IsAny<string>())).Callback((string uri) => { toAbsoluteUriParameter = uri; }).Returns(GetValidUri());
            navWrapperMock.Setup(x => x.Uri).Returns(ValidQueryUrl);

            var toastServiceMock = new Mock<IToastService>();
            toastServiceMock.Setup(x => x.ShowSuccess(It.IsAny<string>(), It.IsAny<string>())).Callback((string message, string heading) => { notificationMessage = message; });

            Services.AddSingleton(authenticationServiceMock.Object);
            Services.AddSingleton(loaderServiceMock.Object);
            Services.AddSingleton(navWrapperMock.Object);
            Services.AddSingleton(toastServiceMock.Object);

            var validResetPasswordView = GetValidResetPasswordView();
            var resetPasswordForm = RenderComponent<ResetPasswordPage>();

            resetPasswordForm.Find("input[id=password]").Change(validResetPasswordView.Password.ToString());
            resetPasswordForm.Find("input[id=confirmPassword]").Change(validResetPasswordView.ConfirmPassword.ToString());
            resetPasswordForm.Find("form").Submit();

            toAbsoluteUriParameter.Should().BeEquivalentTo(navWrapperMock.Object.Uri);
            switchOnCalled.Should().BeTrue();
            switchOffCalled.Should().BeTrue();
            resetPasswordViewSentToServer.Password.Should().Be(validResetPasswordView.Password);
            resetPasswordViewSentToServer.ConfirmPassword.Should().Be(validResetPasswordView.ConfirmPassword);
            navigateToUri.Should().BeNull();
            notificationMessage.Should().BeNull();
        }

        [Fact]
        public void ResetPasswordPage_UserSubmitsValidData_NoErrorMessagesWereShown()
        {
            Services.AddSingleton(_authenticationService);
            Services.AddSingleton(_loaderService);
            Services.AddSingleton(_navigationWrapper);
            Services.AddSingleton(_toastService);

            var validResetPasswordView = GetValidResetPasswordView();
            var resetPasswordForm = RenderComponent<ResetPasswordPage>();

            resetPasswordForm.Find("input[id=password]").Change(validResetPasswordView.Password.ToString());
            resetPasswordForm.Find("input[id=confirmPassword]").Change(validResetPasswordView.ConfirmPassword.ToString());
            resetPasswordForm.Find("form").Submit();

            var validationErrorMessage = resetPasswordForm.FindAll("div[class=validation-message]");
            validationErrorMessage.Should().BeEmpty();
        }

        [Fact]
        public void ResetPasswordPage_PasswwordIsTooShort_CorrespondingErrorMessageWasShown()
        {
            Services.AddSingleton(_authenticationService);
            Services.AddSingleton(_loaderService);
            Services.AddSingleton(_navigationWrapper);
            Services.AddSingleton(_toastService);

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
        public void ResetPasswordPage_PasswwordDoesNotMatchesRegularExpression_CorrespondingErrorMessageWasShown()
        {
            Services.AddSingleton(_authenticationService);
            Services.AddSingleton(_loaderService);
            Services.AddSingleton(_navigationWrapper);
            Services.AddSingleton(_toastService);

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
        public void SignUpPage_PasswwordAndConfirmPasswordDoesNotMatch_CorrespondingErrorMessageWasShown()
        {
            Services.AddSingleton(_authenticationService);
            Services.AddSingleton(_loaderService);
            Services.AddSingleton(_navigationWrapper);
            Services.AddSingleton(_toastService);

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
