using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Dtos.Requests.Company;
using Psycheflow.Api.Dtos.Requests.User;
using Psycheflow.Api.Dtos.Responses;
using Psycheflow.Api.Entities;
using Psycheflow.Api.Interfaces.Services;
using Psycheflow.Api.UseCases.Users;

namespace Psycheflow.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private AppDbContext Context { get; set; }
        private UserManager<User> UserManager { get; set; }
        private SignInManager<User> SigningManager { get; set; }
        private ITokenService TokenService { get; set; }
        private RegisterUserUseCase RegisterUserUseCase { get; set; }
        public UserController(
            AppDbContext context,
            UserManager<User> manager,
            SignInManager<User> signingManager,
            ITokenService tokenService,
            RegisterUserUseCase registerUserUseCase
            )
        {
            Context = context;
            UserManager = manager;
            SigningManager = signingManager;
            TokenService = tokenService;
            RegisterUserUseCase = registerUserUseCase;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto requestDto)
        {
            GenericResponseDto<object?> responseDto = await RegisterUserUseCase.Execute(requestDto, requestDto.Password);
            if (!responseDto.Success)
            {
                return BadRequest(responseDto);
            }

            return Ok(responseDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestDto requestDto)
        {
            User? user = await UserManager.FindByEmailAsync(requestDto.Email);
            if (user == null)
            {
                return Unauthorized(GenericResponseDto<object>.ToFail("Usuário ou senha inválidos"));
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await SigningManager.CheckPasswordSignInAsync(user, requestDto.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized(GenericResponseDto<object>.ToFail("Usuário ou senha inválidos"));
            }
            List<string> roles = (await UserManager.GetRolesAsync(user)).ToList();

            string token = TokenService.GenerateToken(user, roles);

            return Ok(GenericResponseDto<object>.ToSuccess("Login realizado com sucesso", new { token }));
        }
    }
}
