using BankingApp.ViewModels.Banking.Admin;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.UI.Components.UserTable
{
    public partial class UserTable
    {
        [Parameter]
        public int Page { get; set; } 
        [Parameter]
        public int UsersOnPage { get; set; }
        [Parameter]
        public IList<UserGetAllAdminViewItem> UsersViewList { get; set; }

        [Parameter]
        public EventCallback<(int, bool)> OnBlockUserClick { get; set; }

        private void OnUserBlockClicked(int userId, bool block)
        {
            OnBlockUserClick.InvokeAsync((userId, block));
        }
    }
}
