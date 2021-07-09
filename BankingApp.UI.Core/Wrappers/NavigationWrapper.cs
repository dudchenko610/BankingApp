using BankingApp.UI.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;

namespace BankingApp.UI.Core.Wrappers
{
    public class NavigationWrapper : INavigationWrapper
    {
        private NavigationManager _navigationManager;
        public string BaseUri { get => _navigationManager.BaseUri; }
        public string Uri { get => _navigationManager.Uri; }

        public event EventHandler<LocationChangedEventArgs> LocationChanged
        {
            add
            {
                _navigationManager.LocationChanged += value;
            }

            remove
            {
                _navigationManager.LocationChanged -= value;
            }
        }

        public NavigationWrapper(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public void NavigateTo(string uri, bool forceLoad = false)
        {
            _navigationManager.NavigateTo(uri, forceLoad);
        }

        public Uri ToAbsoluteUri(string relativeUri)
        {
            return _navigationManager.ToAbsoluteUri(relativeUri);
        }

        public string ToBaseRelativePath(string uri)
        {
            return _navigationManager.ToBaseRelativePath(uri);
        }
    }
}
