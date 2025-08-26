using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Dtos.Requests.Session;
using Psycheflow.Api.Dtos.Responses;
using Psycheflow.Api.Entities;

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
            Session? session = await Context.Sessions.Where(s => s.Id == id)
                .Include(x => x.Schedule)
                .Include(x => x.Patient)
                .Include(x => x.Psychologist)
                .Include(x => x.Company)
                .FirstOrDefaultAsync();

            if (session is null)
            {
                return NotFound(GenericResponseDto<object>.ToFail($"Sessão com o Id {id} não encontrada"));
            }
            return Ok(GenericResponseDto<Session>.ToSuccess("Sessão encontrada",session));
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
    }
}
