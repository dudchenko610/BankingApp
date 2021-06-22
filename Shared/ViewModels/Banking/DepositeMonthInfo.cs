using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels.Banking
{
    public class DepositeMonthInfo
    {
        public int MonthNumber { get; set; }
        public int Percents { get; set; }
        public decimal TotalMonthSum { get; set; }
    }
}
