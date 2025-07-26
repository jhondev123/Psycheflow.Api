using System.Text.Json.Serialization;

namespace Psycheflow.Api.Dtos.Requests.User
{
    public sealed class RegisterUserRequestDto
    {
        [JsonPropertyName("company_id")]
        public Guid? CompanyId { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("role_name")]
        public string? RoleName { get; set; } = string.Empty;
        [JsonPropertyName("license_number")]
        public string? LicenseNumber { get; set; } = string.Empty;

    }
}
