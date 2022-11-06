using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EducationPlatformApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        /*[JsonIgnore]
        public ICollection<Course> Courses { get; set; } = new List<Course>();*/
        
        [Required]
        public Role Role { get; set; }

        [JsonIgnore] 
        public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();

    }
}
