﻿using BankingApp.UI.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace BankingApp.UI.Shared.Header
{
    public partial class Header
    {
        private string _navLinksDisabledClass;
        [Inject]
        private INavigationWrapper _navigationWrapper { get; set; }
        [Inject]
        private ILoaderService _loaderService { get; set; }

        public Header()
        {
            _navLinksDisabledClass = "";
        }

        protected override void OnInitialized()
        {
            _navigationWrapper.LocationChanged += (s, e) => StateHasChanged();
            _loaderService.OnLoaderSwitch += (show) => {
                _navLinksDisabledClass = show ? "disabled" : "";
                StateHasChanged();
            };
        }

        private bool IsActive(string href, NavLinkMatch navLinkMatch = NavLinkMatch.Prefix)
        {
            var relativePath = _navigationWrapper.ToBaseRelativePath(_navigationWrapper.Uri).ToLower();

            if (relativePath.Contains("/"))
                relativePath = relativePath.Split("/")[0];

            return navLinkMatch == NavLinkMatch.All ? relativePath == href.ToLower() : relativePath.StartsWith(href.ToLower());
        }

        private string GetActive(string href, NavLinkMatch navLinkMatch = NavLinkMatch.Prefix)
        {
            return IsActive(href, navLinkMatch) ? "active" : "";
        }
    }
}
