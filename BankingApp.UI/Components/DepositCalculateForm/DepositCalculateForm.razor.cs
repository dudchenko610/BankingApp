using BankingApp.ViewModels.Enums;
using BankingApp.ViewModels.ViewModels.Deposit;
using Microsoft.AspNetCore.Components;

namespace BankingApp.UI.Components.DepositCalculateForm
{
    /// <summary>
    /// Component renders form to add new deposit. 
    /// </summary>
    public partial class DepositCalculateForm
    {
        private CalculateDepositView _requestModel;

        /// <summary>
        /// Triggers when new deposit form submits.
        /// </summary>
        [Parameter]
        public EventCallback<CalculateDepositView> OnFormSubmit { get; set; }

        /// <summary>
        /// Creates instance of <see cref="DepositCalculateForm"/>.
        /// </summary>
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
