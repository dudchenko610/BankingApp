using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApp.ViewModels.Pagination
{
    public class ResponsePagedDataView<T> where T : class
    {
        public ResponsePagedDataView()
        {
            Data = new List<T>();
        }
        public IList<T> Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
    }
}
