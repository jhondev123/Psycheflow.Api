using Microsoft.AspNetCore.Identity;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Dtos.Requests.User;
using Psycheflow.Api.Dtos.Responses;
using Psycheflow.Api.Entities;
using Psycheflow.Api.Enums;

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
                if (!Enum.TryParse(dto.RoleName, ignoreCase: true, out Role role))
                {
                    throw new Exception("Role inválida");
                }

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

                await SaveUserType(user, dto, role);

                await Context.SaveChangesAsync();

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
        private async Task SaveUserType(User user, RegisterUserRequestDto dto, Role role)
        {
            Psychologist? psychologist = null;
            if (role == Role.Psychologist)
            {
                if(string.IsNullOrEmpty(dto.LicenseNumber))
                {
                    throw new Exception("Para cadastrar um psicólogo é necessário o número da licensa");
                }
                psychologist = new Psychologist
                {
                    UserId = user.Id,
                    CompanyId = user.CompanyId,
                    LicenseNumber = new Entities.ValueObjects.LicenseNumber(dto.LicenseNumber)
                };
                await Context.AddAsync(psychologist);
            }

            Patient? patient = null;
            if (role == Role.Patient)
            {
                patient = new Patient
                {
                    CompanyId = user.CompanyId,
                    UserId = user.Id,
                };
                await Context.AddAsync(patient);
            }
        }
    }
}
