using AllInOne.Domains.Core.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllInOne.Domains.Core.Identity
{
    public interface IRoleManager
    {
        Task<Role> FindByIdAsync(Guid id);
        Task<Role> FindByNameAsync(string name);
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role> CreateAsync(Role role);
    }
}