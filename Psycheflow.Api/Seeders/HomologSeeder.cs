
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

        public HomologSeeder(AppDbContext context,UserManager<User> userManager)
        {
            Context = context;
            UserManager = userManager;
        }
        public async Task Up()
        {
            try
            {
                await Context.Database.BeginTransactionAsync();
                #region [ INSERINDO COMPANY ]

                Company company = new Company
                {
                    Id = new Guid("266E089A-F2C2-40BA-539D-08DDCEF1516A"),
                    Name = "teste",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
                await Context.AddAsync(company);
                await Context.SaveChangesAsync();

                #endregion

                #region [ INSERINDO USER ]

                string password = "Test123$";

                User user = new User
                {
                    UserName = "Test",
                    Email = "test@gmail.com",
                    CompanyId = company.Id
                };

                IdentityResult result = await UserManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                await Context.Users.AddAsync(user);
                await Context.SaveChangesAsync();

                #endregion

                #region 

                await Context.Database.CommitTransactionAsync();
                throw new NotImplementedException();

            }
            catch (Exception ex)
            {
                await Context.Database.RollbackTransactionAsync();
            }
        }
        public Task Down()
        {
            throw new NotImplementedException();
        }


    }
}
