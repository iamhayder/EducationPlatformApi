using System.ComponentModel.DataAnnotations;

namespace EducationPlatformApi.Models
{
    public class AddCategoryRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

    }
}
