using BankingApp.UI.Core.Helpers;
using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.ViewModels.Authentication;
using Blazored.LocalStorage;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static BankingApp.Shared.Constants;

namespace BankingApp.UI.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly INavigationWrapper _navigationWrapper;
        private readonly ILocalStorageService _localStorageService;
        private readonly IHttpService _httpService;

        private IList<Claim> _claims;

        public TokensView TokensView { get; private set; }
        public bool IsAdmin { get; private set; }

        public AuthenticationService(INavigationWrapper navigationWrapper, 
            ILocalStorageService localStorageService,
            IHttpService httpService)
        {
            _navigationWrapper = navigationWrapper;
            _localStorageService = localStorageService;
            _httpService = httpService;

            _claims = new List<Claim>();
        }

        public async Task InitializeAsync()
        {
            TokensView = await _localStorageService.GetItemAsync<TokensView>(Constants.Constants.Authentication.TokensView);
            ExtractClaimsFromToken();
        }

        public async Task<bool> SignUpAsync(SignUpAuthenticationView signUpAccountView)
        {
            return await _httpService.PostAsync($"{Routes.Authentication.Route}/{Routes.Authentication.SignUp}", signUpAccountView, false);
        }

        public async Task<bool> ConfirmEmailAsync(ConfirmEmailAuthenticationView confirmEmailAccountView)
        {
            return await _httpService.PostAsync($"{Routes.Authentication.Route}/{Routes.Authentication.ConfirmEmail}", confirmEmailAccountView, false);
        }

        public async Task<bool> SignInAsync(SignInAuthenticationView signInAccountView)
        {
            var tokensView = await _httpService.PostAsync<TokensView>($"{Routes.Authentication.Route}/{Routes.Authentication.SignIn}", signInAccountView, false);
            await _localStorageService.SetItemAsync(Constants.Constants.Authentication.TokensView, tokensView);
            TokensView = tokensView;
            ExtractClaimsFromToken();
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
            return await _httpService.PostAsync($"{Routes.Authentication.Route}/{Routes.Authentication.ResetPassword}", resetPasswordAuthenticationView, false);
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordAuthenticationView forgotPasswordAuthenticationView)
        {
            return await _httpService.PostAsync($"{Routes.Authentication.Route}/{Routes.Authentication.ForgotPassword}", forgotPasswordAuthenticationView, false);
        }

        public IList<string> GetRoles()
        {
            return _claims.Where(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Select(x => x.Value).ToList();
        }

        private void ExtractClaimsFromToken()
        {
            if (TokensView == null)
            {
                _claims = new List<Claim>();
            }
            else
            {
                _claims = JwtDecodeHelper.ParseClaimsFromJwt(TokensView.AccessToken).ToList();
            }

            IsAdmin = _claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultRoleClaimType && x.Value == Roles.Admin.ToString()) != null;
        }
    }
}
