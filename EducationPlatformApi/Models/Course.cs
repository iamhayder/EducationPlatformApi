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
        public string Name { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }

    }
}
