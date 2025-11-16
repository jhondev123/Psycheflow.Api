using Microsoft.EntityFrameworkCore;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Dtos.Requests.Schedule;
using Psycheflow.Api.Dtos.Responses;
using Psycheflow.Api.Dtos.Responses.Schedule;
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
        public async Task<GenericResponseDto<CreateScheduleResponseDto?>> Execute(CreateScheduleRequestDto dto, User user)
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
                    if (!Enum.IsDefined(typeof(ScheduleStatus), dto.Status))
                    {
                        throw new InvalidOperationException("O type é inválido");
                    }

                    scheduleStatus = (ScheduleStatus)dto.Status;
                }

                Schedule schedule = new Schedule(dto.Date, dto.Start, dto.End, dto.PsychologistId, scheduleType, user.CompanyId, scheduleStatus);

                if (!await VerifyPsychologistWorkInThisHour(schedule))
                {
                    throw new Exception("O psicólogo não trabalha nesse horário");
                }

                if (!await VerifyHourIsAvaliable(schedule))
                {
                    throw new Exception("Este horário já possui outros agendamentos");
                }

                await Context.AddAsync(schedule);
                await Context.SaveChangesAsync();

                Session? session = null;
                if (scheduleType == ScheduleTypes.SESSION)
                {
                    if (dto.SessionData == null)
                    {
                        throw new Exception("Os dados para criar a sessão não foram encontrados");
                    }
                    Patient? patient = await Context.Patients.FirstOrDefaultAsync(x => x.Id == dto.SessionData.PatientId);
                    if (patient == null)
                    {
                        throw new Exception("O paciente não foi encontrado");
                    }
                    session = await CreateSession(schedule, patient);
                }

                await Context.Database.CommitTransactionAsync();
                return GenericResponseDto<CreateScheduleResponseDto?>.ToSuccess("Agendamento criado com sucesso", new CreateScheduleResponseDto
                {
                    Id = schedule.Id,
                    Date = schedule.Date,
                    Start = schedule.Start,
                    End = schedule.End,
                    PsychologistId = schedule.PsychologistId,
                    Type = (int)schedule.ScheduleTypes,
                    Status = (int)schedule.ScheduleStatus,
                    CompanyId = schedule.CompanyId,
                    SessionId = session?.Id
                });
            }
            catch (Exception ex)
            {
                if (Context.Database.CurrentTransaction != null)
                {
                    await Context.Database.RollbackTransactionAsync();
                }
                return GenericResponseDto<CreateScheduleResponseDto?>.ToFail($"Erro ao criar um agendamento {ex.Message}", null);
            }
        }
        private async Task<Session> CreateSession(Schedule schedule, Patient patient)
        {
            Session session = new Session(schedule.PsychologistId, patient.Id, schedule.Id, schedule.CompanyId, SessionStatus.Scheduled);

            await Context.AddAsync(session);
            await Context.SaveChangesAsync();

            return session;
        }
        private async Task<bool> VerifyHourIsAvaliable(Schedule schedule)
        {
            return !await Context.Schedules.AnyAsync(x =>
                x.PsychologistId == schedule.PsychologistId &&
                x.Date.Date == schedule.Date.Date &&
                x.ScheduleStatus != ScheduleStatus.Cancelled &&
                ((schedule.Start >= x.Start && schedule.Start < x.End) ||
                 (schedule.End > x.Start && schedule.End <= x.End) ||
                 (schedule.Start <= x.Start && schedule.End >= x.End))
            );
        }
        private async Task<bool> VerifyPsychologistWorkInThisHour(Schedule schedule)
        {
            PsychologistHours? hours = await Context.PsychologistsHours.FirstOrDefaultAsync(
                x => x.DayOfWeek == schedule.Date.DayOfWeek &&
                     x.PsychologistId == schedule.PsychologistId &&
                     x.StartTime <= schedule.Start &&
                     x.EndTime >= schedule.End
            );

            return hours != null;
        }
    }
}
