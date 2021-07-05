using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking;
using BankingApp.UI.Core.Enums;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BankingApp.UI.Pages.MainPage
{
    public partial class MainPage
    {
        public MainPage()
        {
            _pageState = DepositePageState.DisplayFormState;
        }

        [Inject]
        private IDepositeService _depositeService { get; set; }

        public DepositePageState _pageState;
        private ResponseCalculateDepositeBankingView depositeResponse;

        protected async Task OnDepositeFormSubmit(RequestCalculateDepositeBankingView reqModel)
        {
            depositeResponse = null;
            _pageState = DepositePageState.LoadingState;
            depositeResponse = await _depositeService.CalculateDepositeAsync(reqModel);
            _pageState = DepositePageState.DispalyListState;
        }

        private void BackToForm()
        {
            _pageState = DepositePageState.DisplayFormState;
        }
    }
}
