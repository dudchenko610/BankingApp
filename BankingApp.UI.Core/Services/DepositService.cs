using BankingApp.Shared;
using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking.Deposit;
using BankingApp.ViewModels.Pagination;
using System.Threading.Tasks;
using static BankingApp.Shared.Constants;

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
            int depositId = await _httpService.PostAsync<int>($"{Routes.Deposit.Route}/{Routes.Deposit.Calculate}", reqDeposite);
            return depositId;
        }

        public async Task<PagedDataView<DepositGetAllDepositViewItem>> GetAllAsync(int pageNumber, int pageSize)
        {
            var pagedDataView = await _httpService
                .GetAsync<PagedDataView<DepositGetAllDepositViewItem>>($"{Routes.Deposit.Route}/{Routes.Deposit.GetAll}?pageNumber={pageNumber}&pageSize={pageSize}");
            return pagedDataView;
        }

        public async Task<GetByIdDepositView> GetByIdAsync(int depositeHistoryId)
        {
            var depositViewWithMonthyPaymentList = await _httpService
                .GetAsync<GetByIdDepositView>($"{Routes.Deposit.Route}/{Routes.Deposit.GetById}?depositeHistoryId={depositeHistoryId}");
            return depositViewWithMonthyPaymentList;
        }
    }
}
