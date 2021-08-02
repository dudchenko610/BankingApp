using Microsoft.AspNetCore.Components.Routing;
using System;

namespace BankingApp.UI.Core.Interfaces
{
    /// <summary>
    /// Provides interface to navigate the application routes.
    /// </summary>
    public interface INavigationWrapper
    {
        /// <summary>
        /// Base url of current page address.
        /// </summary>
        string BaseUri { get; }

        /// <summary>
        /// Url address.
        /// </summary>
        string Uri { get; }

        /// <summary>
        /// Triggers when application navigates to another route.
        /// </summary>
        event EventHandler<LocationChangedEventArgs> LocationChanged;

        /// <summary>
        /// Vavigates aplication to specified route address.
        /// </summary>
        /// <param name="uri">Url address.</param>
        /// <param name="forceLoad">If true, bypasses client-side routing and forces the browser to load the new page from the server, whether or not the URI would normally be handled by the client-side router.</param>
        void NavigateTo(string uri, bool forceLoad = false);

        /// <summary>
        /// Converts a relative URI into an absolute one (by resolving it relative to the current absolute URI).
        /// </summary>
        /// <param name="relativeUri">The relative URI.</param>
        /// <returns>The absolute URI.</returns>
        Uri ToAbsoluteUri(string relativeUri);

        /// <summary>
        ///  Given a base URI (e.g., one previously returned by Microsoft.AspNetCore.Components.NavigationManager.BaseUri), converts an absolute URI into one relative to the base URI prefix.
        /// </summary>
        /// <param name="uri">An absolute URI that is within the space of the base URI.</param>
        /// <returns>A relative URI path.</returns>
        string ToBaseRelativePath(string uri);
    }
}
