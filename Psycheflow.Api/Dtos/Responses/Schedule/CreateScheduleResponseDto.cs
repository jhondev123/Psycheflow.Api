namespace Psycheflow.Api.Dtos.Responses.Schedule
{
    public class CreateScheduleResponseDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
        public Guid PsychologistId { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public Guid? SessionId { get; set; }
        public Guid CompanyId { get; set; }
    }
}
