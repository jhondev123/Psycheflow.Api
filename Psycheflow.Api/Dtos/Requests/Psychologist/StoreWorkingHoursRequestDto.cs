using Psycheflow.Api.Dtos.Utils;
using System.Text.Json.Serialization;

namespace Psycheflow.Api.Dtos.Requests.Psychologist
{
    public class StoreWorkingHoursRequestDto
    {
        [JsonPropertyName("hours")]
        public Dictionary<int, List<TimeRangeDto>> Hours { get; set; } = new();
        [JsonPropertyName("psychologist_id")]
        public string PsychologistId { get; set; } = string.Empty;
    }
}
