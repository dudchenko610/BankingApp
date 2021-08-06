using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Pages.SignInPage;
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
    public class SignInPageTests : TestContext
    {
        private Mock<IAuthenticationService> _authenticationServiceMock { get; set; }

        private Mock<INavigationWrapper> _navigationWrapperMock { get; set; }

        private Mock<ILoaderService> _loaderServiceMock { get; set; }

        private Mock<IToastService> _toastServiceMock { get; set; }

        public SignInPageTests()
        {
            _authenticationServiceMock = new Mock<IAuthenticationService>();

            _loaderServiceMock = new Mock<ILoaderService>();
            _loaderServiceMock.Setup(x => x.SwitchOn());
            _loaderServiceMock.Setup(x => x.SwitchOff());

            _navigationWrapperMock = new Mock<INavigationWrapper>();
            _navigationWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), false)).Verifiable();
            _navigationWrapperMock.Setup(x => x.ToBaseRelativePath(It.IsAny<string>())).Returns("");

            _toastServiceMock = new Mock<IToastService>();

            Services.AddSingleton(_authenticationServiceMock.Object);
            Services.AddSingleton(_loaderServiceMock.Object);
            Services.AddSingleton(_navigationWrapperMock.Object);
            Services.AddSingleton(_toastServiceMock.Object);
        }

        [Fact]
        public void WhenTheFormIsSubmited_ValidData_SignInAsyncInvoked()
        {
            bool switchOnCalled = false;
            bool switchOffCalled = false;
            SignInAuthenticationView signInViewSentToServer = null;
            string navigateToUri = null;
            string notificationMessage = null;

            _authenticationServiceMock.Setup(x => x.SignInAsync(It.IsAny<SignInAuthenticationView>()))
                .Callback((SignInAuthenticationView view) => { signInViewSentToServer = view; })
                .ReturnsAsync(true);

            _loaderServiceMock.Setup(x => x.SwitchOn()).Callback(() => { switchOnCalled = true; });
            _loaderServiceMock.Setup(x => x.SwitchOff()).Callback(() => { switchOffCalled = true; });

            _navigationWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), false)).Callback((string uri, bool force) => { navigateToUri = uri; });
            _toastServiceMock.Setup(x => x.ShowSuccess(It.IsAny<string>(), It.IsAny<string>())).Callback((string message, string heading) => { notificationMessage = message; });

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
        public void WhenTheFormIsSubmited_SendMesssageFailure_ExpectedResults()
        {
            bool switchOnCalled = false;
            bool switchOffCalled = false;
            SignInAuthenticationView signInViewSentToServer = null;
            string navigateToUri = null;
            string notificationMessage = null;

            _authenticationServiceMock.Setup(x => x.SignInAsync(It.IsAny<SignInAuthenticationView>()))
                .Callback((SignInAuthenticationView view) => { signInViewSentToServer = view; })
                .ReturnsAsync(false);

            _loaderServiceMock.Setup(x => x.SwitchOn()).Callback(() => { switchOnCalled = true; });
            _loaderServiceMock.Setup(x => x.SwitchOff()).Callback(() => { switchOffCalled = true; });
            _navigationWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), false)).Callback((string uri, bool force) => { navigateToUri = uri; });
            _toastServiceMock.Setup(x => x.ShowSuccess(It.IsAny<string>(), It.IsAny<string>())).Callback((string message, string heading) => { notificationMessage = message; });

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
        public void WhenTheFormIsSubmited_ValidData_ExpectedMarkupRendered()
        {
            var validSignInView = GetValidSignInView();
            var signInForm = RenderComponent<SignInPage>();

            signInForm.Find("input[id=email]").Change(validSignInView.Email.ToString());
            signInForm.Find("input[id=password]").Change(validSignInView.Password.ToString());
            signInForm.Find("form").Submit();

            var validationErrorMessage = signInForm.FindAll("div[class=validation-message]");
            validationErrorMessage.Should().BeEmpty();
        }

        [Fact]
        public void WhenTheFormIsSubmited_DataWithEmptyEmail_ExpectedMarkupRendered()
        {
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
        public void WhenTheFormIsSubmited_DataWithInvalidFormatEmail_ExpectedMarkupRendered()
        {
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
        public void WhenTheFormIsSubmited_PasswwordIsTooShort_ExpectedMarkupRendered()
        {
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
        public void WhenTheFormIsSubmited_PasswwordDoesNotMatchesRegularExpression_ExpectedMarkupRendered()
        {
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
