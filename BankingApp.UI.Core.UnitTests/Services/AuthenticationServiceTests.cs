using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Core.Services;
using BankingApp.ViewModels.Banking.Authentication;
using Blazored.LocalStorage;
using Bunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using static BankingApp.Shared.Constants;

namespace BankingApp.UI.Core.UnitTests.Services
{
    public class AuthenticationServiceTests : TestContext
    {
        private INavigationWrapper _navigationWrapper;
        private ILocalStorageService _localStorageService;
        private IHttpService _httpService;

        public AuthenticationServiceTests()
        {
            var navigationWrapperMock = new Mock<INavigationWrapper>();
            _navigationWrapper = navigationWrapperMock.Object;

            var localStorageMock = new Mock<ILocalStorageService>();
            _localStorageService = localStorageMock.Object;

            var httpServiceMock = new Mock<IHttpService>();
            _httpService = httpServiceMock.Object;
        }

        [Fact]
        public async Task Initialize_CallMethod_CallsGetItemAsyncOfLocalStorageService()
        {
            var validTokensView = GetValidTokensView();
            string accesTokenKey = null;

            var localStorageMock = new Mock<ILocalStorageService>();
            localStorageMock.Setup(x => x.GetItemAsync<TokensView>(It.IsAny<string>(), It.IsAny<CancellationToken?>()))
                .Callback((string key, CancellationToken? cancellationToken) => { accesTokenKey = key; }).ReturnsAsync(validTokensView);

            var authenticationService = new AuthenticationService(_navigationWrapper, localStorageMock.Object, _httpService);
            await authenticationService.InitializeAsync();

            accesTokenKey.Should().BeEquivalentTo(Constants.Constants.Authentication.TokensView);
        } 
        
        [Fact]
        public async Task SignUp_PassValidParameter_CallsPostAsyncOfHttpService()
        {
            var validSignUpView = GetValidSignUpView();
            string passedUrl = null;
            object signUpView = null;

            var httpServiceMock = new Mock<IHttpService>();
            httpServiceMock.Setup(x => x.PostAsync<object>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<bool>()))
                .Callback((string url, object val, bool authorized) =>
                {
                    passedUrl = url;
                    signUpView = val;
                })
                .ReturnsAsync(new object());

            var authenticationService = new AuthenticationService(_navigationWrapper, _localStorageService, httpServiceMock.Object);
            var execResult = await authenticationService.SignUpAsync(validSignUpView);

            passedUrl.Should().BeEquivalentTo($"{Routes.Authentication.Route}/{Routes.Authentication.SignUp}");
            signUpView.Should().NotBeNull().And.BeOfType<SignUpAuthenticationView>().And.BeEquivalentTo(validSignUpView);
            execResult.Should().BeTrue();
        }

        [Fact]
        public async Task ConfirmEmail_PassValidParameter_CallsPostAsyncOfHttpService()
        {
            var validConfirmEmailView = GetValidConfirmEmailView();
            string passedUrl = null;
            object confirmEmailView = null;

            var httpServiceMock = new Mock<IHttpService>();
            httpServiceMock.Setup(x => x.PostAsync<object>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<bool>()))
                .Callback((string url, object val, bool authorized) =>
                {
                    passedUrl = url;
                    confirmEmailView = val;
                })
                .ReturnsAsync(new object());

            var authenticationService = new AuthenticationService(_navigationWrapper, _localStorageService, httpServiceMock.Object);
            var execResult = await authenticationService.ConfirmEmailAsync(validConfirmEmailView);

