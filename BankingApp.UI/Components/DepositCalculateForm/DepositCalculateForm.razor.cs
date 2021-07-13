using BankingApp.ViewModels.Banking.Calculate;
using BankingApp.ViewModels.Enums;
using Microsoft.AspNetCore.Components;

namespace BankingApp.UI.Components.DepositCalculateForm
{
    public partial class DepositCalculateForm
    {
        private CalculateDepositView _requestModel;

        [Parameter]
        public EventCallback<CalculateDepositView> OnFormSubmit { get; set; }

        public DepositCalculateForm()
        {
            _requestModel = new CalculateDepositView();
            _requestModel.CalculationFormula = DepositCalculationFormulaEnumView.SimpleInterest;
        }

        private void SubmitForm()
        {
            OnFormSubmit.InvokeAsync(_requestModel);
        }
    }
}
