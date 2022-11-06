using System.ComponentModel.DataAnnotations;

namespace EducationPlatformApi.Requests
{
    public class UpdateCategoryRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
