using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.ViewModels.Deposit;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BankingApp.UI.Pages.DetailsPage
{
    /// <summary>
    /// Component renders deposit item with its monthly payments.
    /// </summary>
    public partial class DetailsPage
    {
        private GetByIdDepositView _responseDepositView;

        [Inject]
        private ILoaderService _loaderService { get; set; }
        [Inject]
        private IDepositService _depositeService { get; set; }

        /// <summary>
        /// Id of rendered deposit.
        /// </summary>
        [Parameter]
        public int DepositId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _loaderService.SwitchOn();
            _responseDepositView = await _depositeService.GetByIdAsync(DepositId);
            _loaderService.SwitchOff();
        }
    }
}
