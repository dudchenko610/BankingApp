﻿using BankingApp.Entities.Entities;
using BankingApp.UI.Core.Interfaces;
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

namespace BankingApp.UI.Core.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly INavigationWrapper _navigationWrapper;
        private readonly ILocalStorageService _localStorageService;
        private readonly IAuthenticationService _authenticationService;

        public HttpService(
            HttpClient httpClient,
            INavigationWrapper navigationWrapper,
            ILocalStorageService localStorageService,
            IAuthenticationService authenticationService
        )
        {
            _httpClient = httpClient;
            _navigationWrapper = navigationWrapper;
            _localStorageService = localStorageService;
            _authenticationService = authenticationService;
        }

        public async Task<T> GetAsync<T>(string uri, bool authorized = true)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            return await SendRequestAsync<T>(request);
        }

        public async Task<T> PostAsync<T>(string uri, object value, bool authorized = true)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            return await SendRequestAsync<T>(request, authorized);
        }

        private async Task<T> SendRequestAsync<T>(HttpRequestMessage request, bool authorized = true)
        {
            if (authorized)
            {
                // add jwt auth header if user is logged in and request is to the api url
                var user = await _localStorageService.GetItem<User>("user");
                var isApiUrl = !request.RequestUri.IsAbsoluteUri;
                if (user != null && isApiUrl)
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", user.RefreshToken);
            }
            
            using var response = await _httpClient.SendAsync(request);

            // auto logout on 401 response
            if (authorized && response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _authenticationService.LogoutAsync();
                return default;
            }

            // throw exception on error response
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                _navigationWrapper.NavigateTo($"{Routes.Routes.NotificationPage}?message={errorMessage}");
                return default;
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
