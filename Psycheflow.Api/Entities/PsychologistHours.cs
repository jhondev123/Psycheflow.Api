using Psycheflow.Api.Enums;

namespace Psycheflow.Api.Entities
{
    public class PsychologistHours : BaseEntity
    {
        public Company Company { get; set; }
        public Guid CompanyId { get; set; }

        public Psychologist Psychologist { get; set; }
        public Guid PsychologistId { get; set; }
        public WeekDay DayOfWeek { get; set; }  
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
