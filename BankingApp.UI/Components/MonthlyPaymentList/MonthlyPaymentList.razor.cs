using BankingApp.ViewModels.ViewModels.Deposit;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.UI.Components.MonthlyPaymentList
{
    public partial class MonthlyPaymentList
    {
        [Parameter]
        public IList<MonthlyPaymentGetByIdDepositViewItem> MonthlyPaymentViewList { get; set; }

        protected override void OnParametersSet()
        {
            MonthlyPaymentViewList = MonthlyPaymentViewList.OrderBy(x => x.MonthNumber).ToList();
        }
    }
}
