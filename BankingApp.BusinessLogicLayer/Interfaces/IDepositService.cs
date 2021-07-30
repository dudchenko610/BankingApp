using BankingApp.ViewModels.ViewModels.Deposit;
using BankingApp.ViewModels.ViewModels.Pagination;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Allows the user to make deposit calculations and get the calculation history.
    /// </summary>
    public interface IDepositService
    {
        /// <summary>
        /// Calculates deposit by passed input data.
        /// </summary>
        /// <param name="calculateDepositView">Contains input deposit data.</param>
        /// <returns>Id of saved deposit.</returns>
        Task<int> CalculateAsync(CalculateDepositView calculateDepositView);

        /// <summary>
        /// Allows getting page of user's deposit calculations.
        /// </summary>
        /// <param name="pageNumber">Requested page number.</param>
        /// <param name="pageSize">How much elements contains single page.</param>
        /// <returns>View model with data about all deposits in storage and deposits list for specified page.</returns>
        Task<PagedDataView<DepositGetAllDepositViewItem>> GetAllAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Allows to get information about deposit with monthly payments information.
        /// </summary>
        /// <param name="depositId">Id of deposit in storage.</param>
        /// <returns>View model representing deposit.</returns>
        Task<GetByIdDepositView> GetByIdAsync(int depositId);
    }
}
