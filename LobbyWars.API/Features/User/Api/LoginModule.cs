using Carter;
using Carter.ModelBinding;
using FluentValidation;
using LobbyWars.API.Features.Contract.Domain;
using LobbyWars.API.Features.User.Application.Login;
using MediatR;

namespace LobbyWars.API.Modules
{
    /// <summary>
    /// This class is a module that implements ICarterModule to provide routes for login.
    /// </summary>
    public class LoginModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/user", async (IMediator mediator, LoginCommand command) =>
            {
                return await mediator.Send(command);
            })
            .WithName(nameof(LoginModule))
            .WithTags(nameof(UserEntity))
            .ProducesValidationProblem()
            .Produces<LoginResponseDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem();
        }
    }
}
