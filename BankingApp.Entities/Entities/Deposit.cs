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

        public CalculationFormulaEnum CalculationFormula { get; set; }
        public decimal DepositeSum { get; set; }
        public int MonthsCount { get; set; }
        public float Percents { get; set; }
        public DateTime CalulationDateTime { get; set; }
        public virtual IList<MonthlyPayment> MonthlyPayments { get; set; }
    }
}
