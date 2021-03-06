using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Core.Services;
using BankingApp.ViewModels.Banking.Admin;
using Bunit;
using Moq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using static BankingApp.Shared.Constants;
using BankingApp.ViewModels.ViewModels.Pagination;
using System.Collections.Generic;

namespace BankingApp.UI.Core.UnitTests.Services
{
    public class UserServiceTests : TestContext
    {
        private UserService _userService;
        private Mock<IHttpService> _httpServiceMock; 

        public UserServiceTests()
        {
            _httpServiceMock = new Mock<IHttpService>();
            _userService = new UserService(_httpServiceMock.Object);
        }

        [Fact]
        public async Task Block_ValidBlockUserView_PostAsyncInvoked()
        {
            BlockUserAdminView blockUserView = null;
            string passedUrl = null;
            
            _httpServiceMock.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<BlockUserAdminView>(), It.IsAny<bool>()))
                .Callback((string url, object blockUserAdminView, bool authorized) =>
                {
                    passedUrl = url;
                    blockUserView = (BlockUserAdminView) blockUserAdminView;
                });

            var blockUserview = GetValidBlockUserView();
            await _userService.BlockAsync(blockUserview);

            passedUrl.Should().Be($"{Routes.Admin.Route}/{Routes.Admin.BlockUser}");
            blockUserView.Should().NotBeNull().And.BeEquivalentTo(blockUserview);
        }

        [Fact]
        public async Task GetAll_ValidBlockUserView_GetAllAsyncInvoked()
        {
            const int ValidPageNumber = 1;
            const int ValidPageSize = 2;
            var validInputPagedDataView = GetValidPagedDataViewWithUserGetAllViewItems();

            string passedUrl = null;

            _httpServiceMock.Setup(x => x.GetAsync<PagedDataView<UserGetAllAdminViewItem>>(It.IsAny<string>(), It.IsAny<bool>()))
                .Callback((string url, bool authorized) =>
                {
                    passedUrl = url;
                })
                .ReturnsAsync(validInputPagedDataView);

            var blockUserview = GetValidBlockUserView();
            var respondedPageDataView = await _userService.GetAllAsync(ValidPageNumber, ValidPageSize);

            passedUrl.Should().Be($"{Routes.Admin.Route}/{Routes.Admin.GetAll}?pageNumber={ValidPageNumber}&pageSize={ValidPageSize}");
            respondedPageDataView.Should().NotBeNull().And.BeEquivalentTo(respondedPageDataView);
        }

        private BlockUserAdminView GetValidBlockUserView()
        {
            return new BlockUserAdminView
            {
                UserId = 1,
                Block = true
            };
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
