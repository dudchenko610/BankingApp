using BankingApp.UI.Core.Attributes;
using BankingApp.UI.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Linq;
using static BankingApp.UI.Core.Constants.Constants;

namespace BankingApp.UI.Shared.AppRouteView
{
    /// <summary>
    /// The component used to control if the specified page should be rendered with given authentication settings.
    /// </summary>
    public class AppRouteView : RouteView
    {
        [Inject]
        private INavigationWrapper _navigationWrapper { get; set; }
        [Inject]
        private IAuthenticationService _authenticationService { get; set; }

        protected override void Render(RenderTreeBuilder builder)
        {
            AuthorizeAttribute authAttribute = (AuthorizeAttribute) Attribute.GetCustomAttribute(RouteData.PageType, typeof(AuthorizeAttribute));
            UnauthorizedAttribute unauthAttribute = (UnauthorizedAttribute)Attribute.GetCustomAttribute(RouteData.PageType, typeof(UnauthorizedAttribute));

            if (authAttribute is not null)
            {
                if (_authenticationService.TokensView is null)
                {
                    _navigationWrapper.NavigateTo(Routes.SignInPage);

                    return;
                }

                var allowedPageRoles = authAttribute.Roles is null ? new string[0] : authAttribute.Roles.Split(','); // if empty - all roles allowed
                var userRoles = _authenticationService.GetRoles();

                if (allowedPageRoles.Any() && !allowedPageRoles.Intersect(userRoles).Any())
                {
                    _navigationWrapper.NavigateTo(Routes.MainPage);

                    return;
                }
            } 

            if (unauthAttribute is not null && _authenticationService.TokensView is not null)
            {
                _navigationWrapper.NavigateTo(Routes.MainPage);

                return;
            }

            base.Render(builder);
        }
    }
}
