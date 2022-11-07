using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using EducationPlatformApi.Models;
using Microsoft.AspNetCore.Identity;

namespace EducationPlatformApi.Responses
{
    public class User
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public Role Role { get; set; }
    }

    public class UserResponse : BaseResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public User? Data { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<IdentityError>? Errors { get; set; }

    }

    public class UserListResponse : BaseResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<User>? Data { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<IdentityError>? Errors { get; set; }
    }

}
