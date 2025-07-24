using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Dtos.Requests.Company;
using Psycheflow.Api.Dtos.Requests.User;
using Psycheflow.Api.Dtos.Responses;
using Psycheflow.Api.Entities;
using Psycheflow.Api.Interfaces;

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
        public UserController(
            AppDbContext context, 
            UserManager<User> manager,
            SignInManager<User> signingManager,
            ITokenService tokenService
            )
        {
            Context = context;
            UserManager = manager;
            SigningManager = signingManager;
            TokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto requestDto)
        {
            User user = new User
            {
                UserName = requestDto.Email,
                Email = requestDto.Email,
                CompanyId = (Guid)requestDto.CompanyId!
            };

            IdentityResult result = await UserManager.CreateAsync(user, requestDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(GenericResponseDto<object>.ToFail($"Erro ao criar um usuário {result.Errors.First().Description}"));
            }

            return Ok(GenericResponseDto<User>.ToSuccess("Usuário criado com sucesso",user));
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

            string token = TokenService.GenerateToken(user);

            return Ok(GenericResponseDto<object>.ToSuccess("Login realizado com sucesso", new { token }));
        }

    }
}
