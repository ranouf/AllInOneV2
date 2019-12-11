using AllInOne.Servers.API.Controllers.Dtos;
using System;

namespace AllInOne.Servers.API.Controllers.Identity.Dtos
{
    public class RoleDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
