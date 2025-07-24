namespace Psycheflow.Api.Entities
{
    public class Session : BaseEntity
    {
        public string Description { get; set; }
        public string Feedback { get; set; }
        public Guid PsychologistId { get; set; }
        public Psychologist Psychologist { get; set; }
        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }
        public Guid ScheduleId { get; set; }
        public Schedule Schedule { get; set; }
        public Company Company { get; set; }
        public Guid CompanyId { get; set; }

        public Session()
        {
        }

        public Session(string description, string feedback, Guid psychologistId, Guid patientId, Guid scheduleId, Guid companyId)
        {
            Description = description;
            Feedback = feedback;
            PsychologistId = psychologistId;
            PatientId = patientId;
            ScheduleId = scheduleId;
            CompanyId = companyId;
        }
    }
}
