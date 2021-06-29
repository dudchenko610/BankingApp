using BankingApp.ViewModels.Banking;
using Microsoft.AspNetCore.Components;

namespace BankingApp.UI.Components.DepositeForm
{
    public partial class DepositeForm
    {
        public DepositeForm()
        { 
            _requestModel = new RequestCalculateDepositeBankingView();
            _requestModel.CalculationFormula = DepositeCalculationFormulaEnumView.SimpleInterest;
        }
        [Parameter]
        public EventCallback<RequestCalculateDepositeBankingView> OnFormSubmit { get; set; }

        private RequestCalculateDepositeBankingView _requestModel;

        private void SubmitForm()
        {
            OnFormSubmit.InvokeAsync(_requestModel);
        }
    }
}
