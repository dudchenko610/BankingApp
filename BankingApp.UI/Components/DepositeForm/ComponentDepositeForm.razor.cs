using BankingApp.ViewModels.Banking;
using Microsoft.AspNetCore.Components;

namespace BankingApp.UI.Components.DepositeForm
{
    public partial class ComponentDepositeForm
    {
        [Parameter]
        public EventCallback<RequestCalculateDepositeBankingView> OnFormSubmit { get; set; }

        private RequestCalculateDepositeBankingView requestModel = new RequestCalculateDepositeBankingView();

        private void SubmitForm()
        {
            OnFormSubmit.InvokeAsync(requestModel);
        }
    }
}
