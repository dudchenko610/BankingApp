using System;
using System.Collections.Generic;

namespace BankingApp.ViewModels.Banking.Deposit
{
    public class GetByIdDepositView
    {
        public int Id { get; set; }
        public string CalculationFormula { get; set; }
        public decimal DepositSum { get; set; }
        public int MonthsCount { get; set; }
        public float Percents { get; set; }
        public DateTime CalсulationDateTime { get; set; }

        public IList<MonthlyPaymentGetByIdDepositViewItem> MonthlyPaymentItems { get; set; }
    }

    public class MonthlyPaymentGetByIdDepositViewItem
    {
        public int MonthNumber { get; set; }
        public decimal TotalMonthSum { get; set; }
        public float Percents { get; set; }
    }
}
