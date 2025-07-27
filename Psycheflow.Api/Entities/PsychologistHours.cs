using Psycheflow.Api.Enums;
using System.Text.Json.Serialization;

namespace Psycheflow.Api.Entities
{
    public class PsychologistHours : BaseEntity
    {
        public Company Company { get; set; }
        public Guid CompanyId { get; set; }

        [JsonIgnore] // Prevent circular reference
        public Psychologist Psychologist { get; set; }
        public Guid PsychologistId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }  
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public PsychologistHours()
        {
        }

        public PsychologistHours(Guid companyId, Guid psychologistId, TimeSpan startTime, TimeSpan endTime)
        {
            CompanyId = companyId;
            PsychologistId = psychologistId;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
