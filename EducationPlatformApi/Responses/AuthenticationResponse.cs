using System.Text.Json.Serialization;

namespace EducationPlatformApi.Responses
{

    public class BaseAuthenticationResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
    public class AuthenticationResponse : BaseResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public BaseAuthenticationResponse? Data { get; set; }
    }
}
