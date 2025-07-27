using Microsoft.EntityFrameworkCore;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Dtos.Requests.Schedule;
using Psycheflow.Api.Dtos.Responses;
using Psycheflow.Api.Entities;
using Psycheflow.Api.Enums;

namespace Psycheflow.Api.UseCases.Schedules
{
    public class CreateScheduleUseCase
    {
        private AppDbContext Context { get; set; }
        public CreateScheduleUseCase(AppDbContext context)
        {
            Context = context;
        }
        public async Task<GenericResponseDto<Schedule?>> Execute(CreateScheduleRequestDto dto, User user)
        {
            try
            {
                await Context.Database.BeginTransactionAsync();

                if (!Enum.IsDefined(typeof(ScheduleTypes), dto.Type))
                {
                    throw new Exception("O type é inválido");
                }
                ScheduleTypes scheduleType = (ScheduleTypes)dto.Type;

                ScheduleStatus scheduleStatus = ScheduleStatus.Pending;
                if (dto.Status != null)
                {
                    if(!Enum.IsDefined(typeof(ScheduleStatus), dto.Status))
                    {
                        throw new InvalidOperationException("O type é inválido");
                    }

                    scheduleStatus = (ScheduleStatus)dto.Status;
                }

                Schedule schedule = new Schedule(dto.Date, dto.Start, dto.End, dto.PsychologistId, scheduleType, user.CompanyId, scheduleStatus);
                await Context.AddAsync(schedule);
                await Context.SaveChangesAsync();

                if (scheduleType == ScheduleTypes.SESSION)
                {
                    if(dto.SessionData == null)
                    {
                        throw new Exception("Os dados para criar a sessão não foram encontrados");
                    }
                    Patient? patient = await Context.Patients.FirstOrDefaultAsync(x => x.Id == dto.SessionData.PatientId);
                    if (patient == null)
                    {
                        throw new Exception("O paciente não foi encontrado");
                    }
                    await CreateSession(schedule, patient);
                }

                await Context.Database.CommitTransactionAsync();
                return GenericResponseDto<Schedule?>.ToSuccess("Agendamento criado com sucesso", schedule);
            }
            catch (Exception ex)
            {
                if (Context.Database.CurrentTransaction != null)
                {
                    await Context.Database.RollbackTransactionAsync();
                }
                return GenericResponseDto<Schedule?>.ToFail($"Erro ao criar um agendamento {ex.Message}", null);
            }
        }
        private async Task CreateSession(Schedule schedule, Patient patient)
        {
            Session session = new Session(schedule.PsychologistId, patient.Id, schedule.Id, schedule.CompanyId, SessionStatus.Scheduled);
            await Context.AddAsync(session);
            await Context.SaveChangesAsync();
        }
    }
}
