using BankingApp.Shared;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.ViewModels.Pagination
{
    public class RequestPaginationFilterView
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = Constants.Errors.Page.IncorrectPageNumberFormat)]
        public int PageNumber { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = Constants.Errors.Page.IncorrectPageSizeFormat)]
        public int PageSize { get; set; }

        public RequestPaginationFilterView()
        {
            PageNumber = Constants.Page.PageNumber;
            PageSize = Constants.Page.PageSizeElements;
        }

        public RequestPaginationFilterView(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
