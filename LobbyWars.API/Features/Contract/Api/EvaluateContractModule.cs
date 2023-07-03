using Carter;
using Carter.ModelBinding;
using FluentValidation;
using LobbyWars.API.Features.Contract.Application;
using LobbyWars.API.Features.Contract.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace LobbyWars.API.Features.Contract.Api
{
    /// <summary>
    /// This class is a module that implements ICarterModule to provide routes for evaluate contracts.
    /// </summary>
    public class EvaluateContractModule : ICarterModule
    {
        /// <summary>
        /// Method to add routes to the WebApplication instance.
        /// </summary>
        /// <param name="app">WebApplication instance.</param>
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/contract", [Authorize] async (IMediator mediator, EvaluateContractCommand command) =>
            {
                return await mediator.Send(command);
            })
            .WithName(nameof(EvaluateContractModule))
            .WithTags(nameof(ContractEntity))
            .ProducesValidationProblem()
            .Produces<EvaluateContractResponseDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem();
        }
    }
}
