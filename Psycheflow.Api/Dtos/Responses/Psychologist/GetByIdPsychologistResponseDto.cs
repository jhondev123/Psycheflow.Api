namespace Psycheflow.Api.Dtos.Responses.Psychologist
{
    public class GetByIdPsychologistResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LicenseNumber { get; set; }
        public string ApproachType { get; set; }
        public string CompanyId { get; set; }
        public IEnumerable<GetByIdPsychologistHourResponseDto> Hours { get; set; }
        public IEnumerable<GetByIdPsychologistScheduleResponseDto> Schedules { get; set; }
        public IEnumerable<GetByIdPsychologistSessionsResponseDto> Sessions { get; set; }
    }
    public class GetByIdPsychologistHourResponseDto
    {
        public string DayOfWeek { get; set; }
        public int DayOfWeekNumber { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
    public class GetByIdPsychologistScheduleResponseDto
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public DateTime Date { get; set; }
        public string ScheduleType { get; set; }
        public string ScheduleStatus { get; set; }
    }
    public class GetByIdPsychologistSessionsResponseDto
    {
        public string Description { get; set; }
        public string Feedback { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
    }
}
