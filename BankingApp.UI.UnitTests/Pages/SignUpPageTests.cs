using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Pages.SignUpPage;
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
    public class SignUpPageTests : TestContext
    {
        private readonly Mock<IAuthenticationService> _authenticationServiceMock; 
        private readonly Mock<INavigationWrapper> _navigationWrapperMock;
        private readonly Mock<ILoaderService> _loaderServiceMock;     
        private readonly Mock<IToastService> _toastServiceMock;

        public SignUpPageTests()
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
            SignUpAuthenticationView signUpViewSentToServer = null;
            string navigateToUri = null;
            string notificationMessage = null;

            _authenticationServiceMock.Setup(x => x.SignUpAsync(It.IsAny<SignUpAuthenticationView>()))
                .Callback((SignUpAuthenticationView view) => { signUpViewSentToServer = view; })
                .ReturnsAsync(true);

            _loaderServiceMock.Setup(x => x.SwitchOn()).Callback(() => { switchOnCalled = true; });
            _loaderServiceMock.Setup(x => x.SwitchOff()).Callback(() => { switchOffCalled = true; });
            _navigationWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), false)).Callback((string uri, bool force) => { navigateToUri = uri; });
            _toastServiceMock.Setup(x => x.ShowSuccess(It.IsAny<string>(), It.IsAny<string>())).Callback((string message, string heading) => { notificationMessage = message; });

            var validSignUpView = GetValidSignUpView();
            var signUpForm = RenderComponent<SignUpPage>();

            signUpForm.Find("input[id=nickname]").Change(validSignUpView.Nickname.ToString());
            signUpForm.Find("input[id=email]").Change(validSignUpView.Email.ToString());
            signUpForm.Find("input[id=password]").Change(validSignUpView.Password.ToString());
            signUpForm.Find("input[id=confirmPassword]").Change(validSignUpView.ConfirmPassword.ToString());
            signUpForm.Find("form").Submit();

            switchOnCalled.Should().BeTrue();
            switchOffCalled.Should().BeTrue();
            signUpViewSentToServer.Should().NotBeNull().And.BeEquivalentTo(validSignUpView);
            navigateToUri.Should().Be(Routes.SignInPage);
            notificationMessage.Should().Be(Notifications.ConfirmYourEmail);
        }

        [Fact]
        public void WhenTheFormIsSubmited_SendMessageFailure_ExpectedResults()
        {
            bool switchOnCalled = false;
            bool switchOffCalled = false;
            SignUpAuthenticationView signUpViewSentToServer = null;
            string navigateToUri = null;
            string notificationMessage = null;

            _authenticationServiceMock.Setup(x => x.SignUpAsync(It.IsAny<SignUpAuthenticationView>()))
                .Callback((SignUpAuthenticationView view) => { signUpViewSentToServer = view; })
                .ReturnsAsync(false);

            _loaderServiceMock.Setup(x => x.SwitchOn()).Callback(() => { switchOnCalled = true; });
            _loaderServiceMock.Setup(x => x.SwitchOff()).Callback(() => { switchOffCalled = true; });
            _navigationWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), false)).Callback((string uri, bool force) => { navigateToUri = uri; });
            _toastServiceMock.Setup(x => x.ShowSuccess(It.IsAny<string>(), It.IsAny<string>())).Callback((string message, string heading) => { notificationMessage = message; });

            var validSignUpView = GetValidSignUpView();
            var signUpForm = RenderComponent<SignUpPage>();

            signUpForm.Find("input[id=nickname]").Change(validSignUpView.Nickname.ToString());
            signUpForm.Find("input[id=email]").Change(validSignUpView.Email.ToString());
            signUpForm.Find("input[id=password]").Change(validSignUpView.Password.ToString());
            signUpForm.Find("input[id=confirmPassword]").Change(validSignUpView.ConfirmPassword.ToString());
            signUpForm.Find("form").Submit();

            switchOnCalled.Should().BeTrue();
            switchOffCalled.Should().BeTrue();
            signUpViewSentToServer.Should().NotBeNull().And.BeEquivalentTo(validSignUpView);
            navigateToUri.Should().BeNull();
            notificationMessage.Should().BeNull();
        }

        [Fact]
        public void WhenTheFormIsSubmited_ValidData_ExpectedMarkupRendered()
        {
            var validSignUpView = GetValidSignUpView();
            var signUpForm = RenderComponent<SignUpPage>();

            signUpForm.Find("input[id=nickname]").Change(validSignUpView.Nickname.ToString());
            signUpForm.Find("input[id=email]").Change(validSignUpView.Email.ToString());
            signUpForm.Find("input[id=password]").Change(validSignUpView.Password.ToString());
            signUpForm.Find("input[id=confirmPassword]").Change(validSignUpView.ConfirmPassword.ToString());
            signUpForm.Find("form").Submit();

            var validationErrorMessage = signUpForm.FindAll("div[class=validation-message]");
            validationErrorMessage.Should().BeEmpty();
        }

        [Fact]
        public void WhenTheFormIsSubmited_DataWithEmptyNickname_ExpectedMarkupRendered()
        {
            var validSignUpView = GetValidSignUpView();
            validSignUpView.Nickname = string.Empty;

            var signUpForm = RenderComponent<SignUpPage>();

            signUpForm.Find("input[id=nickname]").Change(validSignUpView.Nickname.ToString());
            signUpForm.Find("input[id=email]").Change(validSignUpView.Email.ToString());
            signUpForm.Find("input[id=password]").Change(validSignUpView.Password.ToString());
            signUpForm.Find("input[id=confirmPassword]").Change(validSignUpView.ConfirmPassword.ToString());
            signUpForm.Find("form").Submit();

            var validationErrorMessage = signUpForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Authentication.NicknameIsRequired);
        }

        [Fact]
        public void WhenTheFormIsSubmited_DataWithTooLongNickname_ExpectedMarkupRendered()
        {
            var validSignUpView = GetValidSignUpView();
            validSignUpView.Nickname = "123456789abcd";

            var signUpForm = RenderComponent<SignUpPage>();

            signUpForm.Find("input[id=nickname]").Change(validSignUpView.Nickname.ToString());
            signUpForm.Find("input[id=email]").Change(validSignUpView.Email.ToString());
            signUpForm.Find("input[id=password]").Change(validSignUpView.Password.ToString());
            signUpForm.Find("input[id=confirmPassword]").Change(validSignUpView.ConfirmPassword.ToString());
            signUpForm.Find("form").Submit();

            var validationErrorMessage = signUpForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Authentication.NicknameLengthIsTooLong);
        }

        [Fact]
        public void WhenTheFormIsSubmited_DataWithEmptyEmail_ExpectedMarkupRendered()
        {
            var validSignUpView = GetValidSignUpView();
            validSignUpView.Email = string.Empty;

            var signUpForm = RenderComponent<SignUpPage>();

            signUpForm.Find("input[id=nickname]").Change(validSignUpView.Nickname.ToString());
            signUpForm.Find("input[id=email]").Change(validSignUpView.Email.ToString());
            signUpForm.Find("input[id=password]").Change(validSignUpView.Password.ToString());
            signUpForm.Find("input[id=confirmPassword]").Change(validSignUpView.ConfirmPassword.ToString());
            signUpForm.Find("form").Submit();

            var validationErrorMessage = signUpForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Authentication.EmailIsRequired);
        }

        [Fact]
        public void WhenTheFormIsSubmited_DataWithInvalidFormatEmail_ExpectedMarkupRendered()
        {
            var validSignUpView = GetValidSignUpView();
            validSignUpView.Email = "ksfdsfk";

            var signUpForm = RenderComponent<SignUpPage>();

            signUpForm.Find("input[id=nickname]").Change(validSignUpView.Nickname.ToString());
            signUpForm.Find("input[id=email]").Change(validSignUpView.Email.ToString());
            signUpForm.Find("input[id=password]").Change(validSignUpView.Password.ToString());
            signUpForm.Find("input[id=confirmPassword]").Change(validSignUpView.ConfirmPassword.ToString());
            signUpForm.Find("form").Submit();

            var validationErrorMessage = signUpForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Authentication.EmailFormatIncorrect);
        }

        [Fact]
        public void WhenTheFormIsSubmited_PasswwordIsTooShort_ExpectedMarkupRendered()
        {
            var validSignUpView = GetValidSignUpView();
            validSignUpView.Password = "abc";
            validSignUpView.ConfirmPassword = "abc";

            var signUpForm = RenderComponent<SignUpPage>();

            signUpForm.Find("input[id=nickname]").Change(validSignUpView.Nickname.ToString());
            signUpForm.Find("input[id=email]").Change(validSignUpView.Email.ToString());
            signUpForm.Find("input[id=password]").Change(validSignUpView.Password.ToString());
            signUpForm.Find("input[id=confirmPassword]").Change(validSignUpView.ConfirmPassword.ToString());
            signUpForm.Find("form").Submit();

            var validationErrorMessage = signUpForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Authentication.PasswordIsTooShort);
        }

        [Fact]
        public void WhenTheFormIsSubmited_PasswwordDoesNotMatchesRegularExpression_ExpectedMarkupRendered()
        {
            var validSignUpView = GetValidSignUpView();
            validSignUpView.Password = "abcdAAABBBBBBCCCCCC";
            validSignUpView.ConfirmPassword = "abcdAAABBBBBBCCCCCC";

            var signUpForm = RenderComponent<SignUpPage>();

            signUpForm.Find("input[id=nickname]").Change(validSignUpView.Nickname.ToString());
            signUpForm.Find("input[id=email]").Change(validSignUpView.Email.ToString());
            signUpForm.Find("input[id=password]").Change(validSignUpView.Password.ToString());
            signUpForm.Find("input[id=confirmPassword]").Change(validSignUpView.ConfirmPassword.ToString());
            signUpForm.Find("form").Submit();

            var validationErrorMessage = signUpForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Authentication.PasswordIsNotHardEnough);
        }

        [Fact]
        public void WhenTheFormIsSubmited_PasswwordAndConfirmPasswordDoesNotMatch_ExpectedMarkupRendered()
        {
            var validSignUpView = GetValidSignUpView();
            validSignUpView.Password = "abcdAAABBBBBBCCCCCC12345";
            validSignUpView.ConfirmPassword = "abcdAAABBBBBBCCCCCC1234";

            var signUpForm = RenderComponent<SignUpPage>();

            signUpForm.Find("input[id=nickname]").Change(validSignUpView.Nickname.ToString());
            signUpForm.Find("input[id=email]").Change(validSignUpView.Email.ToString());
            signUpForm.Find("input[id=password]").Change(validSignUpView.Password.ToString());
            signUpForm.Find("input[id=confirmPassword]").Change(validSignUpView.ConfirmPassword.ToString());
            signUpForm.Find("form").Submit();

            var validationErrorMessage = signUpForm.Find("div[class=validation-message]").TextContent;
            validationErrorMessage.Should().Be(Constants.Errors.Authentication.ConfirmPasswordShouldMatchPassword);
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
    }
}
