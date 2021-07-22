using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking.Authentication;
using Blazored.LocalStorage;
using System.Threading.Tasks;
using static BankingApp.Shared.Constants;

namespace BankingApp.UI.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly INavigationWrapper _navigationWrapper;
        private readonly ILocalStorageService _localStorageService;
        private readonly IHttpService _httpService;

        public TokensView TokensView { get; private set; }
        
        public AuthenticationService(INavigationWrapper navigationWrapper, 
            ILocalStorageService localStorageService,
            IHttpService httpService)
        {
            _navigationWrapper = navigationWrapper;
            _localStorageService = localStorageService;
            _httpService = httpService;
        }

        public async Task InitializeAsync()
        {
            TokensView = await _localStorageService.GetItemAsync<TokensView>(Constants.Constants.Authentication.TokensView);
        }

        public async Task<bool> SignUpAsync(SignUpAuthenticationView signUpAccountView)
        {
            return await _httpService.PostAsync<object>($"{Routes.Authentication.Route}/{Routes.Authentication.SignUp}", signUpAccountView, false) != null;
        }

        public async Task<bool> ConfirmEmailAsync(ConfirmEmailAuthenticationView confirmEmailAccountView)
        {
            return await _httpService.PostAsync<object>($"{Routes.Authentication.Route}/{Routes.Authentication.ConfirmEmail}", confirmEmailAccountView, false) != null;
        }

        public async Task<bool> SignInAsync(SignInAuthenticationView signInAccountView)
        {
            var tokensView = await _httpService.PostAsync<TokensView>($"{Routes.Authentication.Route}/{Routes.Authentication.SignIn}", signInAccountView, false);
            await _localStorageService.SetItemAsync(Constants.Constants.Authentication.TokensView, tokensView);
            TokensView = tokensView;
            return tokensView != null;
        }

        public async Task LogoutAsync()
        {
            TokensView = null;
            await _localStorageService.RemoveItemAsync(Constants.Constants.Authentication.TokensView);
            _navigationWrapper.NavigateTo(Constants.Constants.Routes.SignInPage);
        }
        
        public async Task<bool> ResetPasswordAsync(ResetPasswordAuthenticationView resetPasswordAuthenticationView)
        {
            return await _httpService.PostAsync<object>($"{Routes.Authentication.Route}/{Routes.Authentication.ResetPassword}", resetPasswordAuthenticationView, false) != null;
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordAuthenticationView forgotPasswordAuthenticationView)
        {
            return await _httpService.PostAsync<object>($"{Routes.Authentication.Route}/{Routes.Authentication.ForgotPassword}", forgotPasswordAuthenticationView, false) != null;
        }
    }
}
