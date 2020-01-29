using System.ComponentModel.DataAnnotations;

namespace AllInOne.Common.Smtp.Configuration
{
    public class SmtpSettings
    {
        [Required]
        public string Server { get; set; }
        [Required]
        public int Port { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string DefaultFrom { get; set; }
        [Required]
        public bool EnableSsl { get; set; }
    }
}
