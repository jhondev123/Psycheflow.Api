using System.Text.Json.Serialization;

namespace Psycheflow.Api.Dtos.Utils
{
    public sealed class  TimeRangeDto
    {
        [JsonPropertyName("start")]
        public string Start { get; set; }
        [JsonPropertyName("end")]
        public string End { get; set; }

    }
}
