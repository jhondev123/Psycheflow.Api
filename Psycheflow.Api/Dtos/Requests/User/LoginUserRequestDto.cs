using System.Text.Json.Serialization;

namespace Psycheflow.Api.Dtos.Requests.User
{
    public class LoginUserRequestDto
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
