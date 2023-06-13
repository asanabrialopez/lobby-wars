using Carter;
using FluentValidation;
using LobbyWars.API;
using LobbyWars.API.Extensions;
using LobbyWars.Infrastructure.Database;
using LobbyWars.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCarter();
builder.Services.AddSwagger();
builder.Services.AddScoped();
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

//builder.Services.Configure<AppSettingsProvider>(builder.Configuration.GetSection("ApplicationSettings"));

// Add JWT configuration
builder.Services.AddAuthentication(builder.Configuration);

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

// Other configuration stuff
builder.Services.AddOptions();
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(nameof(AppDbContext)));

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

var app = builder.Build();
app.MapSwagger();
app.MapCarter();
app.UseSerilogRequestLogging();
app.Run();
public partial class Program
{
    // Expose the class fro use in integration test
}