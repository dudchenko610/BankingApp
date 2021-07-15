
using Microsoft.AspNetCore.Components;

namespace BankingApp.UI.Pages.ErrorPage
{
    public partial class ErrorPage
    {
        [Parameter]
        public string ErrorMessage { get; set; }
    }
}
