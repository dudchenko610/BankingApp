using System.Collections.Generic;

namespace BankingApp.DataAccessLayer.Models
{
    /// <summary>
    /// View model to contain paged response from storage.
    /// </summary>
    /// <typeparam name="T">Type of paged elements.</typeparam>
    public class PagedDataView<T>
    {
        /// <summary>
        /// Paged items.
        /// </summary>
        public IList<T> Items { get; set; }

        /// <summary>
        /// Total count of elements of that type in storage.
        /// </summary>
        public int TotalCount { get; set; }
    }
}
