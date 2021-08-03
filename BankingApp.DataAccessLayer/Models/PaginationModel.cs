using System.Collections.Generic;

namespace BankingApp.DataAccessLayer.Models
{
    public class PaginationModel<T>
    {
        public IList<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}