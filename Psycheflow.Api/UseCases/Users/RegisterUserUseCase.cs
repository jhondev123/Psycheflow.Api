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
        public RegisterUserUseCase(UserManager<User> userManager, AppDbContext context)
        {
            UserManager = userManager;
            Context = context;
        }
        public async Task<GenericResponseDto<object?>> Execute(RegisterUserRequestDto dto, string password)
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

                Psychologist? psychologist = null;
                if (role == Role.Psychologist)
                {
                    psychologist = await SavePsychologist(user, dto);
                }
                Patient? patient = null;
                if (role == Role.Patient)
                {
                    patient = await SavePatient(user);
                }

                await Context.SaveChangesAsync();
                await Context.Database.CommitTransactionAsync();

                return GenericResponseDto<object?>.ToSuccess("Usuário criado com sucesso", new
                {
                    user,
                    patient = patient,
                    psychologist = psychologist
                });
            }
            catch (Exception ex)
            {
                await Context.Database.RollbackTransactionAsync();
                return GenericResponseDto<object?>.ToFail($"Erro ao criar o usuário: {ex.Message}", null);
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
        private async Task<Psychologist> SavePsychologist(User user, RegisterUserRequestDto dto)
        {
            if (string.IsNullOrEmpty(dto.LicenseNumber))
            {
                throw new Exception("Para cadastrar um psicólogo é necessário o número da licensa");
            }
            Psychologist psychologist = new Psychologist
            {
                UserId = user.Id,
                CompanyId = user.CompanyId,
                LicenseNumber = new Entities.ValueObjects.LicenseNumber(dto.LicenseNumber)
            };
            await Context.AddAsync(psychologist);
            return psychologist;
        }
        private async Task<Patient> SavePatient(User user)
        {
            Patient patient = new Patient
            {
                CompanyId = user.CompanyId,
                UserId = user.Id,
            };
            await Context.AddAsync(patient);
            return patient;
        }
    }
}
