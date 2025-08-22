
using Bogus;
using Microsoft.AspNetCore.Identity;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Entities;

namespace Psycheflow.Api.Seeders
{
    public class HomologSeeder : ISeeder
    {
        private AppDbContext Context { get; set; }
        private UserManager<User> UserManager { get; set; }


        public bool onlyHomolog => true;

        public HomologSeeder(AppDbContext context, UserManager<User> userManager)
        {
            Context = context;
            UserManager = userManager;
        }
        public async Task Up()
        {
            try
            {
                await Context.Database.BeginTransactionAsync();

                Faker faker = new Faker("pt_BR");

                #region [ INSERINDO COMPANY ]

                Company company = new Company
                {
                    Name = "teste",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
                await Context.AddAsync(company);
                await Context.SaveChangesAsync();

                #endregion

                #region [ INSERINDO USERS ]

                string password = "Test123$";

                User admin = new User
                {
                    UserName = faker.Internet.Email(),
                    Email = faker.Internet.Email(),
                    CompanyId = company.Id
                };

                User psychologistUser = new User
                {
                    UserName = faker.Internet.Email(),
                    Email = faker.Internet.Email(),
                    CompanyId = company.Id
                };

                User patientUser = new User
                {
                    UserName = faker.Internet.Email(),
                    Email = faker.Internet.Email(),
                    CompanyId = company.Id
                };

                await CreateUser(admin, password, "Admin");
                await CreateUser(psychologistUser, password, "Psychologist");
                await CreateUser(patientUser, password, "Patient");

                #endregion

                #region [ INSERINDO PSICÓLOGO ] 
                Psychologist psychologist = new Psychologist
                {
                    UserId = psychologistUser.Id,
                    Approach = Enums.ApproachType.PSYCHOANALYSIS,
                    LicenseNumber = new Entities.ValueObjects.LicenseNumber("123"),
                    CompanyId = company.Id,
                };

                await Context.AddAsync(psychologist);
                await Context.SaveChangesAsync();

                #region [ INSERINDO HORÁRIOS QUE O PSICÓLOGO TRABALHA ]

                for (int i = 1; i < 6; i++)
                {
                    PsychologistHours psychologistHours = new PsychologistHours
                    {
                        CompanyId = company.Id,
                        PsychologistId = psychologist.Id,
                        StartTime = TimeSpan.Parse("08:00"),
                        EndTime = TimeSpan.Parse("18:00"),
                        DayOfWeek = (DayOfWeek)i,
                    };
                    await Context.AddAsync(psychologistHours);
                    await Context.SaveChangesAsync();
                }
                #endregion
                #endregion

                #region [ INSERINDO PACIENTE ] 

                Patient patient = new Patient
                {
                    CompanyId = company.Id,
                    UserId = patientUser.Id,
                };

                await Context.AddAsync(patient);
                await Context.SaveChangesAsync();

                #endregion

                #region [ INSERINDO DOCUMENTO ]
                Document document = new Document
                {
                    Name = "Teste",
                    TemplateName = "RelTesteUsuarios.frx",
                    Description = "Relatório de teste"
                };
                await Context.AddAsync(document);
                await Context.SaveChangesAsync();

                #region [ INSERINDO OS CAMPOS DO DOCUMENTO ]

                DocumentField documentField = new DocumentField
                {
                    DocumentId = document.Id,
                    Name = "email",
                    Order = 1,
                    IsRequired = true,
                };
                await Context.AddAsync(documentField);
                await Context.SaveChangesAsync();

                #endregion
                #endregion

                await Context.Database.CommitTransactionAsync();

            }
            catch (Exception ex)
            {
                await Context.Database.RollbackTransactionAsync();
            }
        }
        private async Task CreateUser(User user, string password, string role)
        {
            IdentityResult result = await UserManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            await UserManager.AddToRoleAsync(user, role);
        }
        public Task Down()
        {
            throw new NotImplementedException();
        }


    }
}
