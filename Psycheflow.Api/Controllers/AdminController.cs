using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Dtos.Requests.Admin;
using Psycheflow.Api.Dtos.Requests.User;
using Psycheflow.Api.Dtos.Responses;
using Psycheflow.Api.Entities;
using Psycheflow.Api.Enums;
using Psycheflow.Api.Interfaces.Services;
using Psycheflow.Api.UseCases.Users;
using System.Security.Claims;

namespace Psycheflow.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Manager,Admin")]
    public class AdminController : ControllerBase
    {
        private AppDbContext Context { get; set; }
        private UserManager<User> UserManager { get; set; }
        private SignInManager<User> SigningManager { get; set; }
        private ITokenService TokenService { get; set; }
        private RegisterUserUseCase RegisterUserUseCase { get; set; }
        private IPasswordGeneratorService PasswordGeneratorService { get; set; }
        public AdminController(
            AppDbContext context,
            UserManager<User> manager,
            SignInManager<User> signingManager,
            ITokenService tokenService,
            RegisterUserUseCase registerUserUseCase,
            IPasswordGeneratorService passwordGeneratorService
            )
        {
            Context = context;
            UserManager = manager;
            SigningManager = signingManager;
            TokenService = tokenService;
            RegisterUserUseCase = registerUserUseCase;
            PasswordGeneratorService = passwordGeneratorService;
        }

        [HttpPost("create/user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto requestDto)
        {
            try
            {
                string randomPassword = PasswordGeneratorService.GeneratePassword();

                string userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;

                User requestUser = Context.Users.Find(userId)!;
                requestDto.CompanyId = requestUser.CompanyId;

                GenericResponseDto<User?> responseDto = await RegisterUserUseCase.Execute((RegisterUserRequestDto)requestDto, randomPassword);
                if (!responseDto.Success || responseDto.Data == null)
                {
                    throw new Exception(responseDto.Message);
                }

                return Ok(GenericResponseDto<object>.ToSuccess("Usuário criado com sucesso",new
                {
                    user = responseDto.Data,
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(GenericResponseDto<User?>.ToFail(ex.Message, null));
            }
        }
    }
}
