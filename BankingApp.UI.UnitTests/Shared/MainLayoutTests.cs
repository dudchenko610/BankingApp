using BankingApp.UI.Core.Routes;
using BankingApp.UI.Shared;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BankingApp.UI.UnitTests.Shared
{
    public class MainLayoutTests : TestContext
    {
        private NavigationManager _navigationManager;

        public MainLayoutTests()
        {
            Services.AddSingleton<NavigationManager, FakeNavigationManager>();
            _navigationManager = Services.GetService<NavigationManager>();
        }

        [Fact]
        public void Header_UserClicksTheButon_DepositePageOpens()
        {
            var mainLayoutComp = RenderComponent<MainLayout>();
            _navigationManager.NavigateTo(Routes.DepositePage);

          //  mainLayoutComp.Find()
        }
    }
}
