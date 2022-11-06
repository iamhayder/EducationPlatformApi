namespace EducationPlatformApi.Models
{
    public class UserCourse
    {
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = new ApplicationUser();
        public int CourseId { get; set; }
        public Course Course { get; set; } = new Course();
    }
}
