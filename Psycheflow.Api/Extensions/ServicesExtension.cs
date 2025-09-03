using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Entities;
using Psycheflow.Api.Interfaces;
using Psycheflow.Api.Interfaces.Services;
using Psycheflow.Api.Services;
using Psycheflow.Api.UseCases.Schedules;
using Psycheflow.Api.UseCases.Users;
using System.Text;

namespace Psycheflow.Api.Extensions
{
    public static class ServicesExtension
    {
        public static void ConfigurePersistenceApp(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("SqlServer");

            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));
        }
        public static void ConfigureIdentityEndpoints(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        }
        public static void ConfigureDependencies(this IServiceCollection services)
        {
            #region [ SERVICES ]
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordGeneratorService, PasswordGeneratorService>();
            services.AddScoped<IDocumentExporter, FastReportExporter>();
            services.AddScoped<ConfigService>();
            #endregion

            #region [ USE CASES ]
            services.AddScoped<RegisterUserUseCase>();
            services.AddScoped<CreateScheduleUseCase>();
            #endregion
        }
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            });
        }

    }
}
