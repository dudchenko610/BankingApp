using BankingApp.UI.Core.Enums;
using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BankingApp.UI.Pages.DepositePage
{
    public partial class DepositePage
    {
        public DepositePage()
        {
            _pageState = DepositePageState.DISPLAY_FORM_STATE;
        }
        [Inject]
        private IDepositeService _depositeService { get; set; }

        private DepositePageState _pageState;
        private ResponseCalculateDepositeBankingView depositeResponse;

        protected async Task OnDepositeFormSubmit(RequestCalculateDepositeBankingView reqModel)
        {
            depositeResponse = null;
            _pageState = DepositePageState.LOADING_STATE;
            depositeResponse = await _depositeService.CalculateDepositeAsync(reqModel);
            _pageState = DepositePageState.DISPLAY_LIST_STATE;
        }

        private void BackToForm()
        {
            _pageState = DepositePageState.DISPLAY_FORM_STATE;
        }
    }
}
