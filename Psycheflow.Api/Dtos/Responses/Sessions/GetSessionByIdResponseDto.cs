using Psycheflow.Api.Enums;

namespace Psycheflow.Api.Dtos.Responses.Sessions
{
    public class GetSessionByIdResponseDto
    {
        public string Description { get; set; } = string.Empty;
        public string Feedback { get; set; } = string.Empty;
        public Guid SessionId { get; set; }
        public Guid PsychologistId { get; set; }
        public string PsychologistName { get; set; } = string.Empty;
        public Guid PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public Guid ScheduleId { get; set; }
        public DateTime ScheduleDate { get; set; }
        public TimeOnly ScheduleStart { get; set; }
        public TimeOnly ScheduleEnd { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public SessionStatus Status { get; set; }
    }
}
