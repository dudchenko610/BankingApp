﻿using BankingApp.Shared;
using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking;
using BankingApp.ViewModels.Banking.History;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Services
{
    public class DepositeService : IDepositeService
    {
        private readonly HttpClient _httpClient;

        public DepositeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseCalculateDepositeBankingView> CalculateDepositeAsync(RequestCalculateDepositeBankingView reqDeposite)
        {
            var serializedDepositeRequest = JsonConvert.SerializeObject(reqDeposite);

            var requestContent = new StringContent(serializedDepositeRequest, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Constants.Routes.Banking.BankingRoute}/{Constants.Routes.Banking.CalculateDeposite}", requestContent);

            var serializedResponse = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<ResponseCalculateDepositeBankingView>(serializedResponse);

            return responseObject;
        }

        public async Task<ResponseCalculationHistoryBankingView> GetCalculationDepositeHistoryAsync()
        {
            var response = await _httpClient.GetAsync($"{Constants.Routes.Banking.BankingRoute}/{Constants.Routes.Banking.CalculationHistory}");

            var serializedResponse = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<ResponseCalculationHistoryBankingView>(serializedResponse);

            return responseObject;
        }

        public async Task<ResponseCalculationHistoryBankingViewItem> GetCalculationHistoryDetailsAsync()
        {
            var response = await _httpClient.GetAsync($"{Constants.Routes.Banking.BankingRoute}/{Constants.Routes.Banking.CalculationHistoryDetails}");

            var serializedResponse = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<ResponseCalculationHistoryBankingViewItem>(serializedResponse);

            return responseObject;
        }
    }
}
