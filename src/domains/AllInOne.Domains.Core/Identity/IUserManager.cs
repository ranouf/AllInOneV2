using AllInOne.Common.Paging;
using AllInOne.Domains.Core.Identity.Entities;
using System;
using System.Threading.Tasks;

namespace AllInOne.Domains.Core.Identity
{
    public interface IUserManager
    {
        Task AllowUserToLoginAsync(User user, bool allow);
        Task<User> FindByIdAsync(Guid id, bool includeDeleted = false);
        Task<User> FindByEmailAsync(string email, bool includeDeleted = false);
        Task<PagedResult<User>> GetAllAsync(string filter, int? maxResultCount, int? SkipCount);
        Task<User> CreateAsync(User user, string password, Role role, bool sendEmail = true, bool raiseEvent = true);
        Task<User> RegisterAsync(User user, string password);
        Task DeleteAsync(User user);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task ChangePasswordAsync(User user, string currentPassword, string newPassword);
        Task<User> UpdateAsync(User user, bool raiseEvent = true);
    }
}