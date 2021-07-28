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

        private UserManager<User> _userManager;
        private IUserRepository _userRepository;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var validUser = GetValidUser();

            var store = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(validUser);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);

            _userManager = userManagerMock.Object;

            var mapperConfig = new MapperConfiguration(config => { config.AddProfile(new MapperProfile()); });
            _mapper = mapperConfig.CreateMapper();

            var userRepositoryMock = new Mock<IUserRepository>();
            _userRepository = userRepositoryMock.Object;
        }

        [Test]
        public void GetSignedInUserId_InvalidHttpContextAccessorInjected_ReturnsMinusOne()
        {
            const int MinusOne = -1;

            var validHttpContextAccessor = GetMockedHttpContextAccessor(InvalidUserId);
            var userService = new UserService(validHttpContextAccessor, _userManager, _userRepository, _mapper);

            int userId = userService.GetSignedInUserId();
            userId.Should().Be(MinusOne);
        }

        [Test]
        public void GetSignedInUserId_ValidHttpContextAccessorInjected_ReturnsValidId()
        {
            var validHttpContextAccessor = GetMockedHttpContextAccessor(ValidUserId);
            var userService = new UserService(validHttpContextAccessor, _userManager, _userRepository, _mapper);

            int userId = userService.GetSignedInUserId();
            userId.Should().Be(int.Parse(ValidUserId));
        }

        [Test]
        public async Task GetUserByEmail_ValidEmailPassed_ReturnsValidUser()
        {
            const string ValidEmail = "a@a.com";

            var validHttpContextAccessor = GetMockedHttpContextAccessor(ValidUserId);
            var userService = new UserService(validHttpContextAccessor, _userManager, _userRepository, _mapper);

            var userByEmail = await userService.GetUserByEmailAsync(ValidEmail);
            var userReturnedByUserManager = GetValidUser();

            userByEmail.Id.Should().Be(userReturnedByUserManager.Id);
            userByEmail.Email.Should().Be(userReturnedByUserManager.Email);
            userByEmail.UserName.Should().Be(userReturnedByUserManager.UserName);
            userByEmail.Deposits.Should().BeEquivalentTo(userReturnedByUserManager.Deposits);
        }

        [Test]
        public async Task GetUserByEmail_InvalidEmailPassed_ReturnsNull()
        {
            const string InvalidEmail = "aacom";

            var validHttpContextAccessor = GetMockedHttpContextAccessor(ValidUserId);
            var store = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User) null);

            var userService = new UserService(validHttpContextAccessor, userManagerMock.Object, _userRepository, _mapper);
            var userByEmail = await userService.GetUserByEmailAsync(InvalidEmail);
            userByEmail.Should().BeNull();
        }

        [Test]
        public async Task Block_ValidBlockUserViewPassed_CallsBlockAsyncOfUserRepository()
        {
            var validBlockUserView = GetValidBlockUserView();
            var validUser = GetValidUser();

            int userId = -1;
            bool block = false;

            var validHttpContextAccessor = GetMockedHttpContextAccessor(ValidUserId);
           
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.BlockAsync(It.IsAny<int>(), It.IsAny<bool>()))
                .Callback((int _userId, bool _block) => { userId = _userId; block = _block; });

            var store = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(validUser);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);
            userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());

            var userService = new UserService(validHttpContextAccessor, userManagerMock.Object, userRepositoryMock.Object, _mapper);
            await userService.BlockAsync(validBlockUserView);

            userId.Should().Be(validBlockUserView.UserId);
            block.Should().Be(validBlockUserView.Block);
        }

        [Test]
        public void Block_ValidBlockUserViewPassedButSignedInUserHasAdminRole_ThrowsExceptionWithCorrespondingMessage()
        {
            var validBlockUserView = GetValidBlockUserView();
            var validUser = GetValidUser();

            var validHttpContextAccessor = GetMockedHttpContextAccessor(ValidUserId);

            var store = new Mock<IUserStore<User>>();
            var userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(validUser);
            userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(validUser);
            userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(GetListOfRoleNamesWithAdminRole());

            var userService = new UserService(validHttpContextAccessor, userManagerMock.Object, _userRepository, _mapper);

            FluentActions.Awaiting(() => userService.BlockAsync(validBlockUserView))
                .Should().Throw<Exception>().WithMessage(Constants.Errors.Admin.UnableToBlockUser);
        }

        [Test]
        public async Task GetAllAsync_CallGetAllMethodPassingValidParameters_ReturnsNotNullModelContainingCorrectlyMappedList()
        {
            var validPagedDataView = GetValidPagedDataViewWithUserGetAllViewItems();
            var validPagedUsers = GetValidPagedDataViewWithUsers();

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock
              .Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                  .ReturnsAsync(validPagedUsers);

            var validHttpContextAccessor = GetMockedHttpContextAccessor(ValidUserId);
            var userService = new UserService(validHttpContextAccessor, _userManager, userRepositoryMock.Object, _mapper);

            var resPagedGetAllViewItems = await userService.GetAllAsync(ValidPageNumber, ValidPageSize);

            resPagedGetAllViewItems
                .Should().NotBeNull().And
                .BeOfType<ViewModels.ViewModels.Pagination.PagedDataView<UserGetAllAdminViewItem>>()
                .Which.Items.Should().NotBeNull();

            validPagedUsers.Items
               .Should().BeEquivalentTo(resPagedGetAllViewItems.Items,
                   options => options.ExcludingMissingMembers());
        }

        [Test]
        public void GetAllAsync_CallGetAllMethodPassingInvalidPageNumber_ThrowsExceptionWithCorrespondingMessage()
        {
            const int InvalidPageNumber = -1;

            var validHttpContextAccessor = GetMockedHttpContextAccessor(ValidUserId);
            var userService = new UserService(validHttpContextAccessor, _userManager, _userRepository, _mapper);

            FluentActions.Awaiting(() => userService.GetAllAsync(InvalidPageNumber, ValidPageSize))
                .Should().Throw<Exception>().WithMessage(Constants.Errors.Page.IncorrectPageNumberFormat);
        }

        [Test]
        public void GetAllAsync_CallGetAllMethodPassingInvalidPageSize_ThrowsExceptionWithCorrespondingMessage()
        {
            const int InvalidPageSize = -1;

            var validHttpContextAccessor = GetMockedHttpContextAccessor(ValidUserId);
            var userService = new UserService(validHttpContextAccessor, _userManager, _userRepository, _mapper);

            FluentActions.Awaiting(() => userService.GetAllAsync(ValidPageNumber, InvalidPageSize))
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

        private IHttpContextAccessor GetMockedHttpContextAccessor(string userId)
        {
            var claimsPrincipialMock = new Mock<ClaimsPrincipal>();
            claimsPrincipialMock.Setup(x => x.FindFirst(It.IsAny<string>())).Returns(new Claim(string.Empty, userId));

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.User).Returns(claimsPrincipialMock.Object);

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);

            return httpContextAccessorMock.Object;
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
                TotalCount = 5
            };
        }

        private PagedDataView<User> GetValidPagedDataViewWithUsers()
        {
            return new PagedDataView<User>
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
