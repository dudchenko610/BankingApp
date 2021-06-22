using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.ViewModels.Banking
{
    public class DepositeOutputData
    {
        public IList<DepositeMonthInfo> PerMonthInfos { get; set; }
    }
}
