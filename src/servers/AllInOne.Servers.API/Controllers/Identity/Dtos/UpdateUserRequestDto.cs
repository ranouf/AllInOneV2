using System;
using System.ComponentModel.DataAnnotations;

namespace AllInOne.Servers.API.Controllers.Identity.Dtos
{
    public class UpdateUserRequestDto
    {
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }
        public string Description { get; set; }

        [Required]
        public Guid RoleId { get; set; }
    }
}
