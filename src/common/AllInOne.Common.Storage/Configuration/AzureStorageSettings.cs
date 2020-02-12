using System.ComponentModel.DataAnnotations;

namespace AllInOne.Common.Storage.Configuration
{
    public class AzureStorageSettings
    {
        [Required]
        public string ConnectionString { get; set; }
        [Required]
        [StringLength(63, MinimumLength = 3)]
        [RegularExpression(@"^[a-z0-9-]*$")]
        public string Container { get; set; }
    }
}
