using BankingApp.UI.Core.Interfaces;
using Microsoft.AspNetCore.Components;

namespace BankingApp.UI.Pages.NotificationPage
{
    public partial class NotificationPage
    {
        [Inject]
        private ILoaderService _loaderService { get; set; }
        [Parameter]
        public string Message { get; set; }

        protected override void OnInitialized()
        {
            _loaderService.SwitchOff();
        }
    }
}
