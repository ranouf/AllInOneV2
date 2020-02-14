using AllInOne.Servers.API.Controllers.Dtos;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AllInOne.Servers.API.Controllers.Identity.Dtos
{
    public class ChangeProfileRequestDto : IDto
    {
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        public IFormFile ProfileImage { get; set; }
    }
}
