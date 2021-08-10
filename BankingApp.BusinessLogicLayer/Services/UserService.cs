using AutoMapper;
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.DataAccessLayer.Interfaces;
using BankingApp.DataAccessLayer.Models;
using BankingApp.Entities.Entities;
using BankingApp.Entities.Enums;
using BankingApp.Shared;
using BankingApp.ViewModels.Banking.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Services
{
    /// <summary>
    /// Allows to provide operations with users.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Creates instance of <see cref="UserService"/>.
        /// </summary>
        /// <param name="httpContextAccessor">An instance of <see cref="IHttpContextAccessor"/>.</param>
        /// <param name="userManager">An instance of <see cref="UserManager{TUser}"/>.</param>
        /// <param name="userRepository">An instance of <see cref="IUserRepository"/>.</param>
        /// <param name="mapper">An instance of <see cref="IMapper"/>.</param>
        public UserService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, IUserRepository userRepository, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Allow to block / unblock specified user.
        /// </summary>
        /// <param name="blockUserAdminView">View model containing user id and block operation type (block / unlock).</param>
        /// <exception cref="Exception">If the user that is blocking has the admin role.</exception>
        public async Task BlockAsync(BlockUserAdminView blockUserAdminView)
        {
            await CheckUserForAdminRole(blockUserAdminView.UserId);
            await _userRepository.BlockAsync(blockUserAdminView.UserId, blockUserAdminView.Block);
        }

        /// <summary>
        /// Allows getting page of users.
        /// </summary>
        /// <param name="pageNumber">Requested page number.</param>
        /// <param name="pageSize">How much elements contains single page.</param>
        /// <returns>View model with data about all users in storage and users list for specified page.</returns>
        public async Task<ViewModels.ViewModels.Pagination.PagedDataView<UserGetAllAdminViewItem>> GetAllAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
            { 
                throw new Exception(Constants.Errors.Page.IncorrectPageNumberFormat);
            }

            if (pageSize < 1)
            { 
                throw new Exception(Constants.Errors.Page.IncorrectPageSizeFormat);
            }

            PaginationModel<User> usersAndTotalCount
                = await _userRepository.GetAllAsync((pageNumber - 1) * pageSize, pageSize);

            var pagedResponse = new ViewModels.ViewModels.Pagination.PagedDataView<UserGetAllAdminViewItem>
            {
                Items = _mapper.Map<IList<User>, IList<UserGetAllAdminViewItem>>(usersAndTotalCount.Items),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = usersAndTotalCount.TotalCount
            };

            return pagedResponse;
        }

        /// <summary>
        /// Gets id of user that makes request.
        /// </summary>
        /// <returns>Id of user that makes request. If it is invalid, -1 will be returned.</returns>
        public int GetSignedInUserId()
        {
            var userIdTextRepresentation = _httpContextAccessor.HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sub).Value;

            try
            {
                return int.Parse(userIdTextRepresentation);
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Gets user by its email.
        /// </summary>
        /// <param name="email">User's email.</param>
        /// <returns>Requested user.</returns>
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        private async Task CheckUserForAdminRole(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var roleNames = await _userManager.GetRolesAsync(user);

            if (roleNames.Contains(RolesEnum.Admin.ToString()))
            {
                throw new Exception(Constants.Errors.Admin.UnableToBlockUser);
            }
        }
    }
}
