using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Pages.SignUpPage;
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
    public class SignUpPageTests : TestContext
    {
        private IAuthenticationService _authenticationService { get; set; }
        
        private INavigationWrapper _navigationWrapper { get; set; }
        
        private ILoaderService _loaderService { get; set; }
        
        private IToastService _toastService { get; set; }

        public SignUpPageTests()
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
        public void SignUpPage_UserSubmitsValidData_CallbacksTriggerAndReturnValidData()
        {
            bool switchOnCalled = false;
            bool switchOffCalled = false;
            SignUpAuthenticationView signUpViewSentToServer = null;
            string navigateToUri = null;
            string notificationMessage = null;

            var authenticationServiceMock = new Mock<IAuthenticationService>();
            authenticationServiceMock.Setup(x => x.SignUpAsync(It.IsAny<SignUpAuthenticationView>()))
                .Callback((SignUpAuthenticationView view) => { signUpViewSentToServer = view; })
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
        public void SignUpPage_UserSubmitsValidDataButSignUpAsyncDidNotSendMessage_CallbacksTriggerAndReturnValidData()
        {
            bool switchOnCalled = false;
            bool switchOffCalled = false;
            SignUpAuthenticationView signUpViewSentToServer = null;
            string navigateToUri = null;
            string notificationMessage = null;

            var authenticationServiceMock = new Mock<IAuthenticationService>();
            authenticationServiceMock.Setup(x => x.SignUpAsync(It.IsAny<SignUpAuthenticationView>()))
                .Callback((SignUpAuthenticationView view) => { signUpViewSentToServer = view; })
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
        public void SignUpPage_UserSubmitsValidData_NoErrorMessagesWereShown()
        {
            Services.AddSingleton(_authenticationService);
            Services.AddSingleton(_loaderService);
            Services.AddSingleton(_navigationWrapper);
            Services.AddSingleton(_toastService);

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
        public void SignUpPage_UserSubmitsDataWithEmptyNickname_CorrespondingErrorMessageWasShown()
        {
            Services.AddSingleton(_authenticationService);
            Services.AddSingleton(_loaderService);
            Services.AddSingleton(_navigationWrapper);
            Services.AddSingleton(_toastService);

            var validSignUpView = GetValidSignUpView();
            validSignUpView.Nickname = "";

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
        public void SignUpPage_UserSubmitsDataWithTooLongNickname_CorrespondingErrorMessageWasShown()
        {
            Services.AddSingleton(_authenticationService);
            Services.AddSingleton(_loaderService);
            Services.AddSingleton(_navigationWrapper);
            Services.AddSingleton(_toastService);

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
        public void SignUpPage_UserSubmitsDataWithEmptyEmail_CorrespondingErrorMessageWasShown()
        {
            Services.AddSingleton(_authenticationService);
            Services.AddSingleton(_loaderService);
            Services.AddSingleton(_navigationWrapper);
            Services.AddSingleton(_toastService);

            var validSignUpView = GetValidSignUpView();
            validSignUpView.Email = "";

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
        public void SignUpPage_UserSubmitsDataWithInvalidFormatEmail_CorrespondingErrorMessageWasShown()
        {
            Services.AddSingleton(_authenticationService);
            Services.AddSingleton(_loaderService);
            Services.AddSingleton(_navigationWrapper);
            Services.AddSingleton(_toastService);

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
        public void SignUpPage_PasswwordIsTooShort_CorrespondingErrorMessageWasShown()
        {
            Services.AddSingleton(_authenticationService);
            Services.AddSingleton(_loaderService);
            Services.AddSingleton(_navigationWrapper);
            Services.AddSingleton(_toastService);

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
        public void SignUpPage_PasswwordDoesNotMatchesRegularExpression_CorrespondingErrorMessageWasShown()
        {
            Services.AddSingleton(_authenticationService);
            Services.AddSingleton(_loaderService);
            Services.AddSingleton(_navigationWrapper);
            Services.AddSingleton(_toastService);

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
        public void SignUpPage_PasswwordAndConfirmPasswordDoesNotMatch_CorrespondingErrorMessageWasShown()
        {
            Services.AddSingleton(_authenticationService);
            Services.AddSingleton(_loaderService);
            Services.AddSingleton(_navigationWrapper);
            Services.AddSingleton(_toastService);

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
