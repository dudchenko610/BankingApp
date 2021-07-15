using System;
using System.Collections.Generic;

namespace BankingApp.ViewModels.Banking.Deposit
{
    public class GetAllDepositView
    {
        public IList<DepositGetAllDepositViewItem> DepositItems { get; set; }

        public GetAllDepositView()
        {
            DepositItems = new List<DepositGetAllDepositViewItem>();
        }
    }

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
