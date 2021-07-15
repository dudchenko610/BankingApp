
using Microsoft.AspNetCore.Components.Routing;
using System;

namespace BankingApp.UI.Core.Interfaces
{
    public interface INavigationWrapper
    {
        string BaseUri { get; }
        string Uri { get; }

        event EventHandler<LocationChangedEventArgs> LocationChanged;
        
        void NavigateTo(string uri, bool forceLoad = false);
        Uri ToAbsoluteUri(string relativeUri);
        string ToBaseRelativePath(string uri);
    }
}
