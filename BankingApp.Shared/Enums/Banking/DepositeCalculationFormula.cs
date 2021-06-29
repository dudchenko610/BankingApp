using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BankingApp.Shared.Enums.Banking
{
    public enum DepositeCalculationFormula
    {
        [Display(Name = "Simple Interest")]
        SimpleInterest = 0,

        [Display(Name = "Compound Interest")]
        CompoundInterest = 1
    }
}
