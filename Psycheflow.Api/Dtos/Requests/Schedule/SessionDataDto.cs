using System.Text.Json.Serialization;

namespace Psycheflow.Api.Dtos.Requests.Schedule
{
    public sealed class SessionDataDto
    {
        [JsonPropertyName("patient_id")]
        public Guid? PatientId { get; set; }
    }
}
