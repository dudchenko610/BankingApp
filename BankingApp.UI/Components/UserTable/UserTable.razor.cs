using BankingApp.UI.Models;
using BankingApp.ViewModels.Banking.Admin;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BankingApp.UI.Components.UserTable
{
    /// <summary>
    /// Component renders tables of user items.
    /// </summary>
    public partial class UserTable
    {
        /// <summary>
        /// Number of current page.
        /// </summary>
        [Parameter]
        public int Page { get; set; } 

        /// <summary>
        /// Maximal number of users on page.
        /// </summary>
        [Parameter]
        public int UsersOnPage { get; set; }

        /// <summary>
        /// List of users on current page.
        /// </summary>
        [Parameter]
        public IList<UserGetAllAdminViewItem> UsersViewList { get; set; }

        /// <summary>
        /// Triggers when user clicks on checkbox to block / unblock user.
        /// </summary>
        [Parameter]
        public EventCallback<BlockUserModel> OnBlockUserClick { get; set; }

        private void OnUserBlockClicked(int userId, bool block)
        {
            var userBlock = new BlockUserModel { UserId = userId, Block = block };
            OnBlockUserClick.InvokeAsync(userBlock);
        }
    }
}
