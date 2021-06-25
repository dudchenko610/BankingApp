using BankingApp.UI.Core.Enums;
using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BankingApp.UI.Pages.DepositePage
{
    public partial class DepositePage
    {
        [Inject]
        private IDepositeService _depositeService { get; set; }

        private DepositePageState pageState = DepositePageState.DISPLAY_FORM_STATE;
        private ResponseCalculateDepositeBankingView depositeResponse;

        protected async Task OnDepositeFormSubmit(RequestCalculateDepositeBankingView reqModel)
        {
            depositeResponse = null;
            pageState = DepositePageState.LOADING_STATE;
            depositeResponse = await _depositeService.CalculateDepositeAsync(reqModel);
            pageState = DepositePageState.DISPLAY_LIST_STATE;
        }

        private void BackToForm()
        {
            pageState = DepositePageState.DISPLAY_FORM_STATE;
        }
    }
}
