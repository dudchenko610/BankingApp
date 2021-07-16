using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApp.ViewModels.Banking.Deposit
{
    public class DepositGetAllDepositViewItem
    {
        public int Id { get; set; }
        public string CalculationFormula { get; set; }
        public decimal DepositSum { get; set; }
        public int MonthsCount { get; set; }
        public float Percents { get; set; }
        public DateTime CalсulationDateTime { get; set; }
    }
}
