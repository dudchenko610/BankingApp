﻿using System.Collections.Generic;

namespace BankingApp.ViewModels.Pagination
{
    public class ResponsePagedDataView<T> where T : class
    {
        public IList<T> Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
    }
}