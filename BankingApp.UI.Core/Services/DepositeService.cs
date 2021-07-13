using BankingApp.Shared;
using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking.Calculate;
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

        public async Task<int> CalculateDepositeAsync(CalculateDepositeBankingView reqDeposite)
        {
            var serializedDepositeRequest = JsonConvert.SerializeObject(reqDeposite);

            var requestContent = new StringContent(serializedDepositeRequest, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Constants.Routes.Banking.BankingRoute}/{Constants.Routes.Banking.CalculateDeposite}", requestContent);

            var serializedResponse = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<int>(serializedResponse);

            return responseObject;
        }

        public async Task<ResponseCalculationHistoryBankingView> GetCalculationDepositeHistoryAsync()
        {
            var response = await _httpClient.GetAsync($"{Constants.Routes.Banking.BankingRoute}/{Constants.Routes.Banking.CalculationHistory}");

            var serializedResponse = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<ResponseCalculationHistoryBankingView>(serializedResponse);

            return responseObject;
        }

        public async Task<ResponseCalculationHistoryDetailsBankingView> GetCalculationHistoryDetailsAsync(int depositeHistoryId)
        {
            var response = await _httpClient
                .GetAsync($"{Constants.Routes.Banking.BankingRoute}/{Constants.Routes.Banking.CalculationHistoryDetails}?depositeHistoryId={depositeHistoryId}");

            var serializedResponse = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<ResponseCalculationHistoryDetailsBankingView>(serializedResponse);

            return responseObject;
        }
    }
}
