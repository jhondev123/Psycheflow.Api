using Microsoft.AspNetCore.Identity;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Entities;
using Psycheflow.Api.Extensions;
using Psycheflow.Api.Seeders;

var builder = WebApplication.CreateBuilder(args);

// configurações personalizadas
builder.Services.ConfigurePersistenceApp(builder.Configuration);
builder.Services.ConfigureIdentityEndpoints();
builder.Services.ConfigureDependencies();
builder.Services.ConfigureAuthentication(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// seeding
using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;

    AppDbContext context = services.GetRequiredService<AppDbContext>();
    RoleManager<IdentityRole> roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    DatabaseSeeder seeder = new DatabaseSeeder(context, roleManager);
    await seeder.Seeding();
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
