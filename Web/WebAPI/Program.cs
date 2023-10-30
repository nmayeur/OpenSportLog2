using System.Reflection;
using WebAPI.Common.Infrastructure.DataSeed;
using WebAPI.Common.Infrastructure.Log;
using WebAPI.Common.Queries;
using WebAPI.Common.Utils;
using WebAPI.Dto;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("*");
        });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var services = builder.Services;

// Dependency injection
services.AddScoped<IActivityQueries>(sp => new ActivityQueries(builder.Configuration.GetConnectionString("OslDB") ?? ""));
services.AddScoped<ITrackPointQueries>(sp => new TrackPointQueries(builder.Configuration.GetConnectionString("OslDB") ?? ""));
services.AddSingleton<ILoggerService>(sp => new NLoggerService("WebApi"));

// Mapping DTOs
services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>(), Assembly.GetAssembly(typeof(AthleteDto)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();
app.MapControllers();

if (builder.Configuration.GetValue<bool>("IsTest"))
{
    await new DataSeed(builder.Configuration.GetConnectionString("OslDB") ?? "",
        "",
        app.Services.GetService<ILoggerService>()).SeedAsync();
}
else
{
    await new DataSeed(builder.Configuration.GetConnectionString("OslDB") ?? "",
        "",
        app.Services.GetService<ILoggerService>()).SyncStructureAsync();
}
app.Run();
