using AllInOne.Servers.API.Controllers.Dtos;

namespace AllInOne.Servers.API.Controllers.Identity.Dtos
{
    public class ProfileResponseDto : IDto
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
