﻿using AllInOne.Common.EntityFramework.Repositories;
using AllInOne.Common.EntityFramework.UnitOfWork;
using AllInOne.Common.Exceptions;
using AllInOne.Common.Extensions;
using AllInOne.Common.Paging;
using AllInOne.Domains.Core.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AllInOne.Domains.Core.Identity
{
    public class UserManager : IUserManager
    {
        private readonly UserManager<User> _userManager;
        private readonly IRoleManager _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<User> _userRepository;

        public UserManager(
            UserManager<User> userManager,
            IRoleManager roleManager,
            IUnitOfWork unitOfWork
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.GetRepository<User>();
        }

        public Task<User> FindByIdAsync(Guid id, bool includeDeleted = false)
        {
            return FindBy(u => u.Id == id, includeDeleted);
        }

        public Task<User> FindByEmailAsync(string email, bool includeDeleted = false)
        {
            return FindBy(u => u.Email == email, includeDeleted);
        }

        public Task<PagedResult<User>> GetAllAsync(string filter, int? maxResultCount, int? SkipCount)
        {
            var query = _userRepository.GetAll()
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Select(u => u);

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(u => u.FullName.Contains(filter) || u.Email.Contains(filter));
            }

            query = query.OrderBy(o => o.FullName);

            return query.ToPagedResultAsync(maxResultCount, SkipCount);
        }

        public async Task<User> RegisterAsync(User user, string password)
        {
            var role = await _roleManager.FindByNameAsync(Constants.Roles.User);
            var result = await CreateAsync(user, password, role);
            return result;
        }

        public async Task<User> CreateAsync(User user, string password, Role role, bool sendEmail = true, bool raiseEvent = true)
        {
            var result = await CreateAsync(user, password, role);
            return result;
        }

        public async Task DeleteAsync(User user)
        {
            var identityResult = await _userManager.DeleteAsync(user);

            if (!identityResult.Succeeded)
            {
                throw new LocalException(identityResult.Errors.First().Description);
            }
        }

        public async Task AllowUserToLoginAsync(User user, bool allow)
        {
            var identityResult = await _userManager.SetLockoutEnabledAsync(user, !allow);
            if (!identityResult.Succeeded)
            {
                throw new LocalException(identityResult.Errors.First().Description);
            }

            if (!allow)
            {
                identityResult = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                if (!identityResult.Succeeded)
                {
                    throw new LocalException(identityResult.Errors.First().Description);
                }
            }
        }

        public Task<bool> CheckPasswordAsync(User user, string password)
        {
            return _userManager.CheckPasswordAsync(user, password);
        }

        public async Task ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            var identityResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!identityResult.Succeeded)
            {
                throw new LocalException(identityResult.Errors.First().Description);
            }
            user.GenerateNewSecurityStamp();
            await UpdateAsync(user);
        }

        public async Task<User> UpdateAsync(User user, bool raiseEvent = true)
        {
            var identityResult = await _userManager.UpdateAsync(user);
            if (!identityResult.Succeeded)
            {
                throw new LocalException(identityResult.Errors.First().Description);
            }
            var result = await FindByIdAsync(user.Id);

            return result;
        }

        #region Private
        private Task<User> FindBy(Expression<Func<User, bool>> where, bool includeDeleted)
        {
            return _userRepository.GetAll()
                .Include(u => u.CreatedByUser)
                .Include(u => u.UpdatedByUser)
                .Include(u => u.DeletedByUser)
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .IgnoreQueryFilters(includeDeleted)
                .FirstOrDefaultAsync(where);
        }

        private async Task<User> CreateAsync(User user, string password, Role role)
        {
            var identityResult = await _userManager.CreateAsync(user, password);
            if (!identityResult.Succeeded)
            {
                throw new LocalException(identityResult.Errors.First().Description);
            }

            user = await _userManager.FindByEmailAsync(user.Email);

            await AllowUserToLoginAsync(user, true);

            identityResult = await _userManager.AddToRoleAsync(user, role.Name);
            if (!identityResult.Succeeded)
            {
                throw new LocalException(identityResult.Errors.First().Description);
            }

            return await FindByIdAsync(user.Id);
        }
        #endregion
    }
}
