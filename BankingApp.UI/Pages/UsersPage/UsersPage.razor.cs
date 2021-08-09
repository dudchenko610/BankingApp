using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Models;
using BankingApp.ViewModels.Banking.Admin;
using BankingApp.ViewModels.ViewModels.Pagination;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using static BankingApp.UI.Core.Constants.Constants;

namespace BankingApp.UI.Pages.UsersPage
{
    /// <summary>
    /// Component renders list of users.
    /// </summary>
    public partial class UsersPage
    {
        private static readonly int UsersOnPage = 2;

        private int _totalPageCount;
        private PagedDataView<UserGetAllAdminViewItem> _pagedUsers;

        [Inject]
        private ILoaderService _loaderService { get; set; }
        [Inject]
        private IUserService _userService { get; set; }
        [Inject]
        private INavigationWrapper _navigationWrapper { get; set; }
        [Inject]
        private IToastService _toastService { get; set; }

        /// <summary>
        /// Number of rendered page.
        /// </summary>
        [Parameter]
        public int Page { get; set; }

        /// <summary>
        /// Creates instance of <see cref="UsersPage"/>.
        /// </summary>
        public UsersPage()
        {
            _pagedUsers = null;
        }

        protected override async Task OnParametersSetAsync()
        {
            await UpdateUsersDataAsync();
            StateHasChanged();
        }

        private void OnPageClicked(int page)
        {
            _navigationWrapper.NavigateTo($"{Routes.UsersPage}/{page}");
        }

        private async Task BlockUserAsync(BlockUserModel blockUserModel)
        {
            var blockUserView = new BlockUserAdminView
            {
                UserId = blockUserModel.UserId,
                Block = blockUserModel.Block
            };

            _loaderService.SwitchOn();
            bool blockedResult = await _userService.BlockAsync(blockUserView);
            _loaderService.SwitchOff();

            if (blockedResult)
            {
                _toastService.ShowSuccess(blockUserModel.Block ? Notifications.UserSuccessfullyBlocked : Notifications.UserSuccessfullyUnblocked);
                _navigationWrapper.NavigateTo($"{Routes.UsersPage}/{Page}");
            }
        }

        private async Task UpdateUsersDataAsync()
        {
            if (Page < 1)
            {
                Page = 1;
            }

            _loaderService.SwitchOn();
            _pagedUsers = await _userService.GetAllAsync(Page, UsersOnPage);
            _loaderService.SwitchOff();

            if (_pagedUsers is null || _pagedUsers.Items.Count == 0)
            { 
                return;
            }

            _totalPageCount = (int)Math.Ceiling(_pagedUsers.TotalItems / ((double)UsersOnPage));

            if (Page > _totalPageCount || Page < 1)
            { 
                _navigationWrapper.NavigateTo($"{Routes.UsersPage}/1");
            }
        }
    }
}
