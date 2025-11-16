using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Dtos.Requests.Psychologist;
using Psycheflow.Api.Dtos.Responses;
using Psycheflow.Api.Dtos.Responses.Psychologist;
using Psycheflow.Api.Dtos.Utils;
using Psycheflow.Api.Entities;
using Psycheflow.Api.Enums;
using Psycheflow.Api.Utils;
using System.ComponentModel.Design;

namespace Psycheflow.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Manager,Admin,Psychologist")]
    public class PsychologistController : ControllerBase
    {
        private AppDbContext Context { get; set; }
        private UserManager<User> UserManager { get; set; }

        public PsychologistController(AppDbContext context, UserManager<User> userManager)
        {
            Context = context;
            UserManager = userManager;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            GetByIdPsychologistResponseDto? psychologist = await GetByIdPsychologistResponseDto(id);

            if (psychologist == null)
            {
                return NotFound(GenericResponseDto<object?>.ToFail($"Psicólogo com o id {id} não encontrado"));
            }
            return Ok(psychologist);
        }

        private async Task<GetByIdPsychologistResponseDto?> GetByIdPsychologistResponseDto(Guid id)
        {
            return await Context.Psychologists
            .Where(x => x.Id == id)
            .Select(p => new GetByIdPsychologistResponseDto
            {
                Id = p.Id,
                Name = p.User.UserName,
                LicenseNumber = p.LicenseNumber.Value,
                ApproachType = p.Approach.ToString(),
                CompanyId = p.CompanyId.ToString(),
                Hours = p.PsychologistHours.Select(h => new GetByIdPsychologistHourResponseDto
                {
                    DayOfWeek = h.DayOfWeek.ToString(),
                    DayOfWeekNumber = (int)h.DayOfWeek,
                    StartTime = h.StartTime,
                    EndTime = h.EndTime
                }),
                Schedules = p.Schedules.Select(s => new GetByIdPsychologistScheduleResponseDto
                {
                    Date = s.Date,
                    StartTime = s.Start,
                    EndTime = s.End,
                    ScheduleStatus = s.ScheduleStatus.ToString(),
                    ScheduleType = s.ScheduleTypes.ToString()
                }),
                Sessions = p.Sessions.Select(s => new GetByIdPsychologistSessionsResponseDto
                {
                    Description = s.Description,
                    Feedback = s.Feedback,
                    PatientId = s.PatientId.ToString(),
                    PatientName = s.Patient.User.UserName ?? ""
                })
            })
            .FirstOrDefaultAsync();
        }

        [HttpPost("working/hours")]

        public async Task<IActionResult> StoreWorkingHours([FromBody] StoreWorkingHoursRequestDto dto)
        {
            try
            {
                User? requestUser = await GetUserRequester.Execute(Context, this);
                if (requestUser == null)
                {
                    throw new Exception("Usuário não encontrdo");
                }


                if (!Guid.TryParse(dto.PsychologistId, out Guid psychologistId))
                {
                    throw new Exception("O Id do psicólogo é inválido");
                }

                await Context.Database.BeginTransactionAsync();

                await Context.PsychologistsHours
                .Where(ph => ph.CompanyId == requestUser.CompanyId && ph.PsychologistId == psychologistId)
                .ExecuteDeleteAsync();

                List<PsychologistHours> hours = new List<PsychologistHours>();
                foreach (int dayOfWeek in dto.Hours.Keys)
                {
                    foreach (TimeRangeDto range in dto.Hours[dayOfWeek])
                    {
                        if (!TimeOnly.TryParse(range.Start, out TimeOnly start))
                        {
                            throw new Exception($"O Horário {range.Start} está inválido");
                        }
                        if (!TimeOnly.TryParse(range.End, out TimeOnly end))
                        {
                            throw new Exception($"O Horário {range.End} está inválido");
                        }
                        if (start >= end)
                        {
                            throw new Exception($"O Horário de inicio {start} é maior que o horário final {end}");
                        }
                        PsychologistHours hour = new PsychologistHours
                        {
                            PsychologistId = psychologistId,
                            CompanyId = requestUser.CompanyId,
                            DayOfWeek = (DayOfWeek)dayOfWeek,
                            StartTime = start,
                            EndTime = end,
                        };
                        hours.Add(hour);
                    }

                }
                await Context.AddRangeAsync(hours);

                await Context.SaveChangesAsync();
                await Context.Database.CommitTransactionAsync();

                return Ok(GenericResponseDto<Dictionary<int, List<TimeRangeDto>>>.ToSuccess("Horários criados com sucesso", dto.Hours));
            }
            catch (Exception ex)
            {
                await Context.Database.RollbackTransactionAsync();
                return BadRequest(GenericResponseDto<User?>.ToFail(ex.Message, null));
            }
        }

        [HttpGet("{psychologistId:guid}/unavaliable/hours")]
        public async Task<IActionResult> GetUnavaliableHours([FromRoute] Guid psychologistId)
        {
            // receber por parâmetro da requisição
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now.AddMonths(1);

            User? requestUser = await GetUserRequester.Execute(Context, this);
            if (requestUser == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            GetByIdPsychologistResponseDto? psychologist = await GetByIdPsychologistResponseDto(psychologistId);

            if (psychologist == null)
            {
                return NotFound(GenericResponseDto<object?>.ToFail($"Psicólogo com o id {psychologistId} não encontrado"));
            }

            psychologist.Schedules = psychologist.Schedules
            .Where(s => s.Date >= startDate && s.Date <= endDate)
            .ToList();

            return Ok(GenericResponseDto<GetByIdPsychologistResponseDto>.ToSuccess("Lista de horários", psychologist));
        }
    }
}