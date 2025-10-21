using Psycheflow.Api.Enums;

namespace Psycheflow.Api.Entities
{
    public class Schedule : BaseEntity
    {
        public DateTime Date { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }

        public Guid PsychologistId { get; set; }
        public Psychologist Psychologist { get; set; }

        public ScheduleTypes ScheduleTypes { get; set; }

        public ScheduleStatus ScheduleStatus { get; set; }

        public Company Company { get; set; }
        public Guid CompanyId { get; set; }

        public Schedule()
        {
        }

        public Schedule(DateTime date, TimeOnly start, TimeOnly end, Guid psychologistId, ScheduleTypes scheduleTypes, Guid companyId,ScheduleStatus scheduleStatus)
        {
            Date = date;
            ValideteDate();
            Start = start;
            End = end;
            ValideteRange();
            PsychologistId = psychologistId;
            ScheduleTypes = scheduleTypes;
            CompanyId = companyId;
            ScheduleStatus = scheduleStatus;
        }
        private void ValideteRange()
        {
            if (End < Start)
            {
                throw new Exception("O horário final do agendamento não pode ser menor que o horário de inicio");
            }
        }
        private void ValideteDate()
        {
            if(Date.Date < DateTime.Now.Date)
            {
                throw new Exception("A data do agendamento não pode ser menor que a data de hoje");
            }
        }
    }
}
