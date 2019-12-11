using AllInOne.Common.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace AllInOne.Domains.Core.Identity.Entities
{
    public class Role : IdentityRole<Guid>, IEntity
    {
        public ICollection<UserRole> UserRoles { get; set; }

        public Role() { }

        public Role(string name)
        {
            Name = name;
            NormalizedName = name.ToUpper();
        }
    }
}
