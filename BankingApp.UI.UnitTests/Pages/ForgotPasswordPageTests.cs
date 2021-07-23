using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Pages.ForgotPasswordPage;
using BankingApp.ViewModels.Banking.Authentication;
using Blazored.Toast.Services;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using FluentAssertions;
using static BankingApp.UI.Core.Constants.Constants;
using BankingApp.Shared;

namespace BankingApp.UI.UnitTests.Pages
{
    public class ForgotPasswordPageTests : TestContext
    {
        private IAuthenticationService _authenticationService { get; set; }

        private INavigationWrapper _navigationWrapper { get; set; }

        private ILoaderService _loaderService { get; set; }

        private IToastService _toastService { get; set; }

        public ForgotPasswordPageTests()
        {
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            _authenticationService = authenticationServiceMock.Object;

            var loaderServiceMock = new Mock<ILoaderService>();
            loaderServiceMock.Setup(x => x.SwitchOn());
            loaderServiceMock.Setup(x => x.SwitchOff());
            _loaderService = loaderServiceMock.Object;

            var navWrapperMock = new Mock<INavigationWrapper>();
            navWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), false)).Verifiable();
            navWrapperMock.Setup(x => x.ToBaseRelativePath(It.IsAny<string>())).Returns("");
            _navigationWrapper = navWrapperMock.Object;

            var toastServiceMock = new Mock<IToastService>();
            _toastService = toastServiceMock.Object;
        }

        [Fact]
        public void ForgotPasswordPage_UserSubmitsValidData_CallbacksTriggerAndReturnValidData()
        {
            bool switchOnCalled = false;
            bool switchOffCalled = false;
            ForgotPasswordAuthenticationView forgotPasswordViewSentToServer = null;
            string navigateToUri = null;
            string notificationMessage = null;

            var authenticationServiceMock = new Mock<IAuthenticationService>();
            authenticationServiceMock.Setup(x => x.ForgotPasswordAsync(It.IsAny<ForgotPasswordAuthenticationView>()))
                .Callback((ForgotPasswordAuthenticationView view) => { forgotPasswordViewSentToServer = view; })
                .ReturnsAsync(true);

            var loaderServiceMock = new Mock<ILoaderService>();
            loaderServiceMock.Setup(x => x.SwitchOn()).Callback(() => { switchOnCalled = true; });
            loaderServiceMock.Setup(x => x.SwitchOff()).Callback(() => { switchOffCalled = true; });

            var navWrapperMock = new Mock<INavigationWrapper>();
            navWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), false)).Callback((string uri, bool force) => { navigateToUri = uri; });

            var toastServiceMock = new Mock<IToastService>();
            toastServiceMock.Setup(x => x.ShowSuccess(It.IsAny<string>(), It.IsAny<string>())).Callback((string message, string heading) => { notificationMessage = message; });

            Services.AddSingleton(authenticationServiceMock.Object);
            Services.AddSingleton(loaderServiceMock.Object);
            Services.AddSingleton(navWrapperMock.Object);
            Services.AddSingleton(toastServiceMock.Object);

            var validForgotPasswordView = GetValidForgotPasswordView();
            var forgotPasswordForm = RenderComponent<ForgotPasswordPage>();

            forgotPasswordForm.Find("input[id=email]").Change(validForgotPasswordView.Email.ToString());
            forgotPasswordForm.Find("form").Submit();

            switchOnCalled.Should().BeTrue();
            switchOffCalled.Should().BeTrue();
            forgotPasswordViewSentToServer.Should().NotBeNull().And.BeEquivalentTo(validForgotPasswordView);
            navigateToUri.Should().Be(Routes.SignInPage);
            notificationMessage.Should().Be(Notifications.PasswordResetEmailSent);
        }

        [Fact]
        public void ForgotPasswordPage_UserSubmitsValidDataButForgotPasswordAsyncDidNotSendMessage_CallbacksTriggerAndReturnValidData()
        {
            bool switchOnCalled = false;
            bool switchOffCalled = false;
            ForgotPasswordAuthenticationView forgotPasswordViewSentToServer = null;
            string navigateToUri = null;
            string notificationMessage = null;

            var authenticationServiceMock = new Mock<IAuthenticationService>();
            authenticationServiceMock.Setup(x => x.ForgotPasswordAsync(It.IsAny<ForgotPasswordAuthenticationView>()))
                .Callback((ForgotPasswordAuthenticationView view) => { forgotPasswordViewSentToServer = view; })
                .ReturnsAsync(false);

            var loaderServiceMock = new Mock<ILoaderService>();
            loaderServiceMock.Setup(x => x.SwitchOn()).Callback(() => { switchOnCalled = true; });
            loaderServiceMock.Setup(x => x.SwitchOff()).Callback(() => { switchOffCalled = true; });

            var navWrapperMock = new Mock<INavigationWrapper>();
            navWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), false)).Callback((string uri, bool force) => { navigateToUri = uri; });

            var toastServiceMock = new Mock<IToastService>();
            toastServiceMock.Setup(x => x.ShowSuccess(It.IsAny<string>(), It.IsAny<string>())).Callback((string message, string heading) => { notificationMessage = message; });

            Services.AddSingleton(authenticationServiceMock.Object);
            Services.AddSingleton(loaderServiceMock.Object);
            Services.AddSingleton(navWrapperMock.Object);
            Services.AddSingleton(toastServiceMock.Object);

            var validForgotPasswordView = GetValidForgotPasswordView();
            var forgotPasswordForm = RenderComponent<ForgotPasswordPage>();

            forgotPasswordForm.Find("input[id=email]").Change(validForgotPasswordView.Email.ToString());
            forgotPasswordForm.Find("form").Submit();

            switchOnCalled.Should().BeTrue();
            switchOffCalled.Should().BeTrue();
            forgotPasswordViewSentToServer.Should().NotBeNull().And.BeEquivalentTo(validForgotPasswordView);
            navigateToUri.Should().BeNull();
            notificationMessage.Should().BeNull();
        }

        [Fact]
        public void ForgotPasswordPage_UserSubmitsValidData_NoErrorMessagesWereShown()
        {
            Services.AddSingleton(_authenticationService);
            Services.AddSingleton(_loaderService);
            Services.AddSingleton(_navigationWrapper);
            Services.AddSingleton(_toastService);

            var validForgotPasswordView = GetValidForgotPasswordView();
            var forgotPasswordForm = RenderComponent<ForgotPasswordPage>();

            forgotPasswordForm.Find("input[id=email]").Change(validForgotPasswordView.Email.ToString());
            forgotPasswordForm.Find("form").Submit();

            var validationErrorMessage = forgotPasswordForm.FindAll("div[class=validation-message]");
            validationErrorMessage.Should().BeEmpty();
        }

        [Fact]
        public void ForgotPasswordPage_UserSubmitsDataWithEmptyEmail_CorrespondingErrorMessageWasShown()
        {
            Services.AddSingleton(_authenticationService);
            Services.AddSingleton(_loaderService);
            Services.AddSingleton(_navigationWrapper);
            Services.AddSingleton(_toastService);

            var validForgotPasswordView = GetValidForgotPasswordView();
            validForgotPasswordView.Email = "";

            var forgotPasswordForm = RenderComponent<ForgotPasswordPage>();

            forgotPasswordForm.Find("input[id=email]").Change(validForgotPasswordView.Email.ToString());
            forgotPasswordForm.Find("form").Submit();

            var validationErrorMessage = forgotPasswordForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Authentication.EmailIsRequired);
        }

        [Fact]
        public void ForgotPasswordPage_UserSubmitsDataWithInvalidFormatEmail_CorrespondingErrorMessageWasShown()
        {
            Services.AddSingleton(_authenticationService);
            Services.AddSingleton(_loaderService);
            Services.AddSingleton(_navigationWrapper);
            Services.AddSingleton(_toastService);

            var validForgotPasswordView = GetValidForgotPasswordView();
            validForgotPasswordView.Email = "ksfdsfk";

            var forgotPasswordForm = RenderComponent<ForgotPasswordPage>();

            forgotPasswordForm.Find("input[id=email]").Change(validForgotPasswordView.Email.ToString());
            forgotPasswordForm.Find("form").Submit();

            var validationErrorMessage = forgotPasswordForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Authentication.EmailFormatIncorrect);
        }

        private ForgotPasswordAuthenticationView GetValidForgotPasswordView()
        {
            return new ForgotPasswordAuthenticationView
            {
                Email = "rusland610@gmail.com"
            };
        }
    }
}
