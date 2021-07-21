using BankingApp.UI.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BankingApp.UI.Pages.LogoutPage
{
    public partial class LogoutPage
    {
        [Inject]
        private IAuthenticationService _authenticationService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await _authenticationService.LogoutAsync();
        }
    }
}
