using BankingApp.UI.Core.Attributes;
using BankingApp.UI.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using static BankingApp.UI.Core.Constants.Constants;

namespace BankingApp.UI.Shared.AppRouteView
{
    public class AppRouteView : RouteView
    {
        [Inject]
        private INavigationWrapper _navigationWrapper { get; set; }
        [Inject]
        private IAuthenticationService _authenticationService { get; set; }

        protected override void Render(RenderTreeBuilder builder)
        {
            bool authorize = Attribute.GetCustomAttribute(RouteData.PageType, typeof(AuthorizeAttribute)) != null;
            bool unauthorized = Attribute.GetCustomAttribute(RouteData.PageType, typeof(UnauthorizedAttribute)) != null;

            if (authorize && _authenticationService.TokensView == null)
            {
                _navigationWrapper.NavigateTo(Routes.SignInPage);
                return;
            } 
            if (unauthorized && _authenticationService.TokensView != null)
            {
                _navigationWrapper.NavigateTo(Routes.MainPage);
                return;
            }

            base.Render(builder);
        }
    }
}
