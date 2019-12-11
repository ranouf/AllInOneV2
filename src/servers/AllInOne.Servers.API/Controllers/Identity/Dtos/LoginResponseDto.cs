using AllInOne.Servers.API.Controllers.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllInOne.Servers.API.Controllers.Identity.Dtos
{
    public class LoginResponseDto : IDto
    {
        public string Token { get; set; }
        public UserDto CurrentUser { get; set; }
    }
}
