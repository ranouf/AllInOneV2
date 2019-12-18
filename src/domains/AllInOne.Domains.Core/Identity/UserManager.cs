using AllInOne.Common.EntityFramework.Repositories;
using AllInOne.Common.EntityFramework.UnitOfWork;
using AllInOne.Common.Events;
using AllInOne.Common.Exceptions;
using AllInOne.Common.Extensions;
using AllInOne.Common.Logging;
using AllInOne.Common.Paging;
using AllInOne.Domains.Core.Identity.Entities;
using AllInOne.Domains.Core.Identity.Events;
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
        private readonly IDomainEvents _domainEvents;
        private readonly ILoggerService<UserManager> _logger;

        public UserManager(
            UserManager<User> userManager,
            IRoleManager roleManager,
            IUnitOfWork unitOfWork,
            IDomainEvents domainEvents,
            ILoggerService<UserManager> logger
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.GetRepository<User>();
            _domainEvents = domainEvents;
            _logger = logger;
        }

        public async Task<User> FindByIdAsync(Guid id, bool includeDeleted = false)
        {
            var result = await FindByAsync(u => u.Id == id, includeDeleted);
            if (result != null)
            {
                _logger.LogInformation($"User found: {result.ToJson()}");
            }
            else
            {
                _logger.LogInformation($"User not found with id: {id}");
            }
            return result;
        }

        public async Task<User> FindByEmailAsync(string email, bool includeDeleted = false)
        {
            var result = await FindByAsync(u => u.Email == email, includeDeleted);
            if (result != null)
            {
                _logger.LogInformation($"User found: {result.ToJson()}");
            }
            else
            {
                _logger.LogInformation($"User not found with email: {email}");
            }
            return result;
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
            await _domainEvents.RaiseAsync(
                new UserRegisteredEvent { User = result }
            );
            return result;
        }

        public async Task<User> CreateAsync(User user, string password, Role role, bool sendEmail = true, bool raiseEvent = true)
        {
            var result = await CreateAsync(user, password, role);

            if (raiseEvent)
            {
                await _domainEvents.RaiseAsync(
                    new UserCreatedEvent { User = result }
                );
            }
            return result;
        }

        public async Task DeleteAsync(User user)
        {
            var identityResult = await _userManager.DeleteAsync(user);

            if (!identityResult.Succeeded)
            {
                throw new LocalException(identityResult.Errors.First().Description);
            }

            var userToDelete = await FindByIdAsync(user.Id, includeDeleted: true);

            await _domainEvents.RaiseAsync(
                new UserDeletedEvent { User = userToDelete }
            );
        }

        public async Task AllowUserToLoginAsync(User user, bool allow, bool raiseEvent = true)
        {
            var identityResult = await _userManager.SetLockoutEnabledAsync(user, !allow);
            if (!identityResult.Succeeded)
            {
                throw new LocalException(identityResult.Errors.First().Description);
            }

            IEvent @event;

            if (allow)
            {
                user = await FindByIdAsync(user.Id);
                @event = new UserUnlockedEvent { User = user };
            }
            else
            {
                identityResult = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                if (!identityResult.Succeeded)
                {
                    throw new LocalException(identityResult.Errors.First().Description);
                }

                user = await FindByIdAsync(user.Id);
                @event = new UserLockedEvent { User = user };
            }
            if (raiseEvent)
            {
                await _domainEvents.RaiseAsync(@event);
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
            if (raiseEvent)
            {
                await _domainEvents.RaiseAsync(
                    new UserUpdatedEvent { User = result }
                );
            }

            return result;
        }

        #region Private
        private async Task<User> FindByAsync(Expression<Func<User, bool>> where, bool includeDeleted)
        {
            var result = await _userRepository.GetAll()
                .Include(u => u.CreatedByUser)
                .Include(u => u.UpdatedByUser)
                .Include(u => u.DeletedByUser)
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .IgnoreQueryFilters(includeDeleted)
                .FirstOrDefaultAsync(where);
            return result;
        }

        private async Task<User> CreateAsync(User user, string password, Role role)
        {
            var identityResult = await _userManager.CreateAsync(user, password);
            if (!identityResult.Succeeded)
            {
                throw new LocalException(identityResult.Errors.First().Description);
            }

            user = await _userManager.FindByEmailAsync(user.Email);

            await AllowUserToLoginAsync(user, allow: true, raiseEvent: false);

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
