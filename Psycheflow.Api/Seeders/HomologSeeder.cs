
using Bogus;
using Microsoft.AspNetCore.Identity;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Entities;
using Psycheflow.Api.Entities.Configs;

namespace Psycheflow.Api.Seeders
{
    public class HomologSeeder : ISeeder
    {
        private AppDbContext Context { get; set; }
        private UserManager<User> UserManager { get; set; }

        private int Quantity = 1;
        public bool onlyHomolog => true;

        public HomologSeeder(AppDbContext context, UserManager<User> userManager,int quantity = 1)
        {
            Context = context;
            UserManager = userManager;
            Quantity = quantity;
        }
        public async Task Up()
        {
            try
            {
                await Context.Database.BeginTransactionAsync();

                Faker faker = new Faker("pt_BR");

                for (int i = 0; i < Quantity; i++)
                {
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

                    for (int j = 1; j < 6; j++)
                    {
                        PsychologistHours psychologistHours = new PsychologistHours
                        {
                            CompanyId = company.Id,
                            PsychologistId = psychologist.Id,
                            StartTime = TimeSpan.Parse("08:00"),
                            EndTime = TimeSpan.Parse("18:00"),
                            DayOfWeek = (DayOfWeek)j,
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

                    #region [ INSERINDO O AGENDAMENTO ]

                    Schedule schedule = new Schedule
                    {
                        CompanyId = company.Id,
                        Date = DateTime.UtcNow,
                        PsychologistId = psychologist.Id,
                        ScheduleStatus = Enums.ScheduleStatus.Pending,
                        ScheduleTypes = Enums.ScheduleTypes.SESSION,
                        Start = TimeSpan.FromHours(10),
                        End = TimeSpan.FromHours(11)
                    };
                    await Context.AddAsync(schedule);
                    await Context.SaveChangesAsync();

                    #endregion

                    #region [ INSERINDO A SESSÃO ]

                    Session session = new Session
                    {
                        CompanyId = company.Id,
                        Description = "descrição da sessão",
                        Feedback = "Ok",
                        PatientId = patient.Id,
                        PsychologistId = psychologist.Id,
                        ScheduleId = schedule.Id,
                        SessionStatus = Enums.SessionStatus.Scheduled,
                    };
                    await Context.AddAsync(session);
                    await Context.SaveChangesAsync();

                    #endregion

                    #region [ INSERINDO CONFIG ]
                    Config config = new Config
                    {
                        CompanyId = company.Id,
                        Key = ConfigKey.EnableAI.Value,
                        Description = "Configuração para habilitar e configurar IA",
                        Value = null
                    };
                    await Context.AddAsync(config);
                    await Context.SaveChangesAsync();

                    #region [ INSERINDO CONFIG AI ]

                    ConfigAi configAi = new ConfigAi
                    {
                        ConfigId = config.Id,
                        IsEnabled = true,
                        MaxTokens = 1000,
                        Provider = "provider",
                        Temperature = 0,                        
                    };
                    await Context.AddAsync(configAi);
                    await Context.SaveChangesAsync();

                    #endregion

                    #endregion
                }

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
