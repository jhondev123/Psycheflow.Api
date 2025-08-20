using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Dtos.Requests.Schedule;
using Psycheflow.Api.Dtos.Responses;
using Psycheflow.Api.Entities;
using Psycheflow.Api.UseCases.Schedules;
using Psycheflow.Api.Utils;

namespace Psycheflow.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Manager,Admin,Psychologist")]
    public class ScheduleController : ControllerBase
    {

        private AppDbContext Context { get; set; }
        private CreateScheduleUseCase CreateScheduleUseCase { get; set; }
        public ScheduleController(AppDbContext context, CreateScheduleUseCase createScheduleUseCase)
        {
            Context = context;
            CreateScheduleUseCase = createScheduleUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSchedule([FromBody] CreateScheduleRequestDto requestDto)
        {
            User? requestUser = await GetUserRequester.Execute(Context, this);
            if (requestUser == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            GenericResponseDto<Schedule?> genericResponseDto = await CreateScheduleUseCase.Execute(requestDto, requestUser);

            if (genericResponseDto.Success == false)
            {
                return BadRequest(genericResponseDto);
            }
            return Ok(genericResponseDto);
        }
    }
}
