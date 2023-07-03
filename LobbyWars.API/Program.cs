using Carter;
using FluentValidation;
using LobbyWars.API.Extensions;
using LobbyWars.Database;
using Microsoft.EntityFrameworkCore;
using Serilog;

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