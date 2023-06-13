using Carter;
using FluentValidation;
using LobbyWars.API;
using LobbyWars.API.Extensions;
using MediatR;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddCarter();
builder.Services.AddSwagger();
builder.Services.AddScoped();
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

var app = builder.Build();
//app.UseSwagger();
//app.MapGet("/", () => "Hello World!");
//app.UseSwaggerUI();

//Endpoints.Map(app);
//app.MapGroup("/contract")
//    .MapContractApi()
//    .WithTags("Contract");
app.MapSwagger();
app.MapCarter();
app.Run();

public partial class Program
{
    // Expose the class fro use in integration test
}