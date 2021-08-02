using BankingApp.UI.Core.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System;

namespace BankingApp.UI.Core.Wrappers
{
    /// <summary>
    /// Provides interface to navigate the application routes.
    /// </summary>
    public class NavigationWrapper : INavigationWrapper
    {
        private NavigationManager _navigationManager;

        /// <summary>
        /// Base url of current page address.
        /// </summary>
        public string BaseUri { get => _navigationManager.BaseUri; }

        /// <summary>
        /// Url address.
        /// </summary>
        public string Uri { get => _navigationManager.Uri; }

        /// <summary>
        /// Triggers when application navigates to another route.
        /// </summary>
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

        /// <summary>
        /// Creates instance of <see cref="NavigationWrapper"/>
        /// </summary>
        /// <param name="navigationManager">Provides an abstraction for querying and managing URI navigation.</param>
        public NavigationWrapper(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        /// <summary>
        /// Vavigates aplication to specified route address.
        /// </summary>
        /// <param name="uri">Url address.</param>
        /// <param name="forceLoad">If true, bypasses client-side routing and forces the browser to load the new page from the server, whether or not the URI would normally be handled by the client-side router.</param>
        public void NavigateTo(string uri, bool forceLoad = false)
        {
            _navigationManager.NavigateTo(uri, forceLoad);
        }

        /// <summary>
        /// Converts a relative URI into an absolute one (by resolving it relative to the current absolute URI).
        /// </summary>
        /// <param name="relativeUri">The relative URI.</param>
        /// <returns>The absolute URI.</returns>
        public Uri ToAbsoluteUri(string relativeUri)
        {
            return _navigationManager.ToAbsoluteUri(relativeUri);
        }

        /// <summary>
        ///  Given a base URI (e.g., one previously returned by Microsoft.AspNetCore.Components.NavigationManager.BaseUri), converts an absolute URI into one relative to the base URI prefix.
        /// </summary>
        /// <param name="uri">An absolute URI that is within the space of the base URI.</param>
        /// <returns>A relative URI path.</returns>
        public string ToBaseRelativePath(string uri)
        {
            return _navigationManager.ToBaseRelativePath(uri);
        }
    }
}
