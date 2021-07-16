using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking.Deposit;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BankingApp.UI.Pages.DetailsPage
{
    public partial class DetailsPage
    {
        private GetByIdDepositView _responseDepositView;

        [Inject]
        private ILoaderService _loaderService { get; set; }
        [Inject]
        private IDepositService _depositeService { get; set; }
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
