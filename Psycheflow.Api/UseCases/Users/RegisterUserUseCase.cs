using Microsoft.AspNetCore.Identity;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Dtos.Requests.User;
using Psycheflow.Api.Dtos.Responses;
using Psycheflow.Api.Entities;

namespace Psycheflow.Api.UseCases.Users
{
    public class RegisterUserUseCase
    {
        private UserManager<User> UserManager { get; set; }
        private AppDbContext Context { get; set; }
        public RegisterUserUseCase(UserManager<User> userManager,AppDbContext context)
        {
            UserManager = userManager;
            Context = context;
        }
        public async Task<GenericResponseDto<User?>> Execute(RegisterUserRequestDto dto, string password)
        {
            try
            {
                await Context.Database.BeginTransactionAsync();
                User user = new User
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    CompanyId = (Guid)dto.CompanyId!
                };

                IdentityResult result = await UserManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                await SaveUserRole(user, dto.RoleName);

                await Context.Database.CommitTransactionAsync();

                return GenericResponseDto<User?>.ToSuccess("Usuário criado com sucesso", user);
            }
            catch (Exception ex)
            {
                await Context.Database.RollbackTransactionAsync();
                return GenericResponseDto<User?>.ToFail($"Erro ao criar o usuário: {ex.Message}", null);
            }
        }

        private async Task SaveUserRole(User user, string? roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                IdentityResult result = await UserManager.AddToRoleAsync(user, roleName);
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
        }
    }
}
