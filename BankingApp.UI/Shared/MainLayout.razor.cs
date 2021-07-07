using BankingApp.UI.Core.Interfaces;
using Microsoft.AspNetCore.Components;

namespace BankingApp.UI.Shared
{
    public partial class MainLayout
    {
        private bool _showLoader;
        [Inject]
        private ILoaderService _loaderService { get; set; }

        public MainLayout()
        { 
            _showLoader = false;
        }
        protected override void OnInitialized()
        {
            _loaderService.OnLoaderSwitch += OnLoaderSwitch;
        }

        private void OnLoaderSwitch(bool show)
        {
            _showLoader = show;
            StateHasChanged();
        }
    }
}