            passedUrl.Should().BeEquivalentTo($"{Routes.Authentication.Route}/{Routes.Authentication.ConfirmEmail}");
            confirmEmailView.Should().NotBeNull().And.BeOfType<ConfirmEmailAuthenticationView>().And.BeEquivalentTo(validConfirmEmailView);
            execResult.Should().BeTrue();
        }

        [Fact]
        public async Task SignIn_PassValidParameter_CallsPostAsyncOfHttpServiceAndSetItemAsyncOfLocalStorageAsync()
        {
            var validSignInView = GetValidSignInView();
            var validTokenItems = GetValidTokensView();

            string passedUrl = null;
            object signInView = null;

            string accesTokenKey = null;
            object savedTokensView = null;

            var httpServiceMock = new Mock<IHttpService>();
            httpServiceMock.Setup(x => x.PostAsync<TokensView>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<bool>()))
                .Callback((string url, object val, bool authorized) =>
                {
                    passedUrl = url;
                    signInView = val;
                })
                .ReturnsAsync(validTokenItems);

            var localStorageMock = new Mock<ILocalStorageService>();
            localStorageMock.Setup(x => x.SetItemAsync<TokensView>(It.IsAny<string>(), It.IsAny<TokensView>(), It.IsAny<CancellationToken?>()))
                .Callback((string key, TokensView data, CancellationToken? cancellationToken) => 
                    { 
                        accesTokenKey = key;
                        savedTokensView = data;
                    }
                );

            var authenticationService = new AuthenticationService(_navigationWrapper, localStorageMock.Object, httpServiceMock.Object);
            var execResult = await authenticationService.SignInAsync(validSignInView);

            passedUrl.Should().BeEquivalentTo($"{Routes.Authentication.Route}/{Routes.Authentication.SignIn}");
            signInView.Should().NotBeNull().And.BeOfType<SignInAuthenticationView>().And.BeEquivalentTo(validSignInView);

            accesTokenKey.Should().Be(Constants.Constants.Authentication.TokensView);
            savedTokensView.Should().BeOfType<TokensView>().And.BeEquivalentTo(validTokenItems);

            execResult.Should().BeTrue();
        }

        [Fact]
        public async Task Logout_PassValidParameter_CallsNavigateToOfNavigationWrapperAndRemoveItemAsyncOfLocalStorageAsync()
        {
            string accesTokenKey = null;
            object navigatedToUri = null;
            
            var localStorageMock = new Mock<ILocalStorageService>();
            localStorageMock.Setup(x => x.RemoveItemAsync(It.IsAny<string>(), It.IsAny<CancellationToken?>()))
                .Callback((string key, CancellationToken? cancellationToken) => { accesTokenKey = key; });

            var navigationWrapperMock = new Mock<INavigationWrapper>();
            navigationWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), It.IsAny<bool>()))
                .Callback((string uri, bool forceLoad) => { navigatedToUri = uri; });

            var authenticationService = new AuthenticationService(navigationWrapperMock.Object, localStorageMock.Object, _httpService);
             await authenticationService.LogoutAsync();

            accesTokenKey.Should().Be(Constants.Constants.Authentication.TokensView);
            navigatedToUri.Should().BeEquivalentTo(Constants.Constants.Routes.SignInPage);
        }

        [Fact]
        public async Task ResetPassword_PassValidParameter_CallsPostAsyncOfHttpService()
        {
            var validResetPasswordView = GetValidResetPasswordView();
            string passedUrl = null;
            object resetPasswordView = null;

            var httpServiceMock = new Mock<IHttpService>();
            httpServiceMock.Setup(x => x.PostAsync<object>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<bool>()))
                .Callback((string url, object val, bool authorized) =>
                {
                    passedUrl = url;
                    resetPasswordView = val;
                })
                .ReturnsAsync(new object());

            var authenticationService = new AuthenticationService(_navigationWrapper, _localStorageService, httpServiceMock.Object);
            var execResult = await authenticationService.ResetPasswordAsync(validResetPasswordView);

            passedUrl.Should().BeEquivalentTo($"{Routes.Authentication.Route}/{Routes.Authentication.ResetPassword}");
            resetPasswordView.Should().NotBeNull().And.BeOfType<ResetPasswordAuthenticationView>().And.BeEquivalentTo(validResetPasswordView);
            execResult.Should().BeTrue();
        }

        [Fact]
        public async Task ForgotPassword_PassValidParameter_CallsPostAsyncOfHttpService()
        {
            var validForgotPasswordView = GetValidForgotPasswordView();
            string passedUrl = null;
            object forgotPasswordView = null;

            var httpServiceMock = new Mock<IHttpService>();
            httpServiceMock.Setup(x => x.PostAsync<object>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<bool>()))
                .Callback((string url, object val, bool authorized) =>
                {
                    passedUrl = url;
                    forgotPasswordView = val;
                })
                .ReturnsAsync(new object());

            var authenticationService = new AuthenticationService(_navigationWrapper, _localStorageService, httpServiceMock.Object);
            var execResult = await authenticationService.ForgotPasswordAsync(validForgotPasswordView);

            passedUrl.Should().BeEquivalentTo($"{Routes.Authentication.Route}/{Routes.Authentication.ForgotPassword}");
            forgotPasswordView.Should().NotBeNull().And.BeOfType<ForgotPasswordAuthenticationView>().And.BeEquivalentTo(validForgotPasswordView);
            execResult.Should().BeTrue();
        }

        private ForgotPasswordAuthenticationView GetValidForgotPasswordView()
        {
            return new ForgotPasswordAuthenticationView
            {
                Email = "rusland610@gmail.com"
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

        private ConfirmEmailAuthenticationView GetValidConfirmEmailView()
        {
            return new ConfirmEmailAuthenticationView
            {
                Email = "a@a.com",
                Code = "the_code_heregdfsgdfgdfgkdjfgjdfjgdfhjgdfdfgdfgdfdhfjgdfhjgdfjgdfgdjfgdhfjghdfghdgdjrheureutuetuyertyureyutyur" // should be long
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

        private SignInAuthenticationView GetValidSignInView()
        {
            return new SignInAuthenticationView
            {
                Email = "rusland610@gmail.com",
                Password = "qwerty12345AAA"
            };
        }

        private TokensView GetValidTokensView()
        {
            return new TokensView
            {
                AccessToken = "access_token"
            };
        }
    }
}
