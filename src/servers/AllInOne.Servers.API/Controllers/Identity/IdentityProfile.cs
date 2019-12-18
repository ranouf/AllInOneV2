using AllInOne.Domains.Core.Identity.Entities;
using AllInOne.Servers.API.Controllers.Identity.Dtos;
using AllInOne.Servers.API.Extensions;
using AutoMapper;

namespace AllInOne.Servers.API.Controllers.Identity
{
    public class IdentityProfile : Profile
    {
        public IdentityProfile()
        {
            CreateMap<User, UserDto>()
                .AddFullAuditedBy();
            CreateMap<Role, RoleDto>();
        }
    }
}
