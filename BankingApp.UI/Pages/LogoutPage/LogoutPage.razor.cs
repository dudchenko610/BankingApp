using BankingApp.UI.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BankingApp.UI.Pages.LogoutPage
{
    /// <summary>
    /// Component used to perform logout action that will redirect application to another route.
    /// </summary>
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
