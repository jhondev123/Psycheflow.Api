using System.Text.Json.Serialization;

namespace Psycheflow.Api.Dtos.Requests.Schedule
{
    public sealed class CreateScheduleRequestDto
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("start")]
        public TimeSpan Start { get; set; }
        [JsonPropertyName("end")]
        public TimeSpan End { get; set; }
        [JsonPropertyName("psychologist_id")]
        public Guid PsychologistId { get; set; }
        [JsonPropertyName("type")]
        public int Type { get; set; }
        [JsonPropertyName("session")]
        public SessionDataDto? SessionData { get; set; } = null;

        [JsonPropertyName("status")]
        public int? Status { get; set; } = null;

    }
}
