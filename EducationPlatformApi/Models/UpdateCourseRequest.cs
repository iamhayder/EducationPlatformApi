namespace EducationPlatformApi.Models
{
    public class UpdateCourseRequest
    {
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }
}
