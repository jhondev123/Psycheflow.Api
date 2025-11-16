using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Dtos.Requests.Session;
using Psycheflow.Api.Dtos.Responses;
using Psycheflow.Api.Dtos.Responses.Sessions;
using Psycheflow.Api.Entities;
using Psycheflow.Api.Utils;

namespace Psycheflow.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Manager,Admin,Psychologist,Patient")]
    public class SessionController : ControllerBase
    {
        private AppDbContext Context { get; set; }

        public SessionController(AppDbContext context) { Context = context; }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            GetSessionByIdResponseDto? sessionDto = await Context.Sessions
                .Where(s => s.Id == id)
                .Select(s => new GetSessionByIdResponseDto
                {
                    Description = s.Description,
                    Feedback = s.Feedback,
                    SessionId = s.Id,
                    PsychologistId = s.PsychologistId,
                    PsychologistName = s.Psychologist.User.NormalizedUserName!,
                    PatientId = s.PatientId,
                    PatientName = s.Patient.User.NormalizedUserName ?? string.Empty,
                    ScheduleId = s.ScheduleId,
                    ScheduleDate = s.Schedule.Date,
                    ScheduleStart = s.Schedule.Start,
                    ScheduleEnd = s.Schedule.End,
                    CompanyId = s.CompanyId,
                    CompanyName = s.Company.Name,
                    Status = s.SessionStatus
                })
                .FirstOrDefaultAsync();

            if (sessionDto is null)
            {
                return NotFound(GenericResponseDto<object>.ToFail($"Sessão com o Id {id} não encontrada"));
            }
            return Ok(GenericResponseDto<GetSessionByIdResponseDto>.ToSuccess("Sessão encontrada", sessionDto));
        }

        [HttpPost("complete")]
        public async Task<IActionResult> CompleteSession([FromBody] CompleteSessionRequestDto dto)
        {
            Session? session = await Context.Sessions.FirstOrDefaultAsync(s => s.Id == dto.SessionId);
            if (session is null)
            {
                return NotFound(GenericResponseDto<object>.ToFail($"Sessão com o Id {dto.SessionId} não encontrada"));
            }
            session.Feedback = dto.Feedback;
            session.Description = dto.Description;
            session.SessionStatus = Enums.SessionStatus.Completed;

            await Context.SaveChangesAsync();

            return Ok(GenericResponseDto<Session>.ToSuccess("Sessão completada com sucesso", session));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteById([FromRoute] Guid id)
        {
            Session? session = await Context.Sessions.FirstOrDefaultAsync(s => s.Id == id);
            if (session is null)
            {
                return NotFound(GenericResponseDto<object>.ToFail($"Sessão com o Id {id} não encontrada"));
            }
            session.DeletedAt = DateTime.Now;
            await Context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateById([FromRoute] Guid id, [FromBody] UpdateSessionRequestDto dto)
        {
            Session? session = await Context.Sessions.FirstOrDefaultAsync(s => s.Id == id);
            if (session is null)
            {
                return NotFound(GenericResponseDto<object>.ToFail($"Sessão com o Id {id} não encontrada"));
            }
            UpdateEntityHandler.Update(dto, session);

            await Context.SaveChangesAsync();

            return Ok(GenericResponseDto<Session>.ToSuccess("Sessão atualizada com sucesso",session));
        }


    }
}
