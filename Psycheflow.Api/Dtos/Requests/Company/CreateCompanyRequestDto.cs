using System.Text.Json.Serialization;

namespace Psycheflow.Api.Dtos.Requests.Company
{
    public class CreateCompanyRequestDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
