using BankingApp.ViewModels.Banking;
using Microsoft.AspNetCore.Components;

namespace BankingApp.UI.Components.DepositeForm
{
    public partial class DepositeForm
    {
        public DepositeForm()
        { 
            _requestModel = new RequestCalculateDepositeBankingView();
            _selectFormulaType = 0;
        }
        [Parameter]
        public EventCallback<(RequestCalculateDepositeBankingView, int)> OnFormSubmit { get; set; }

        private RequestCalculateDepositeBankingView _requestModel;
        private int _selectFormulaType;

        private void SubmitForm()
        {
            OnFormSubmit.InvokeAsync((_requestModel, _selectFormulaType));
        }
    }
}
