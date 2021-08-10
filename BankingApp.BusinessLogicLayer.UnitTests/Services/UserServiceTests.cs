using BankingApp.BusinessLogicLayer.Services;
using BankingApp.Entities.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using BankingApp.DataAccessLayer.Interfaces;
using AutoMapper;
using BankingApp.BusinessLogicLayer.Mapper;
using BankingApp.ViewModels.Banking.Admin;
using BankingApp.DataAccessLayer.Models;
using System;
using BankingApp.Shared;

namespace BankingApp.BusinessLogicLayer.UnitTests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private const int ValidPageNumber = 1;
        private const int ValidPageSize = 1;
        private const string ValidUserId = "1";
        private const string InvalidUserId = "dds_123ds";

        private UserService _userService;
        private IMapper _mapper;
        private Mock<UserManager<User>> _userManagerMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<ClaimsPrincipal> _claimsPrincipialMock;

        [SetUp]
        public void SetUp()
        {
            var validUser = GetValidUser();

            var store = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(validUser);
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);

            var mapperConfig = new MapperConfiguration(config => { config.AddProfile(new MapperProfile()); });
            _mapper = mapperConfig.CreateMapper();

            _userRepositoryMock = new Mock<IUserRepository>();
            _claimsPrincipialMock = new Mock<ClaimsPrincipal>();

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.User).Returns(_claimsPrincipialMock.Object);

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);

            _userService = new UserService(httpContextAccessorMock.Object, _userManagerMock.Object, _userRepositoryMock.Object, _mapper);
        }

        [Test]
        public void GetSignedInUserId_InvalidHttpContextAccessor_ExpectedResults()
        {
            const int MinusOne = -1;
            _claimsPrincipialMock.Setup(x => x.FindFirst(It.IsAny<string>())).Returns(new Claim(string.Empty, InvalidUserId));

            int userId = _userService.GetSignedInUserId();
            userId.Should().Be(MinusOne);
        }

        [Test]
        public void GetSignedInUserId_ValidHttpContextAccessor_ExpectedResults()
        {
            _claimsPrincipialMock.Setup(x => x.FindFirst(It.IsAny<string>())).Returns(new Claim(string.Empty, ValidUserId));

            int userId = _userService.GetSignedInUserId();
            userId.Should().Be(int.Parse(ValidUserId));
        }

        [Test]
        public async Task GetUserByEmail_ValidEmail_ExpectedResults()
        {
            const string ValidEmail = "a@a.com";

            _claimsPrincipialMock.Setup(x => x.FindFirst(It.IsAny<string>())).Returns(new Claim(string.Empty, ValidUserId));

            var userByEmail = await _userService.GetUserByEmailAsync(ValidEmail);
            var userReturnedByUserManager = GetValidUser();

            userByEmail.Id.Should().Be(userReturnedByUserManager.Id);
            userByEmail.Email.Should().Be(userReturnedByUserManager.Email);
            userByEmail.UserName.Should().Be(userReturnedByUserManager.UserName);
            userByEmail.Deposits.Should().BeEquivalentTo(userReturnedByUserManager.Deposits);
        }

        [Test]
        public async Task GetUserByEmail_InvalidEmail_ExpectedResults()
        {
            const string InvalidEmail = "aacom";

            _claimsPrincipialMock.Setup(x => x.FindFirst(It.IsAny<string>())).Returns(new Claim(string.Empty, ValidUserId));
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User) null);

            var userByEmail = await _userService.GetUserByEmailAsync(InvalidEmail);
            userByEmail.Should().BeNull();
        }

        [Test]
        public async Task Block_ValidBlockUserView_BlockAsyncInvoked()
        {
            var validBlockUserView = GetValidBlockUserView();
            var validUser = GetValidUser();

            int userId = -1;
            bool block = false;

            _claimsPrincipialMock.Setup(x => x.FindFirst(It.IsAny<string>())).Returns(new Claim(string.Empty, ValidUserId));
            _userRepositoryMock.Setup(x => x.BlockAsync(It.IsAny<int>(), It.IsAny<bool>()))
                .Callback((int _userId, bool _block) => { userId = _userId; block = _block; });
            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(validUser);
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);
            _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());

            await _userService.BlockAsync(validBlockUserView);

            userId.Should().Be(validBlockUserView.UserId);
            block.Should().Be(validBlockUserView.Block);
        }

        [Test]
        public void Block_SignedInUserHasAdminRole_ThrowsException()
        {
            var validBlockUserView = GetValidBlockUserView();
            var validUser = GetValidUser();

            _claimsPrincipialMock.Setup(x => x.FindFirst(It.IsAny<string>())).Returns(new Claim(string.Empty, ValidUserId));
            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(validUser);
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);
            _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetListOfRoleNamesWithAdminRole());

            FluentActions.Awaiting(() => _userService.BlockAsync(validBlockUserView))
                .Should().Throw<Exception>().WithMessage(Constants.Errors.Admin.UnableToBlockUser);
        }

        [Test]
        public async Task GetAllAsync_ValidParameters_ExpectedResults()
        {
            var validPagedDataView = GetValidPagedDataViewWithUserGetAllViewItems();
            var validPagedUsers = GetValidPagedDataViewWithUsers();

            _userRepositoryMock
              .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                  .ReturnsAsync(validPagedUsers);
            _claimsPrincipialMock.Setup(x => x.FindFirst(It.IsAny<string>())).Returns(new Claim(string.Empty, ValidUserId));

            var resPagedGetAllViewItems = await _userService.GetAllAsync(ValidPageNumber, ValidPageSize);

            resPagedGetAllViewItems
                .Should().NotBeNull().And
                .BeOfType<ViewModels.ViewModels.Pagination.PagedDataView<UserGetAllAdminViewItem>>()
                .Which.Items.Should().NotBeNull();

            validPagedUsers.Items
               .Should().BeEquivalentTo(resPagedGetAllViewItems.Items,
                   options => options.ExcludingMissingMembers());
        }

        [Test]
        public void GetAllAsync_InvalidPageNumber_ThrowsException()
        {
            const int InvalidPageNumber = -1;

            _claimsPrincipialMock.Setup(x => x.FindFirst(It.IsAny<string>())).Returns(new Claim(string.Empty, ValidUserId));

            FluentActions.Awaiting(() => _userService.GetAllAsync(InvalidPageNumber, ValidPageSize))
                .Should().Throw<Exception>().WithMessage(Constants.Errors.Page.IncorrectPageNumberFormat);
        }

        [Test]
        public void GetAllAsync_InvalidPageSize_ThrowsException()
        {
            const int InvalidPageSize = -1;

            _claimsPrincipialMock.Setup(x => x.FindFirst(It.IsAny<string>())).Returns(new Claim(string.Empty, ValidUserId));

            FluentActions.Awaiting(() => _userService.GetAllAsync(ValidPageNumber, InvalidPageSize))
                .Should().Throw<Exception>().WithMessage(Constants.Errors.Page.IncorrectPageSizeFormat);
        }

        private User GetValidUser()
        {
            return new User
            {
                Id = 1,
                Email = "a@a.com",
                UserName = "Me",
                EmailConfirmed = true,
                Deposits = new List<Deposit>()
            };
        }

        private BlockUserAdminView GetValidBlockUserView()
        {
            return new BlockUserAdminView
            {
                UserId = 1,
                Block = true
            };
        }

        private PaginationModel<UserGetAllAdminViewItem> GetValidPagedDataViewWithUserGetAllViewItems()
        {
            return new PaginationModel<UserGetAllAdminViewItem>
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
                TotalCount = 5
            };
        }

        private PaginationModel<User> GetValidPagedDataViewWithUsers()
        {
            return new PaginationModel<User>
            {
                Items = new List<User>
                {
                    new User {
                        Id = 1,
                        UserName = "aaa",
                        Email = "a@a.com",
                        EmailConfirmed = true,
                        IsBlocked = false
                    },
                    new User {
                        Id = 2,
                        UserName = "bbb",
                        Email = "b@b.com",
                        EmailConfirmed = true,
                        IsBlocked = false
                    }
                },
                TotalCount = 5
            };
        }

        private IList<string> GetListOfRoleNamesWithAdminRole()
        {
            return new List<string>
            {
                Constants.Roles.Admin
            };
        }
    }
}
