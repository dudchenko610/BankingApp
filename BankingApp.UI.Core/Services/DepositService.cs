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
        private readonly INavigationWrapper _navigationWrapper;

        public DepositService(HttpClient httpClient, INavigationWrapper navigationWrapper)
        {
            _httpClient = httpClient;
            _navigationWrapper = navigationWrapper;
        }

        public async Task<int> CalculateAsync(CalculateDepositView reqDeposite)
        {
            var serializedDepositeRequest = JsonConvert.SerializeObject(reqDeposite);

            var requestContent = new StringContent(serializedDepositeRequest, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{Constants.Routes.Deposit.Route}/{Constants.Routes.Deposit.Calculate}", requestContent);

            var serializedResponse = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                var errorMessage = JsonConvert.DeserializeObject<string>(serializedResponse);
                _navigationWrapper.NavigateTo($"{Routes.Routes.ErrorPage}?errorMessage={errorMessage}");
            }

            var responseObject = JsonConvert.DeserializeObject<int>(serializedResponse);
            return responseObject;
        }

        public async Task<PagedDataView<DepositGetAllDepositViewItem>> GetAllAsync(int pageNumber, int pageSize)
        {
            var response = await _httpClient
                .GetAsync($"{Constants.Routes.Deposit.Route}/{Constants.Routes.Deposit.GetAll}?pageNumber={pageNumber}&pageSize={pageSize}");

            var serializedResponse = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                var errorMessage = JsonConvert.DeserializeObject<string>(serializedResponse);
                _navigationWrapper.NavigateTo($"{Routes.Routes.ErrorPage}?errorMessage={errorMessage}");
            }

            var responseObject = JsonConvert.DeserializeObject<PagedDataView<DepositGetAllDepositViewItem>>(serializedResponse);
            return responseObject;
        }

        public async Task<GetByIdDepositView> GetByIdAsync(int depositeHistoryId)
        {
            var response = await _httpClient
                .GetAsync($"{Constants.Routes.Deposit.Route}/{Constants.Routes.Deposit.GetById}?depositeHistoryId={depositeHistoryId}");

            var serializedResponse = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                var errorMessage = JsonConvert.DeserializeObject<string>(serializedResponse);
                _navigationWrapper.NavigateTo($"{Routes.Routes.ErrorPage}?errorMessage={errorMessage}");
            }

            var responseObject = JsonConvert.DeserializeObject<GetByIdDepositView>(serializedResponse);
            return responseObject;
        }
    }
}
