using BankingApp.UI.Core.Attributes;
using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Core.Routes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            if (authorize && _authenticationService.User == null)
            {
                _navigationWrapper.NavigateTo(Routes.SignInPage);
            }
            else
            {
                bool unauthorized = Attribute.GetCustomAttribute(RouteData.PageType, typeof(UnauthorizedAttribute)) != null;
                base.Render(builder);
            }
        }
    }
}
