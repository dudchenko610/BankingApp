using BankingApp.ViewModels.ViewModels.Deposit;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BankingApp.UI.Components.MonthlyPaymentList
{
    /// <summary>
    /// Component rendered list of deposit monthly payments items.
    /// </summary>
    public partial class MonthlyPaymentList
    {
        /// <summary>
        /// List of deposit monthly payments.
        /// </summary>
        [Parameter]
        public IList<MonthlyPaymentGetByIdDepositViewItem> MonthlyPaymentViewList { get; set; }

        protected override void OnParametersSet()
        {
            MonthlyPaymentViewList = MonthlyPaymentViewList.OrderBy(x => x.MonthNumber).ToList();
        }
    }
}
