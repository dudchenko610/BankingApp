using BankingApp.Shared;
using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking.Deposit;
using BankingApp.ViewModels.Pagination;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Services
{
    public class DepositService : IDepositService
    {
        private readonly HttpClient _httpClient;

        public DepositService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> CalculateAsync(CalculateDepositView reqDeposite)
        {
            var serializedDepositeRequest = JsonConvert.SerializeObject(reqDeposite);

            var requestContent = new StringContent(serializedDepositeRequest, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Constants.Routes.Banking.DepositRoute}/{Constants.Routes.Banking.Calculate}", requestContent);

            var serializedResponse = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<int>(serializedResponse);

            return responseObject;
        }

        public async Task<PagedDataView<DepositGetAllDepositViewItem>> GetAllAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient
                .GetAsync($"{Constants.Routes.Banking.DepositRoute}/{Constants.Routes.Banking.GetAll}?pageNumber={pageNumber}&pageSize={pageSize}");

            var serializedResponse = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<PagedDataView<DepositGetAllDepositViewItem>>(serializedResponse);

            return responseObject;
        }

        public async Task<GetByIdDepositView> GetByIdAsync(int depositeHistoryId)
        {
            var response = await _httpClient
                .GetAsync($"{Constants.Routes.Banking.DepositRoute}/{Constants.Routes.Banking.GetById}?depositeHistoryId={depositeHistoryId}");

            var serializedResponse = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<GetByIdDepositView>(serializedResponse);

            return responseObject;
        }
    }
}
