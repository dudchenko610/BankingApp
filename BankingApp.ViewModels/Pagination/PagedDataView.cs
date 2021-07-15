using System.Collections.Generic;

namespace BankingApp.ViewModels.Pagination
{
    public class PagedDataView<T> where T : class
    {
        public IList<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
    }
}
