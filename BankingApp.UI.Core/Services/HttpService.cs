using BankingApp.UI.Core.Interfaces;
using Blazored.Toast.Services;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BankingApp.ViewModels.ViewModels.Authentication;
using Newtonsoft.Json;

namespace BankingApp.UI.Core.Services
{
    /// <summary>
    /// Used to simplify http requests usage.
    /// </summary>
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly INavigationWrapper _navigationWrapper;
        private readonly ILocalStorageService _localStorageService;
        private readonly IToastService _toastService;

        /// <summary>
        /// Creates instance of <see cref="HttpService"/>.
        /// </summary>
        /// <param name="httpClient">An instance of <see cref="HttpClient"/>.</param>
        /// <param name="navigationWrapper">An instance of <see cref="INavigationWrapper"/>.</param>
        /// <param name="localStorageService">An instance of <see cref="ILocalStorageService"/>.</param>
        /// <param name="toastService">An instance of <see cref="IToastService"/>.</param>
        public HttpService(
            HttpClient httpClient,
            INavigationWrapper navigationWrapper,
            ILocalStorageService localStorageService,
            IToastService toastService
        )
        {
            _httpClient = httpClient;
            _navigationWrapper = navigationWrapper;
            _localStorageService = localStorageService;
            _toastService = toastService;
        }
        
        /// <summary>
        /// Sends GET request to specified address, expects nothing to retrieve from server.
        /// </summary>
        /// <typeparam name="TResult">Data type of view model should be received.</typeparam>
        /// <param name="uri">Url address.</param>
        /// <param name="authorized">Indicates whether access token should be attached to HTTP request.</param>
        /// <returns>Default value, if exception occured, otherwise expected object.</returns>
        public async Task<TResult> GetAsync<TResult>(string uri, bool authorized = true)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            try
            {
                var content = await SendRequestAsync(request, authorized);

                if (content is null)
                {
                    return default;
                }

                return JsonConvert.DeserializeObject<TResult>(content);
            }
            catch
            {
                _toastService.ShowError(Constants.Constants.Notifications.UnexpectedError);

                return default;
            }
        }

        /// <summary>
        /// Sends POST request to specified address, expects object of specified type.
        /// </summary>
        /// <typeparam name="TResult">Data type of view model should be received.</typeparam>
        /// <typeparam name="TModel">Data type of view model should be sent as request body.</typeparam>
        /// <param name="uri">Url address.</param>
        /// <param name="value">Object to be serialized and sent as request body.</param>
        /// <param name="authorized">Indicates whether access token should be attached to HTTP request.</param>
        /// <returns>Default value, if exception occured, otherwise expected object.</returns>
        public async Task<TResult> PostAsync<TResult, TModel>(string uri, TModel value, bool authorized = true)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
            try
            {
                var content = await SendRequestAsync(request, authorized);

                if (content is null)
                {
                    return default;
                }

                return JsonConvert.DeserializeObject<TResult>(content);
            }
            catch
            {
                _toastService.ShowError(Constants.Constants.Notifications.UnexpectedError);

                return default;
            }
        }

        /// <summary>
        /// Sends POST request to specified address.
        /// </summary>
        /// <typeparam name="TModel">Data type of view model should be received.</typeparam>
        /// <param name="uri">Url address.</param>
        /// <param name="value">Object to be serialized and sent as request body.</param>
        /// <param name="authorized">Indicates whether access token should be attached to HTTP request.</param>
        /// <returns>Flag indicating whether server responded with success.</returns>
        public async Task<bool> PostAsync<TModel>(string uri, TModel value, bool authorized = true)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
            try
            {
                var response = await SendRequestAsync(request, authorized);

                if (response is null)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                _toastService.ShowError(Constants.Constants.Notifications.UnexpectedError);

                return default;
            }
        }

        private async Task<string> SendRequestAsync(HttpRequestMessage request, bool authorized = true)
        {
            if (authorized)
            {
                var tokensView = await _localStorageService.GetItemAsync<TokensView>(Constants.Constants.Authentication.TokensView);
                var isApiUrl = !request.RequestUri.IsAbsoluteUri;

                if (tokensView != null && isApiUrl)
                { 
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokensView.AccessToken);
                }
            }

            using HttpResponseMessage response = await _httpClient.SendAsync(request);

            if (authorized && response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _toastService.ShowError(Constants.Constants.Notifications.Unauthorized);
                _navigationWrapper.NavigateTo(Constants.Constants.Routes.LogoutPage);

                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(errorMessage))
                {
                    _toastService.ShowError(Constants.Constants.Notifications.UnexpectedError);
                }
                else 
                {
                    _toastService.ShowError(errorMessage);
                }

                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
