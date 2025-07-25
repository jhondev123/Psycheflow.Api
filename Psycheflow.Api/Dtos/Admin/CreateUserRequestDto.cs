using Psycheflow.Api.Dtos.Requests.User;
using System.Text.Json.Serialization;

namespace Psycheflow.Api.Dtos.Requests.Admin
{
    public sealed class CreateUserRequestDto
    {
        [JsonIgnore]
        public Guid? CompanyId { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("role_name")]
        public string RoleName { get; set; }

        public static explicit operator RegisterUserRequestDto(CreateUserRequestDto dto)
        {
            return new RegisterUserRequestDto 
            {
                CompanyId = dto.CompanyId,
                Email = dto.Email,
                RoleName = dto.RoleName,               
            };
        }
    }
}
