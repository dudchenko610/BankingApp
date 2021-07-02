using Bunit.Rendering;
using Microsoft.AspNetCore.Components;

namespace BankingApp.UI.UnitTests
{
    public class FakeNavigationManager : NavigationManager
    {
        private readonly ITestRenderer renderer;

        public FakeNavigationManager(ITestRenderer renderer)
        {
            Initialize("http://localhost/", "http://localhost/");
            this.renderer = renderer;
        }

        protected override void NavigateToCore(string uri, bool forceLoad)
        {
            Uri = ToAbsoluteUri(uri).ToString();
            renderer.Dispatcher.InvokeAsync(() => NotifyLocationChanged(isInterceptedLink: false));
        }
    }
}
