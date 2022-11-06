using System.ComponentModel.DataAnnotations;

namespace EducationPlatformApi.Models
{
    public class UserResponse
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public Role Role { get; set; }
    }
}
