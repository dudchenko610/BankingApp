using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Core.Services;
using Blazored.LocalStorage;
using Bunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using static BankingApp.Shared.Constants;
using BankingApp.ViewModels.ViewModels.Authentication;

namespace BankingApp.UI.Core.UnitTests.Services
{
    public class AuthenticationServiceTests : TestContext
    {
        private AuthenticationService _authenticationService;
        private Mock<INavigationWrapper> _navigationWrapperMock;
        private Mock<ILocalStorageService> _localStorageServiceMock;
        private Mock<IHttpService> _httpServiceMock;

        public AuthenticationServiceTests()
        {
            _navigationWrapperMock = new Mock<INavigationWrapper>();
            _localStorageServiceMock = new Mock<ILocalStorageService>();
            _httpServiceMock = new Mock<IHttpService>();

            _authenticationService = new AuthenticationService(_navigationWrapperMock.Object, _localStorageServiceMock.Object, _httpServiceMock.Object);
        }

        [Fact]
        public async Task Initialize_ValidTokens_GetItemAsyncInvoked()
        {
            var validTokensView = GetValidTokensViewWithAdminRole();
            string accesTokenKey = null;

            _localStorageServiceMock.Setup(x => x.GetItemAsync<TokensView>(It.IsAny<string>(), It.IsAny<CancellationToken?>()))
                .Callback((string key, CancellationToken? cancellationToken) => { accesTokenKey = key; }).ReturnsAsync(validTokensView);

            await _authenticationService.InitializeAsync();

            accesTokenKey.Should().BeEquivalentTo(Constants.Constants.Authentication.TokensView);
        } 
        
        [Fact]
        public async Task SignUp_ValidParameterSignUpView_PostAsyncInvoked()
        {
            var validSignUpView = GetValidSignUpView();
            string passedUrl = null;
            SignUpAuthenticationView signUpView = null;

            _httpServiceMock.Setup(x => x.PostAsync<SignUpAuthenticationView>(It.IsAny<string>(), It.IsAny<SignUpAuthenticationView>(), It.IsAny<bool>()))
                .Callback((string url, SignUpAuthenticationView val, bool authorized) =>
                {
                    passedUrl = url;
                    signUpView = val;
                })
                .ReturnsAsync(true);

            var execResult = await _authenticationService.SignUpAsync(validSignUpView);

            passedUrl.Should().BeEquivalentTo($"{Routes.Authentication.Route}/{Routes.Authentication.SignUp}");
            signUpView.Should().NotBeNull().And.BeOfType<SignUpAuthenticationView>().And.BeEquivalentTo(validSignUpView);
            execResult.Should().BeTrue();
        }

        [Fact]
        public async Task ConfirmEmail_ValidConfirmEmailView_PostAsyncInvoked()
        {
            var validConfirmEmailView = GetValidConfirmEmailView();
            string passedUrl = null;
            ConfirmEmailAuthenticationView confirmEmailView = null;

            _httpServiceMock.Setup(x => x.PostAsync<ConfirmEmailAuthenticationView>(It.IsAny<string>(), It.IsAny<ConfirmEmailAuthenticationView>(), It.IsAny<bool>()))
                .Callback((string url, ConfirmEmailAuthenticationView val, bool authorized) =>
                {
                    passedUrl = url;
                    confirmEmailView = val;
                })
                .ReturnsAsync(true);

            var execResult = await _authenticationService.ConfirmEmailAsync(validConfirmEmailView);

            passedUrl.Should().BeEquivalentTo($"{Routes.Authentication.Route}/{Routes.Authentication.ConfirmEmail}");
            confirmEmailView.Should().NotBeNull().And.BeOfType<ConfirmEmailAuthenticationView>().And.BeEquivalentTo(validConfirmEmailView);
            execResult.Should().BeTrue();
        }

