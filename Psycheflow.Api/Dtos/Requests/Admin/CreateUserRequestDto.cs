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

        [JsonPropertyName("license_number")]
        public string? LicenseNumber { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string? Password { get; set; } = string.Empty;

        public static explicit operator RegisterUserRequestDto(CreateUserRequestDto dto)
        {
            return new RegisterUserRequestDto 
            {
                CompanyId = dto.CompanyId,
                Email = dto.Email,
                RoleName = dto.RoleName,               
                LicenseNumber = dto.LicenseNumber,
            };
        }
    }
}
