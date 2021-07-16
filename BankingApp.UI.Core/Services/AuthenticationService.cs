using BankingApp.Entities.Entities;
using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking.Account;
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly INavigationWrapper _navigationWrapper;
        private readonly ILocalStorageService _localStorageService;
        private readonly IHttpService _httpService;
        public User User { get; private set; }
        
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
            User = await _localStorageService.GetItem<User>("user");
        }

        public async Task SignUpAsync(SignUpAuthenticationView signUpAccountView)
        {
            throw new System.NotImplementedException();
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
