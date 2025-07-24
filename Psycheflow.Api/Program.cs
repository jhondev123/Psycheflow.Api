using Psycheflow.Api.Entities;
using Psycheflow.Api.Extensions;

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
