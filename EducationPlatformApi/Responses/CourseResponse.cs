using EducationPlatformApi.Models;
using System.Text.Json.Serialization;

namespace EducationPlatformApi.Responses
{
    public class CourseResponse : BaseResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Course? Data { get; set; }
    }
    public class CourseListResponse : BaseResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<Course>? Data { get; set; }
    }
}
