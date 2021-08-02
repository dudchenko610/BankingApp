using System.Collections.Generic;

namespace BankingApp.ViewModels.ViewModels.Pagination
{
    /// <summary>
    /// View model used to respond client with paged items.
    /// </summary>
    /// <typeparam name="T">Page item.</typeparam>
    public class PagedDataView<T> where T : class
    {
        /// <summary>
        /// Creates instance of PagedDataView.
        /// </summary>
        public PagedDataView()
        {
            Items = new List<T>();
        }

        /// <summary>
        /// List of page items.
        /// </summary>
        public IList<T> Items { get; set; }

        /// <summary>
        /// Number of the page.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Count of items of the page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Count of elements of that type in storage.
        /// </summary>
        public int TotalItems { get; set; }
    }
}