        [Fact]
        public async Task SignIn_ValidSignInView_ExpectedResults()
        {
            var validSignInView = GetValidSignInView();
            var validTokenItems = GetValidTokensViewWithAdminRole();

            string passedUrl = null;
            SignInAuthenticationView signInView = null;

            string accesTokenKey = null;
            object savedTokensView = null;

            _httpServiceMock.Setup(x => x.PostAsync<TokensView, SignInAuthenticationView>(It.IsAny<string>(), It.IsAny<SignInAuthenticationView>(), It.IsAny<bool>()))
                .Callback((string url, SignInAuthenticationView val, bool authorized) =>
                {
                    passedUrl = url;
                    signInView = val;
                })
                .ReturnsAsync(validTokenItems);

            _localStorageServiceMock.Setup(x => x.SetItemAsync<TokensView>(It.IsAny<string>(), It.IsAny<TokensView>(), It.IsAny<CancellationToken?>()))
                .Callback((string key, TokensView data, CancellationToken? cancellationToken) => 
                    { 
                        accesTokenKey = key;
                        savedTokensView = data;
                    }
                );

            var execResult = await _authenticationService.SignInAsync(validSignInView);

            passedUrl.Should().BeEquivalentTo($"{Routes.Authentication.Route}/{Routes.Authentication.SignIn}");
            signInView.Should().NotBeNull().And.BeOfType<SignInAuthenticationView>().And.BeEquivalentTo(validSignInView);

            accesTokenKey.Should().Be(Constants.Constants.Authentication.TokensView);
            savedTokensView.Should().BeOfType<TokensView>().And.BeEquivalentTo(validTokenItems);

            execResult.Should().BeTrue();
        }

