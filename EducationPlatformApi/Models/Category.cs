using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace EducationPlatformApi.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [JsonIgnore]
        public List<Course> Courses { get; set; } = new List<Course>();

    }
}
