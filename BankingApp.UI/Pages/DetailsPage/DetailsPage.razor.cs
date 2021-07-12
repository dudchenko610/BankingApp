using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking.History;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BankingApp.UI.Pages.DetailsPage
{
    public partial class DetailsPage
    {
        private ResponseCalculationHistoryDetailsBankingView _responseCalculationHistoryItem;

        [Inject]
        private ILoaderService _loaderService { get; set; }
        [Inject]
        private IDepositeService _depositeService { get; set; }
        [Parameter]
        public int DepositeHistoryId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _loaderService.SwitchOn();
            _responseCalculationHistoryItem = await _depositeService.GetCalculationHistoryDetailsAsync(DepositeHistoryId);
            _loaderService.SwitchOff();
        }
    }
}
