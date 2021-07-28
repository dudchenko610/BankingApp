using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApp.DataAccessLayer.Models
{
    public class PagedDataView<T>
    {
        public IList<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
