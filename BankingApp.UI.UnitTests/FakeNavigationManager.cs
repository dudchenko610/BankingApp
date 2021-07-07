using Bunit.Rendering;
using Microsoft.AspNetCore.Components;


namespace BankingApp.UI.UnitTests
{
    public abstract class FakeNavigationManager : NavigationManager
    {
        public FakeNavigationManager()
        {
            Initialize("http://localhost/", "http://localhost/");
        }

        protected sealed override void NavigateToCore(string uri, bool forceLoad)
        {
            NavigateTo();
        }

        public abstract void NavigateTo();
    }
}
