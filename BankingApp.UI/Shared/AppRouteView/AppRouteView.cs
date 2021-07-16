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
        public INavigationWrapper NavigationWrapper { get; set; }

        [Inject]
        public IAuthenticationService AuthenticationService { get; set; }

        protected override void Render(RenderTreeBuilder builder)
        {
            bool authorize = Attribute.GetCustomAttribute(RouteData.PageType, typeof(AuthorizeAttribute)) != null;
            if (authorize && AuthenticationService.User == null)
            {
                NavigationWrapper.NavigateTo(Routes.SignInPage);
            }
            else
            {
                base.Render(builder);
            }
        }
    }
}
