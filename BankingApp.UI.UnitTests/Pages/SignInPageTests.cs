using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Pages.SignInPage;
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
    public class SignInPageTests : TestContext
    {
        private IAuthenticationService _authenticationService { get; set; }

        private INavigationWrapper _navigationWrapper { get; set; }

        private ILoaderService _loaderService { get; set; }

        private IToastService _toastService { get; set; }

        public SignInPageTests()
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
        public void SignInPage_UserSubmitsValidData_CallbacksTriggerAndReturnValidData()
        {
            bool switchOnCalled = false;
            bool switchOffCalled = false;
            SignInAuthenticationView signInViewSentToServer = null;
            string navigateToUri = null;
            string notificationMessage = null;

            var authenticationServiceMock = new Mock<IAuthenticationService>();
            authenticationServiceMock.Setup(x => x.SignInAsync(It.IsAny<SignInAuthenticationView>()))
                .Callback((SignInAuthenticationView view) => { signInViewSentToServer = view; })
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

            var validSignInForm = GetValidSignInView();
            var signInForm = RenderComponent<SignInPage>();

            signInForm.Find("input[id=email]").Change(validSignInForm.Email.ToString());
            signInForm.Find("input[id=password]").Change(validSignInForm.Password.ToString());
            signInForm.Find("form").Submit();

            switchOnCalled.Should().BeTrue();
            switchOffCalled.Should().BeTrue();
            signInViewSentToServer.Should().NotBeNull().And.BeEquivalentTo(validSignInForm);
            navigateToUri.Should().Be(Routes.MainPage);
            notificationMessage.Should().Be(Notifications.SignInSuccess);
        }

        [Fact]
        public void SignInPage_UserSubmitsValidDataButSignInAsyncDidNotSendMessage_CallbacksTriggerAndReturnValidData()
        {
            bool switchOnCalled = false;
            bool switchOffCalled = false;
            SignInAuthenticationView signInViewSentToServer = null;
            string navigateToUri = null;
            string notificationMessage = null;

            var authenticationServiceMock = new Mock<IAuthenticationService>();
            authenticationServiceMock.Setup(x => x.SignInAsync(It.IsAny<SignInAuthenticationView>()))
                .Callback((SignInAuthenticationView view) => { signInViewSentToServer = view; })
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

            var validSignInForm = GetValidSignInView();
            var signInForm = RenderComponent<SignInPage>();

            signInForm.Find("input[id=email]").Change(validSignInForm.Email.ToString());
            signInForm.Find("input[id=password]").Change(validSignInForm.Password.ToString());
            signInForm.Find("form").Submit();

            switchOnCalled.Should().BeTrue();
            switchOffCalled.Should().BeTrue();
            signInViewSentToServer.Should().NotBeNull().And.BeEquivalentTo(validSignInForm);
            navigateToUri.Should().BeNull();
            notificationMessage.Should().BeNull();
        }

        [Fact]
        public void SignInPage_UserSubmitsValidData_NoErrorMessagesWereShown()
        {
            Services.AddSingleton(_authenticationService);
            Services.AddSingleton(_loaderService);
            Services.AddSingleton(_navigationWrapper);
            Services.AddSingleton(_toastService);

            var validSignInView = GetValidSignInView();
            var signInForm = RenderComponent<SignInPage>();

            signInForm.Find("input[id=email]").Change(validSignInView.Email.ToString());
            signInForm.Find("input[id=password]").Change(validSignInView.Password.ToString());
            signInForm.Find("form").Submit();

            var validationErrorMessage = signInForm.FindAll("div[class=validation-message]");
            validationErrorMessage.Should().BeEmpty();
        }

        [Fact]
        public void SignInPage_UserSubmitsDataWithEmptyEmail_CorrespondingErrorMessageWasShown()
        {
            Services.AddSingleton(_authenticationService);
            Services.AddSingleton(_loaderService);
            Services.AddSingleton(_navigationWrapper);
            Services.AddSingleton(_toastService);

            var validSignInView = GetValidSignInView();
            validSignInView.Email = "";

            var signInForm = RenderComponent<SignInPage>();

            signInForm.Find("input[id=email]").Change(validSignInView.Email.ToString());
            signInForm.Find("input[id=password]").Change(validSignInView.Password.ToString());
            signInForm.Find("form").Submit();

            var validationErrorMessage = signInForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Authentication.EmailIsRequired);
        }

        [Fact]
        public void SignInPage_UserSubmitsDataWithInvalidFormatEmail_CorrespondingErrorMessageWasShown()
        {
            Services.AddSingleton(_authenticationService);
            Services.AddSingleton(_loaderService);
            Services.AddSingleton(_navigationWrapper);
            Services.AddSingleton(_toastService);

            var validSignInView = GetValidSignInView();
            validSignInView.Email = "ksfdsfk";

            var signInForm = RenderComponent<SignInPage>();

            signInForm.Find("input[id=email]").Change(validSignInView.Email.ToString());
            signInForm.Find("input[id=password]").Change(validSignInView.Password.ToString());
            signInForm.Find("form").Submit();

            var validationErrorMessage = signInForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Authentication.EmailFormatIncorrect);
        }

        [Fact]
        public void SignInPage_PasswwordIsTooShort_CorrespondingErrorMessageWasShown()
        {
            Services.AddSingleton(_authenticationService);
            Services.AddSingleton(_loaderService);
            Services.AddSingleton(_navigationWrapper);
            Services.AddSingleton(_toastService);

            var validSignInView = GetValidSignInView();
            validSignInView.Password = "abc";

            var signInForm = RenderComponent<SignInPage>();

            signInForm.Find("input[id=email]").Change(validSignInView.Email.ToString());
            signInForm.Find("input[id=password]").Change(validSignInView.Password.ToString());
            signInForm.Find("form").Submit();

            var validationErrorMessage = signInForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Authentication.PasswordIsTooShort);
        }

        [Fact]
        public void SignUpPage_PasswwordDoesNotMatchesRegularExpression_CorrespondingErrorMessageWasShown()
        {
            Services.AddSingleton(_authenticationService);
            Services.AddSingleton(_loaderService);
            Services.AddSingleton(_navigationWrapper);
            Services.AddSingleton(_toastService);

            var validSignInView = GetValidSignInView();
            validSignInView.Password = "abcdAAABBBBBBCCCCCC";

            var signInForm = RenderComponent<SignInPage>();

            signInForm.Find("input[id=email]").Change(validSignInView.Email.ToString());
            signInForm.Find("input[id=password]").Change(validSignInView.Password.ToString());
            signInForm.Find("form").Submit();

            var validationErrorMessage = signInForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Authentication.PasswordIsNotHardEnough);
        }

        private SignInAuthenticationView GetValidSignInView()
        {
            return new SignInAuthenticationView
            {
                Email = "rusland610@gmail.com",
                Password = "qwerty12345AAA"
            };
        }
    }
}
