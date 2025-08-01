using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Entities;
using Psycheflow.Api.Enums;

namespace Psycheflow.Api.Seeders
{
    public class RoleSeeder : ISeeder
    {
        private AppDbContext Context { get; set; }
        private RoleManager<IdentityRole> RoleManager { get; set; }

        public bool onlyHomolog => false;

        public RoleSeeder(AppDbContext context, RoleManager<IdentityRole> roleManager) 
        {
            Context = context;
            RoleManager = roleManager;
        }
        public async Task Up()
        {
            try
            {
                await Context.Database.BeginTransactionAsync();

                List<string> roles = Enum.GetNames(typeof(Role)).ToList();

                foreach (string role in roles)
                {
                    if (!await RoleManager.RoleExistsAsync(role))
                    {
                        IdentityResult result = await RoleManager.CreateAsync(new IdentityRole(role));
                        if (!result.Succeeded)
                        {
                            throw new Exception($"Failed to create role {role}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                }



                await Context.Database.CommitTransactionAsync();
            }
            catch (Exception _)
            {
                await Context.Database.RollbackTransactionAsync();
            }
        }
        public async Task Down()
        {
            await Context.Roles.ExecuteDeleteAsync();
        }
    }
}
