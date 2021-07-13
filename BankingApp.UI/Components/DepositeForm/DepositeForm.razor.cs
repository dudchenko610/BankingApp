using BankingApp.ViewModels.Banking.Calculate;
using BankingApp.ViewModels.Enums;
using Microsoft.AspNetCore.Components;

namespace BankingApp.UI.Components.DepositeForm
{
    public partial class DepositeForm
    {
        private CalculateDepositeBankingView _requestModel;

        [Parameter]
        public EventCallback<CalculateDepositeBankingView> OnFormSubmit { get; set; }

        public DepositeForm()
        { 
            _requestModel = new CalculateDepositeBankingView();
            _requestModel.CalculationFormula = DepositeCalculationFormulaEnumView.SimpleInterest;
        }
        
        private void SubmitForm()
        {
            OnFormSubmit.InvokeAsync(_requestModel);
        }
    }
}