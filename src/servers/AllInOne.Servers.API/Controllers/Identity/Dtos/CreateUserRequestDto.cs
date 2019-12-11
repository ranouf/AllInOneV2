using System;

namespace AllInOne.Servers.API.Controllers.Identity.Dtos
{
    public class CreateUserRequestDto : RegistrationRequestDto
    {
        public Guid RoleId { get; set; }
    }
}
