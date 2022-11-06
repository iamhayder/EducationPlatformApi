using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPlatformApi.Models
{
    public class Course
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Column(TypeName = "text")]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "text")]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        [JsonIgnore]
        public Category Category { get; set; } = new Category();

        /*[JsonIgnore]
        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();*/

        [Required]
        public Guid TrainerUserId { get; set; }


        [JsonIgnore] 
        public ICollection<UserCourse> CourseUsers { get; set; } = new List<UserCourse>();

    }

}


