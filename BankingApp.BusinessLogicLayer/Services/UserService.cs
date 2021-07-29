using AutoMapper;
using BankingApp.BusinessLogicLayer.Interfaces;
using BankingApp.DataAccessLayer.Interfaces;
using BankingApp.DataAccessLayer.Models;
using BankingApp.Entities.Entities;
using BankingApp.Entities.Enums;
using BankingApp.Shared;
using BankingApp.ViewModels.Banking.Admin;
using BankingApp.ViewModels.ViewModels.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace BankingApp.BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, IUserRepository userRepository, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task BlockAsync(BlockUserAdminView blockUserAdminView)
        {
            await CheckUserForAdminRole();
            await _userRepository.BlockAsync(blockUserAdminView.UserId, blockUserAdminView.Block);
        }

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

            DataAccessLayer.Models.PagedDataView<User> usersAndTotalCount
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

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        private async Task CheckUserForAdminRole()
        {
            var userId = GetSignedInUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var roleNames = await _userManager.GetRolesAsync(user);

            if (roleNames.Contains(RolesEnum.Admin.ToString()))
            {
                throw new Exception(Constants.Errors.Admin.UnableToBlockUser);
            }
        }
    }
}
