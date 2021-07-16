using BankingApp.Shared;
using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking.Deposit;
using BankingApp.ViewModels.Pagination;
using System.Threading.Tasks;

namespace BankingApp.UI.Core.Services
{
    public class DepositService : IDepositService
    {
        private readonly IHttpService _httpService;

        public DepositService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<int> CalculateAsync(CalculateDepositView reqDeposite)
        {
            int depositId = await _httpService.PostAsync<int>($"{Constants.Routes.Deposit.Route}/{Constants.Routes.Deposit.Calculate}", reqDeposite);
            return depositId;
        }

        public async Task<PagedDataView<DepositGetAllDepositViewItem>> GetAllAsync(int pageNumber, int pageSize)
        {
            var pagedDataView = await _httpService
                .GetAsync<PagedDataView<DepositGetAllDepositViewItem>>($"{Constants.Routes.Deposit.Route}/{Constants.Routes.Deposit.GetAll}?pageNumber={pageNumber}&pageSize={pageSize}");
            return pagedDataView;
        }

        public async Task<GetByIdDepositView> GetByIdAsync(int depositeHistoryId)
        {
            var depositViewWithMonthyPaymentList = await _httpService
                .GetAsync<GetByIdDepositView>($"{Constants.Routes.Deposit.Route}/{Constants.Routes.Deposit.GetById}?depositeHistoryId={depositeHistoryId}");
            return depositViewWithMonthyPaymentList;
        }
    }
}
