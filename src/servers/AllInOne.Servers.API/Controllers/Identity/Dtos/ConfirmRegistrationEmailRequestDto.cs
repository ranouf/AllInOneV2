using AllInOne.Servers.API.Controllers.Dtos;

namespace AllInOne.Servers.API.Controllers.Identity.Dtos
{
    public class ConfirmRegistrationEmailRequestDto : IDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
