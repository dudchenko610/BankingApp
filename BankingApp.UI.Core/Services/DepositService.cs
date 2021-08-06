using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.ViewModels.Deposit;
using BankingApp.ViewModels.ViewModels.Pagination;
using System.Threading.Tasks;
using static BankingApp.Shared.Constants;

namespace BankingApp.UI.Core.Services
{
    /// <summary>
    /// Allows the user to make deposit calculations and get the calculation history.
    /// </summary>
    public class DepositService : IDepositService
    {
        private readonly IHttpService _httpService;

        /// <summary>
        /// Creates instance of <see cref="DepositService"/>
        /// </summary>
        /// <param name="httpService">Allows send HTTP request to server.</param>
        public DepositService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        /// <summary>
        /// Calculates deposit by passed input data.
        /// </summary>
        /// <param name="calculateDepositView">Contains input deposit data.</param>
        /// <returns>Id of saved deposit.</returns>
        public async Task<int> CalculateAsync(CalculateDepositView calculateDepositView)
        {
            int depositId = await _httpService.PostAsync<int, CalculateDepositView>($"{Routes.Deposit.Route}/{Routes.Deposit.Calculate}", calculateDepositView);
            return depositId;
        }

        /// <summary>
        /// Allows getting page of user's deposit calculations.
        /// </summary>
        /// <param name="pageNumber">Requested page number.</param>
        /// <param name="pageSize">How much elements contains single page.</param>
        /// <returns>View model with data about all deposits in storage and deposits list for specified page.</returns>
        public async Task<PagedDataView<DepositGetAllDepositViewItem>> GetAllAsync(int pageNumber, int pageSize)
        {
            var pagedDataView = await _httpService
                .GetAsync<PagedDataView<DepositGetAllDepositViewItem>>($"{Routes.Deposit.Route}/{Routes.Deposit.GetAll}?pageNumber={pageNumber}&pageSize={pageSize}");

            return pagedDataView;
        }

        /// <summary>
        /// Allows to get information about deposit with monthly payments information.
        /// </summary>
        /// <param name="depositId">Id of deposit in storage.</param>
        /// <returns>View model representing deposit.</returns>
        public async Task<GetByIdDepositView> GetByIdAsync(int depositId)
        {
            var depositViewWithMonthyPaymentList = await _httpService
                .GetAsync<GetByIdDepositView>($"{Routes.Deposit.Route}/{Routes.Deposit.GetById}?depositId={depositId}");
            return depositViewWithMonthyPaymentList;
        }
    }
}
