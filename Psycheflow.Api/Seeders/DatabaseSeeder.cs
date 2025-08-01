using Microsoft.AspNetCore.Identity;
using Psycheflow.Api.Contexts;

namespace Psycheflow.Api.Seeders
{
    public class DatabaseSeeder
    {
        private AppDbContext Context { get; set; }
        private List<ISeeder> Seeders { get; set; } = new List<ISeeder>();
        private RoleManager<IdentityRole> RoleManager { get; set; }
        public DatabaseSeeder (AppDbContext context, RoleManager<IdentityRole> role)
        {
            Context = context;
            RoleManager = role;

            addSeedings();
        }
        private void addSeedings()
        {
            Seeders.Add(new RoleSeeder (Context, RoleManager));
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
