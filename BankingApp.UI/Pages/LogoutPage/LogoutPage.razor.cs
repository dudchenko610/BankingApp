using BankingApp.UI.Core.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BankingApp.UI.Pages.LogoutPage
{
    public partial class LogoutPage
    {
        [Inject]
        private AuthenticationService _authenticationService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await _authenticationService.LogoutAsync();
        }
    }
}
