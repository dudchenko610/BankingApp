using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Banking.Admin;
using BankingApp.ViewModels.ViewModels.Pagination;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using static BankingApp.UI.Core.Constants.Constants;

namespace BankingApp.UI.Pages.UsersPage
{
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
        [Parameter]
        public int Page { get; set; }

        public UsersPage()
        {
            _pagedUsers = null;
        }

        protected override async Task OnInitializedAsync()
        {
        //    await UpdateUsersDataAsync();
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

        private async Task BlockUserAsync(int userId, bool blocked)
        {
            var blockUserView = new BlockUserAdminView
            {
                UserId = userId,
                Block = blocked
            };

            _loaderService.SwitchOn();
            if (await _userService.BlockAsync(blockUserView))
            {
                _loaderService.SwitchOff();
                _toastService.ShowSuccess(blocked ? Notifications.UserSuccessfullyBlocked : Notifications.UserSuccessfullyUnblocked);
                _navigationWrapper.NavigateTo($"{Routes.UsersPage}/{Page}");
            }
            else
            {
                _loaderService.SwitchOff();
                _toastService.ShowSuccess(blocked ? Notifications.ErrorWhileBlockingUser : Notifications.ErrorWhileUnblockingUser);
            }
        }

        private async Task UpdateUsersDataAsync()
        {
            if (Page < 1)
            {
                _navigationWrapper.NavigateTo($"{Routes.UsersPage}/1");
                return;
            }

            _loaderService.SwitchOn();
            _pagedUsers = await _userService.GetAllAsync(Page, UsersOnPage);
            _loaderService.SwitchOff();

            if (_pagedUsers is null || _pagedUsers.Items.Count == 0)
                return;

            _totalPageCount = (int)Math.Ceiling(_pagedUsers.TotalItems / ((double) UsersOnPage));

            if (Page > _totalPageCount || Page < 1)
                _navigationWrapper.NavigateTo($"{Routes.UsersPage}/1");
        }
    }
}
