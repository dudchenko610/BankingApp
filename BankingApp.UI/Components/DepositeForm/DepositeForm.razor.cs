using BankingApp.ViewModels.Banking;
using BankingApp.ViewModels.Enums;
using Microsoft.AspNetCore.Components;

namespace BankingApp.UI.Components.DepositeForm
{
    public partial class DepositeForm
    {
        private RequestCalculateDepositeBankingView _requestModel;

        [Parameter]
        public EventCallback<RequestCalculateDepositeBankingView> OnFormSubmit { get; set; }

        public DepositeForm()
        { 
            _requestModel = new RequestCalculateDepositeBankingView();
            _requestModel.CalculationFormula = DepositeCalculationFormulaEnumView.SimpleInterest;
        }
        
        private void SubmitForm()
        {
            OnFormSubmit.InvokeAsync(_requestModel);
        }
    }
}