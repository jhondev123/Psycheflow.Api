using Microsoft.AspNetCore.Identity;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Entities;
using Psycheflow.Api.Middlewares;
using Psycheflow.Api.Seeders;

namespace Psycheflow.Api.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static void ConfigureMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
        public async static Task ConfigureSeeders(this IApplicationBuilder app)
        {
            using (IServiceScope scope = app.ApplicationServices.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;

                AppDbContext context = services.GetRequiredService<AppDbContext>();
                RoleManager<IdentityRole> roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                UserManager<User> userManager = services.GetRequiredService<UserManager<User>>();
                IConfiguration configuration = services.GetRequiredService<IConfiguration>();

                DatabaseSeeder seeder = new DatabaseSeeder(context, roleManager, userManager);
                bool isHomolog = bool.TryParse(configuration["ENV"], out bool result) && result;
                await seeder.Seeding(isHomolog);
            }
        }
    }
}
