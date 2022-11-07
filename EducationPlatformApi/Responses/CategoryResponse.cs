using EducationPlatformApi.Models;
using System.Text.Json.Serialization;

namespace EducationPlatformApi.Responses
{
    public class CategoryResponse : BaseResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Category? Data { get; set; }
    }

    public class CategoryListResponse : BaseResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<Category>? Data { get; set; }
    }
}