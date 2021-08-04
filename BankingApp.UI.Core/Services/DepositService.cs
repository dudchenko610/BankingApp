using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.ViewModels.Deposit;
using BankingApp.ViewModels.ViewModels.Pagination;
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

        public async Task<int> CalculateAsync(CalculateDepositView calculateDepositView)
        {
            int depositId = await _httpService.PostAsync<int, CalculateDepositView>($"{Routes.Deposit.Route}/{Routes.Deposit.Calculate}", calculateDepositView);
            return depositId;
        }

        public async Task<PagedDataView<DepositGetAllDepositViewItem>> GetAllAsync(int pageNumber, int pageSize)
        {
            var pagedDataView = await _httpService
                .GetAsync<PagedDataView<DepositGetAllDepositViewItem>>($"{Routes.Deposit.Route}/{Routes.Deposit.GetAll}?pageNumber={pageNumber}&pageSize={pageSize}");
            return pagedDataView;
        }

        public async Task<GetByIdDepositView> GetByIdAsync(int depositId)
        {
            var depositViewWithMonthyPaymentList = await _httpService
                .GetAsync<GetByIdDepositView>($"{Routes.Deposit.Route}/{Routes.Deposit.GetById}?depositId={depositId}");
            return depositViewWithMonthyPaymentList;
        }
    }
}
