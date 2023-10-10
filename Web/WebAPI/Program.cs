using AutoMapper;
using System;
using System.Reflection;
using WebAPI.Common.Dto;
using WebAPI.Common.Queries;
using WebAPI.Common.Utils;

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
services.AddScoped<IActivityQueries>(sp => new ActivityQueries(builder.Configuration.GetConnectionString("OslDB")));

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

app.Run();
