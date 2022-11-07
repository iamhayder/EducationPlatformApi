using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EducationPlatformApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public Role Role { get; set; }

        [JsonIgnore]
        public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
    }
}
