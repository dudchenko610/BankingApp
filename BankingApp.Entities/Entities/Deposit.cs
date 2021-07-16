using BankingApp.Entities.Enums;
using System;
using System.Collections.Generic;

namespace BankingApp.Entities.Entities
{
    public class Deposit : BaseEntity
    {
        public Deposit()
        {
            MonthlyPayments = new List<MonthlyPayment>();
        }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        public CalculationFormulaEnum CalculationFormula { get; set; }
        public decimal DepositSum { get; set; }
        public int MonthsCount { get; set; }
        public float Percents { get; set; }
        public DateTime CalсulationDateTime { get; set; }
        public virtual IList<MonthlyPayment> MonthlyPayments { get; set; }
    }
}
