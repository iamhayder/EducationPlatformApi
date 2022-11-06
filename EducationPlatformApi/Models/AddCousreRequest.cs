using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EducationPlatformApi.Models
{
    public class AddCourseRequest
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        
        [Column(TypeName = "text")]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "text")]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

    }
}
