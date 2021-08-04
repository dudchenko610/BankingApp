using Bunit;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using BankingApp.ViewModels.ViewModels.Pagination;
using BankingApp.ViewModels.Banking.Admin;
using BankingApp.UI.Components.UserTable;
using System.Linq;
using BankingApp.UI.Models;

namespace BankingApp.UI.UnitTests.Components
{
    public class UserTableTests : TestContext
    {
        [Fact]
        public void UserTable_PassValidListData_ComponentContainsAsMuchDivWithSpecifiedClassElementsAsListDataCount()
        {
            var pagedUsers = GetValidPagedDataViewWithUserGetAllViewItems();
            var userTableComponent = RenderComponent<UserTable>(parameters => parameters
                .Add(p => p.UsersViewList, pagedUsers.Items)
                .Add(p => p.Page, pagedUsers.PageNumber)
                .Add(p => p.UsersOnPage, pagedUsers.PageSize)
            );

            userTableComponent.FindAll("tr").Count.Should().Be(pagedUsers.Items.Count + 1);
        }

        [Fact]
        public void UserTable_PassValidListData_ComponentRendersAllItemsFieldsInTdTags()
        {
            const int TdCount = 3;
            var pagedUsers = GetValidPagedDataViewWithUserGetAllViewItems();

            var userTableComponent = RenderComponent<UserTable>(parameters => parameters
                .Add(p => p.UsersViewList, pagedUsers.Items)
                .Add(p => p.Page, pagedUsers.PageNumber)
                .Add(p => p.UsersOnPage, pagedUsers.PageSize)
            );
            var listOfLabelTexts = userTableComponent.FindAll("td").Select(x => x.TextContent).ToList();

            for (int i = 0; i < pagedUsers.Items.Count; i++)
            {
                listOfLabelTexts[i * TdCount + 0].Should().Contain(pagedUsers.Items[i].Nickname.ToString());
                listOfLabelTexts[i * TdCount + 1].Should().Contain(pagedUsers.Items[i].Email.ToString());
            }
        }

        [Fact]
        public void DepositList_UserClicksHistoryItem_EventTriggers()
        {
            BlockUserModel blockUserModel = null;

            var pagedUsers = GetValidPagedDataViewWithUserGetAllViewItems();

            var userTableComponent = RenderComponent<UserTable>(parameters => parameters
                .Add(p => p.UsersViewList, pagedUsers.Items)
                .Add(p => p.Page, pagedUsers.PageNumber)
                .Add(p => p.UsersOnPage, pagedUsers.PageSize)
                .Add(p => p.OnBlockUserClick, x =>
                    {
                        blockUserModel = x;
                    })
            );

            userTableComponent.FindAll("input[type=checkbox]")[pagedUsers.Items.Count - 1].Change(true);
            blockUserModel.Should().NotBeNull();
            blockUserModel.UserId.Should().Be(pagedUsers.Items.Last().Id);
        }

        private PagedDataView<UserGetAllAdminViewItem> GetValidPagedDataViewWithUserGetAllViewItems()
        {
            return new PagedDataView<UserGetAllAdminViewItem>
            {
                Items = new List<UserGetAllAdminViewItem>
                {
                    new UserGetAllAdminViewItem {
                        Id = 1,
                        Nickname = "aaa",
                        Email = "a@a.com",
                        IsEmailConfirmed = true,
                        IsBlocked = false
                    },
                    new UserGetAllAdminViewItem {
                        Id = 2,
                        Nickname = "bbb",
                        Email = "b@b.com",
                        IsEmailConfirmed = true,
                        IsBlocked = false
                    }
                },
                TotalItems = 5,
                PageNumber = 1,
                PageSize = 2
            };
        }
    }
}
