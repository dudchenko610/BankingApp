using BankingApp.Entities.Entities;
using BankingApp.Shared;
using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking.Account;
using BankingApp.ViewModels.Banking.Authentication;
using Blazored.Toast.Services;
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly INavigationWrapper _navigationWrapper;
        private readonly ILocalStorageService _localStorageService;
        private readonly IHttpService _httpService;
        private readonly IToastService _toastService;

        public User User { get; private set; }
        
        public AuthenticationService(INavigationWrapper navigationWrapper, 
            ILocalStorageService localStorageService,
            IHttpService httpService,
            IToastService toastService)
        {
            _navigationWrapper = navigationWrapper;
            _localStorageService = localStorageService;
            _httpService = httpService;
            _toastService = toastService;
        }

        public async Task InitializeAsync()
        {
            User = await _localStorageService.GetItem<User>("user");
        }

        public async Task SignUpAsync(SignUpAuthenticationView signUpAccountView)
        {
            await _httpService.PostAsync<object>($"{Constants.Routes.Authentication.Route}/{Constants.Routes.Authentication.SignUp}", signUpAccountView, false);
            _toastService.ShowSuccess(Notifications.Notifications.ConfirmYourEmail);
        }

        public async Task<TokensView> ConfirmEmailAsync(ConfirmEmailAuthenticationView confirmEmailAccountView)
        {
            throw new System.NotImplementedException();
        }

        public async Task<TokensView> SignInAsync(SignInAuthenticationView signInAccountView)
        {
            throw new System.NotImplementedException();
        }

        public async Task LogoutAsync()
        {
            User = null;
            await _localStorageService.RemoveItem("user");
            _navigationWrapper.NavigateTo(Routes.Routes.SignInPage);
        }
    }
}
