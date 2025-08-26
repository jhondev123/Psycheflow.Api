using Microsoft.AspNetCore.Identity;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Entities;
using Psycheflow.Api.Extensions;
using Psycheflow.Api.Seeders;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// configurações personalizadas
builder.Services.ConfigurePersistenceApp(builder.Configuration);
builder.Services.ConfigureIdentityEndpoints();
builder.Services.ConfigureDependencies();
builder.Services.ConfigureAuthentication(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



WebApplication app = builder.Build();

app.ConfigureMiddlewares();

await app.ConfigureSeeders();



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
