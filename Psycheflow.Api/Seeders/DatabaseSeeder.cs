using Microsoft.AspNetCore.Identity;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Entities;

namespace Psycheflow.Api.Seeders
{
    public class DatabaseSeeder
    {
        private AppDbContext Context { get; set; }
        private List<ISeeder> Seeders { get; set; } = new List<ISeeder>();
        private RoleManager<IdentityRole> RoleManager { get; set; }
        private UserManager<User> UserManager { get; set; }
        public DatabaseSeeder(AppDbContext context, RoleManager<IdentityRole> role, UserManager<User> userManager)
        {
            Context = context;
            RoleManager = role;
            UserManager = userManager;

            addSeedings();
        }
        private void addSeedings()
        {
            Seeders.Add(new RoleSeeder(Context, RoleManager));
            Seeders.Add(new HomologSeeder(Context, UserManager));
        }
        public async Task Seeding(bool isHomolog)
        {
            foreach (ISeeder seeder in Seeders)
            {
                if (isHomolog || !seeder.onlyHomolog)
                {
                    await seeder.Up();
                }
            }
        }
        public async Task DesSeeding()
        {
            foreach (ISeeder seeder in Seeders)
            {
                await seeder.Down();
            }
        }

    }
}
