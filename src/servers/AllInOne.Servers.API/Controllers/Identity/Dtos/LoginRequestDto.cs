using AllInOne.Servers.API.Controllers.Dtos;
using System.ComponentModel.DataAnnotations;

namespace AllInOne.Servers.API.Controllers.Identity.Dtos
{
    public class LoginRequestDto : IDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
