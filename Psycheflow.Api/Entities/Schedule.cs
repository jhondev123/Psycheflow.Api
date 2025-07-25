namespace Psycheflow.Api.Entities
{
    public class Schedule : BaseEntity
    {
        public DateTime Date { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }

        public Guid PsychologistId { get; set; }
        public Psychologist Psychologist { get; set; }

        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }

        public Company Company { get; set; }
        public Guid CompanyId { get; set; }

        public Schedule()
        {
        }

        public Schedule(DateTime date, TimeSpan start, TimeSpan end, Guid psychologistId, Guid patientId, Guid companyId)
        {
            Date = date;
            Start = start;
            End = end;
            PsychologistId = psychologistId;
            PatientId = patientId;
            CompanyId = companyId;
        }
    }
}
