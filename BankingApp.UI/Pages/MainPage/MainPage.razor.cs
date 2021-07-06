using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking;
using BankingApp.UI.Core.Enums;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BankingApp.UI.Pages.MainPage
{
    public partial class MainPage
    {
        [Inject]
        private IDepositeService _depositeService { get; set; }
        private ResponseCalculateDepositeBankingView _depositeResponse;

        public DepositePageState _pageState;

        public MainPage()
        {
            _pageState = DepositePageState.DisplayFormState;
        }

        protected async Task OnDepositeFormSubmit(RequestCalculateDepositeBankingView reqModel)
        {
            _depositeResponse = null;
            _pageState = DepositePageState.LoadingState;
            _depositeResponse = await _depositeService.CalculateDepositeAsync(reqModel);
            _pageState = DepositePageState.DispalyListState;
        }

        private void BackToForm()
        {
            _pageState = DepositePageState.DisplayFormState;
        }
    }
}
