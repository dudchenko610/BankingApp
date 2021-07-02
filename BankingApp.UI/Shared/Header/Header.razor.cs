using BankingApp.UI.Core.Routes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;

namespace BankingApp.UI.Shared.Header
{
    public partial class Header
    {
        [Inject]
        private NavigationManager _navigationManager { get; set; }

        protected override void OnInitialized()
        {
            _navigationManager.LocationChanged += (s, e) => StateHasChanged();
        } 

        bool IsActive(string href, NavLinkMatch navLinkMatch = NavLinkMatch.Prefix)
        {
            var relativePath = _navigationManager.ToBaseRelativePath(_navigationManager.Uri).ToLower();
            return navLinkMatch == NavLinkMatch.All ? relativePath == href.ToLower() : relativePath.StartsWith(href.ToLower());
        }

        string GetActive(string href, NavLinkMatch navLinkMatch = NavLinkMatch.Prefix)
        {
            return IsActive(href, navLinkMatch) ? "active" : "";
        }

    }
}
