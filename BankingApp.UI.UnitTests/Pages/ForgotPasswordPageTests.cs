using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Pages.ForgotPasswordPage;
using Blazored.Toast.Services;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using FluentAssertions;
using static BankingApp.UI.Core.Constants.Constants;
using BankingApp.Shared;
using BankingApp.ViewModels.ViewModels.Authentication;

namespace BankingApp.UI.UnitTests.Pages
{
    public class ForgotPasswordPageTests : TestContext
    {
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;
        private readonly Mock<INavigationWrapper> _navigationWrapperMock;
        private readonly Mock<ILoaderService> _loaderServiceMock;
        private readonly Mock<IToastService> _toastServiceMock;

        public ForgotPasswordPageTests()
        {
            _authenticationServiceMock = new Mock<IAuthenticationService>();

            _loaderServiceMock = new Mock<ILoaderService>();
            _loaderServiceMock.Setup(x => x.SwitchOn());
            _loaderServiceMock.Setup(x => x.SwitchOff());

            _navigationWrapperMock = new Mock<INavigationWrapper>();
            _navigationWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), false)).Verifiable();
            _navigationWrapperMock.Setup(x => x.ToBaseRelativePath(It.IsAny<string>())).Returns(string.Empty);

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
            ForgotPasswordAuthenticationView forgotPasswordViewSentToServer = null;
            string navigateToUri = null;
            string notificationMessage = null;

            _authenticationServiceMock.Setup(x => x.ForgotPasswordAsync(It.IsAny<ForgotPasswordAuthenticationView>()))
                .Callback((ForgotPasswordAuthenticationView view) => { forgotPasswordViewSentToServer = view; })
                .ReturnsAsync(true);
            _loaderServiceMock.Setup(x => x.SwitchOn()).Callback(() => { switchOnCalled = true; });
            _loaderServiceMock.Setup(x => x.SwitchOff()).Callback(() => { switchOffCalled = true; });
            _navigationWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), false)).Callback((string uri, bool force) => { navigateToUri = uri; });
            _toastServiceMock.Setup(x => x.ShowSuccess(It.IsAny<string>(), It.IsAny<string>())).Callback((string message, string heading) => { notificationMessage = message; });

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
        public void WhenTheFormIsSubmited_SendMessageFailure_ExpectedResults()
        {
            bool switchOnCalled = false;
            bool switchOffCalled = false;
            ForgotPasswordAuthenticationView forgotPasswordViewSentToServer = null;
            string navigateToUri = null;
            string notificationMessage = null;

            _authenticationServiceMock.Setup(x => x.ForgotPasswordAsync(It.IsAny<ForgotPasswordAuthenticationView>()))
                .Callback((ForgotPasswordAuthenticationView view) => { forgotPasswordViewSentToServer = view; })
                .ReturnsAsync(false);

            _loaderServiceMock.Setup(x => x.SwitchOn()).Callback(() => { switchOnCalled = true; });
            _loaderServiceMock.Setup(x => x.SwitchOff()).Callback(() => { switchOffCalled = true; });

            _navigationWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), false)).Callback((string uri, bool force) => { navigateToUri = uri; });

            _toastServiceMock.Setup(x => x.ShowSuccess(It.IsAny<string>(), It.IsAny<string>())).Callback((string message, string heading) => { notificationMessage = message; });

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
        public void WhenTheComponentIsRendered_ValidData_ExpectedMarkupRendered()
        {
            var validForgotPasswordView = GetValidForgotPasswordView();
            var forgotPasswordForm = RenderComponent<ForgotPasswordPage>();

            forgotPasswordForm.Find("input[id=email]").Change(validForgotPasswordView.Email.ToString());
            forgotPasswordForm.Find("form").Submit();

            var validationErrorMessage = forgotPasswordForm.FindAll("div[class=validation-message]");
            validationErrorMessage.Should().BeEmpty();
        }

        [Fact]
        public void WhenTheComponentIsRendered_DataWithEmptyEmail_ExpectedMarkupRendered()
        {
            var validForgotPasswordView = GetValidForgotPasswordView();
            validForgotPasswordView.Email = string.Empty;

            var forgotPasswordForm = RenderComponent<ForgotPasswordPage>();

            forgotPasswordForm.Find("input[id=email]").Change(validForgotPasswordView.Email.ToString());
            forgotPasswordForm.Find("form").Submit();

            var validationErrorMessage = forgotPasswordForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Authentication.EmailIsRequired);
        }

        [Fact]
        public void WhenTheComponentIsRendered_DataWithInvalidFormatEmail_ExpectedMarkupRendered()
        {
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
