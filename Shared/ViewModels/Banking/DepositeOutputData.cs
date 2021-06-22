using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels.Banking
{
    public class DepositeOutputData
    {
        public DepositeOutputData()
        {
            PerMonthInfos = new List<DepositeMonthInfo>();
        }
        public IList<DepositeMonthInfo> PerMonthInfos { get; set; }
    }
}