        [Fact]
        public async Task Logout_ValidParameters_ExpectedResults()
        {
            string accesTokenKey = null;
            object navigatedToUri = null;

            _localStorageServiceMock.Setup(x => x.RemoveItemAsync(It.IsAny<string>(), It.IsAny<CancellationToken?>()))
                .Callback((string key, CancellationToken? cancellationToken) => { accesTokenKey = key; });

            _navigationWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), It.IsAny<bool>()))
                .Callback((string uri, bool forceLoad) => { navigatedToUri = uri; });

             await _authenticationService.LogoutAsync();

            accesTokenKey.Should().Be(Constants.Constants.Authentication.TokensView);
            navigatedToUri.Should().BeEquivalentTo(Constants.Constants.Routes.SignInPage);
        }

        [Fact]
        public async Task ResetPassword_ValidResetPasswordView_PostAsyncInvoked()
        {
            var validResetPasswordView = GetValidResetPasswordView();
            string passedUrl = null;
            ResetPasswordAuthenticationView resetPasswordView = null;

            _httpServiceMock.Setup(x => x.PostAsync<ResetPasswordAuthenticationView>(It.IsAny<string>(), It.IsAny<ResetPasswordAuthenticationView>(), It.IsAny<bool>()))
                .Callback((string url, ResetPasswordAuthenticationView val, bool authorized) =>
                {
                    passedUrl = url;
                    resetPasswordView = val;
                })
                .ReturnsAsync(true);

            var execResult = await _authenticationService.ResetPasswordAsync(validResetPasswordView);

            passedUrl.Should().BeEquivalentTo($"{Routes.Authentication.Route}/{Routes.Authentication.ResetPassword}");
            resetPasswordView.Should().NotBeNull().And.BeOfType<ResetPasswordAuthenticationView>().And.BeEquivalentTo(validResetPasswordView);
            execResult.Should().BeTrue();
        }

        [Fact]
        public async Task ForgotPassword_ValidForgotPasswordView_PostAsyncInvoked()
        {
            var validForgotPasswordView = GetValidForgotPasswordView();
            string passedUrl = null;
            ForgotPasswordAuthenticationView forgotPasswordView = null;

            _httpServiceMock.Setup(x => x.PostAsync<ForgotPasswordAuthenticationView>(It.IsAny<string>(), It.IsAny<ForgotPasswordAuthenticationView>(), It.IsAny<bool>()))
                .Callback((string url, ForgotPasswordAuthenticationView val, bool authorized) =>
                {
                    passedUrl = url;
                    forgotPasswordView = val;
                })
                .ReturnsAsync(true);

            var execResult = await _authenticationService.ForgotPasswordAsync(validForgotPasswordView);

            passedUrl.Should().BeEquivalentTo($"{Routes.Authentication.Route}/{Routes.Authentication.ForgotPassword}");
            forgotPasswordView.Should().NotBeNull().And.BeOfType<ForgotPasswordAuthenticationView>().And.BeEquivalentTo(validForgotPasswordView);
            execResult.Should().BeTrue();
        }

        [Fact]
        public async Task GetRoles_AdminUserWasRegistered_ExpectedResults()
        {
            var validTokensView = GetValidTokensViewWithAdminRole();

            _localStorageServiceMock.Setup(x => x.GetItemAsync<TokensView>(It.IsAny<string>(), It.IsAny<CancellationToken?>()))
                .ReturnsAsync(validTokensView);

            await _authenticationService.InitializeAsync();
            var roleNames = _authenticationService.GetRoles();
            roleNames.Should().Contain(BankingApp.Shared.Constants.Roles.Admin);
        }

        [Fact]
        public async Task GetRoles_ValidTokens_ExpectedResults()
        {
            var validTokensView = GetValidTokensViewWithClientRole();

            _localStorageServiceMock.Setup(x => x.GetItemAsync<TokensView>(It.IsAny<string>(), It.IsAny<CancellationToken?>()))
                .ReturnsAsync(validTokensView);

            await _authenticationService.InitializeAsync();
            var roleNames = _authenticationService.GetRoles();
            roleNames.Should().Contain(Roles.Client);
        }

        [Fact]
        public async Task IsAdmin_ClientUserWasRegisteredAsClient_ExpectedResults()
        {
            var validTokensView = GetValidTokensViewWithClientRole();

            _localStorageServiceMock.Setup(x => x.GetItemAsync<TokensView>(It.IsAny<string>(), It.IsAny<CancellationToken?>()))
                .ReturnsAsync(validTokensView);

            await _authenticationService.InitializeAsync();
            _authenticationService.IsAdmin.Should().BeFalse();
        }

        [Fact]
        public async Task IsAdmin_ClientUserWasRegisteredAsAdmin_ExpectedResults()
        {
            var validTokensView = GetValidTokensViewWithAdminRole();

            _localStorageServiceMock.Setup(x => x.GetItemAsync<TokensView>(It.IsAny<string>(), It.IsAny<CancellationToken?>()))
                .ReturnsAsync(validTokensView);

            await _authenticationService.InitializeAsync();
            _authenticationService.IsAdmin.Should().BeTrue();
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

        private TokensView GetValidTokensViewWithAdminRole()
        {
            return new TokensView
            {
                AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InIuZHVkY2hlbmtvQHN0dWRlbnQua2hhaS5lZHUiLCJzdWIiOiI4IiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IkNyYXp5QWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsIm5iZiI6MTYyNzQ2NjAxMiwiZXhwIjoxNjI3NDczMjEyLCJpc3MiOiJNeUF1dGhTZXJ2ZXIiLCJhdWQiOiJNeUF1dGhDbGllbnQifQ.dwhWpXpQForywt_3_mbbeUw6KGyW-iP4CYvyE1H2cMk"
            };
        }

        private TokensView GetValidTokensViewWithClientRole()
        {
            return new TokensView
            {
                AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InJ1c2xhbmQ2MTBAZ21haWwuY29tIiwic3ViIjoiOSIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJCb2JpayIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkNsaWVudCIsIm5iZiI6MTYyNzQ4MDI1NSwiZXhwIjoxNjI3NDg3NDU1LCJpc3MiOiJNeUF1dGhTZXJ2ZXIiLCJhdWQiOiJNeUF1dGhDbGllbnQifQ.1srjTt8bRDqe7Nn4C0GVaFO7MDpxWFOls-vEiZfzat8"
            };
        }
    }
}
