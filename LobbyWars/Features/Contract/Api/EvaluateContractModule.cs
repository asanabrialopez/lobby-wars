using Carter;
using Carter.ModelBinding;
using FluentValidation;
using LobbyWars.API.Features.Contract.Application;
using LobbyWars.Application.Contract.DTO;
using LobbyWars.Application.Contract.Service;
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
            .WithTags(nameof(Domain.Entities.ContractEntity))
            .ProducesValidationProblem()
            .Produces<EvaluateContractResponseDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem();
        }

        /// <summary>
        /// This class validates an EvaluateContractCommand.
        /// </summary>
        public class CreateProductValidator : AbstractValidator<EvaluateContractCommand>
        {
            public CreateProductValidator()
            {
                RuleFor(r => r.PlaintiffSignatures)
                    .NotEmpty()
                    .Length(3);
                RuleFor(r => r.DefendantSignatures)
                    .NotEmpty()
                    .Length(3);

                RuleFor(r => r.DefendantSignatures)
                    .NotEmpty()
                    .When(m => m.PlaintiffSignatures.Contains("#") && m.DefendantSignatures.Contains("#"))
                    .WithMessage("Only one sign can be missing");

            }
        }
    }
}
