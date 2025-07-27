using Psycheflow.Api.Enums;

namespace Psycheflow.Api.Entities
{
    public class Session : BaseEntity
    {
        public string Description { get; set; } = string.Empty;
        public string Feedback { get; set; } = string.Empty;
        public Guid PsychologistId { get; set; }
        public Psychologist Psychologist { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }
        public Guid ScheduleId { get; set; }
        public Schedule Schedule { get; set; }
        public Company Company { get; set; }
        public Guid CompanyId { get; set; }

        public SessionStatus SessionStatus { get; set; }

        public Session()
        {
        }

        public Session(Guid psychologistId, Guid patientId, Guid scheduleId, Guid companyId, SessionStatus sessionStatus)
        {
            PsychologistId = psychologistId;
            PatientId = patientId;
            ScheduleId = scheduleId;
            CompanyId = companyId;
            SessionStatus = sessionStatus;
        }
    }
}
