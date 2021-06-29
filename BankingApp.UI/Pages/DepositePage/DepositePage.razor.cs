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
            _pageState = DepositePageStateEnumView.DisplayFormState;
        }
        [Inject]
        private IDepositeService _depositeService { get; set; }

        private DepositePageStateEnumView _pageState;
        private ResponseCalculateDepositeBankingView depositeResponse;

        protected async Task OnDepositeFormSubmit(RequestCalculateDepositeBankingView reqModel)
        {
            depositeResponse = null;
            _pageState = DepositePageStateEnumView.LoadingState;
            depositeResponse = await _depositeService.CalculateDepositeAsync(reqModel);
            _pageState = DepositePageStateEnumView.DispalyListState;
        }

        private void BackToForm()
        {
            _pageState = DepositePageStateEnumView.DisplayFormState;
        }
    }
}
