using AllInOne.Servers.API.Controllers.Dtos;
using System.ComponentModel.DataAnnotations;

namespace AllInOne.Servers.API.Controllers.Identity.Dtos
{
    public class PasswordForgottenRequestDto : IDto
    {
        [Required]
        public string Email { get; set; }
    }
}
