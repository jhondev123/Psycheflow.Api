using System.Text.Json.Serialization;

namespace Psycheflow.Api.Dtos.Requests.Session
{
    public class CompleteSessionRequestDto
    {
        [JsonPropertyName("sessionId")]
        public Guid? SessionId { get; set; }
        [JsonPropertyName("feedback")]
        public string Feedback { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
