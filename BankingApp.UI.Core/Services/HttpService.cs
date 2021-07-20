using BankingApp.Entities.Entities;
using BankingApp.UI.Core.Interfaces;
using Blazored.Toast.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BankingApp.ViewModels.Banking.Account;

namespace BankingApp.UI.Core.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly INavigationWrapper _navigationWrapper;
        private readonly ILocalStorageService _localStorageService;
        private readonly IToastService _toastService;

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

        public async Task<T> GetAsync<T>(string uri, bool authorized = true)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);

            try
            {
                return await SendRequestAsync<T>(request, authorized);
            }
            catch
            {
                _toastService.ShowError(Constants.Constants.Notifications.UnexpectedError);
                return default;
            }
        }

        public async Task<T> PostAsync<T>(string uri, object value, bool authorized = true)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            try
            {
                return await SendRequestAsync<T>(request, authorized);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);

                _toastService.ShowError(Constants.Constants.Notifications.UnexpectedError);
                return default;
            }
        }

        private async Task<T> SendRequestAsync<T>(HttpRequestMessage request, bool authorized = true)
        {
            if (authorized)
            {
                // add jwt auth header if user is logged in and request is to the api url
                var tokensView = await _localStorageService.GetItem<TokensView>(Constants.Constants.Authentication.TokensView);
                var isApiUrl = !request.RequestUri.IsAbsoluteUri;
                if (tokensView != null && isApiUrl)
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokensView.AccessToken);
            }

            using var response = await _httpClient.SendAsync(request);

            // auto logout on 401 response
            if (authorized && response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _navigationWrapper.NavigateTo(Constants.Constants.Routes.LogoutPage);
                return default;
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _toastService.ShowError(errorMessage);
                return default;
            }
            
            try
            {
                return await response.Content.ReadFromJsonAsync<T>();
            }
            catch  // object is not deserializable, but everything is ok!
            {
                return (T)new object();
            }
        }
    }
}
