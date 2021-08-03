using BankingApp.UI.Models;
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
        public EventCallback<BlockUserModel> OnBlockUserClick { get; set; }

        private void OnUserBlockClicked(int userId, bool block)
        {
            var userBlock = new BlockUserModel { UserId = userId, Block = block };
            OnBlockUserClick.InvokeAsync(userBlock);
        }
    }
}
