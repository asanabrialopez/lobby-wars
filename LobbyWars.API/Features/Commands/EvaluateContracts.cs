using Carter;
using Carter.ModelBinding;
using FluentValidation;
using LobbyWars.API.Features.EventHandler;
using LobbyWars.Application.DTOs.Contracts;
using LobbyWars.Application.Interfaces;
using LobbyWars.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Contracts;

namespace LobbyWars.API.Features.Commands
{
    public class EvaluateContracts : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/contract", async (IMediator mediator, EvaluateContractCommand command) =>
            {
                return await mediator.Send(command);
            })
            .WithName(nameof(EvaluateContracts))
            .WithTags(nameof(ContractEntity))
            .ProducesValidationProblem()
            .Produces<ContractResponseDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem();
        }

        public class EvaluateContractCommand : IRequest<IResult>
        {
            /// <summary>
            /// Gets or sets the signatures of the plaintiff.
            /// </summary>
            public string PlaintiffSignatures { get; set; }

            /// <summary>
            /// Gets or sets the signatures of the defendant.
            /// </summary>
            public string DefendantSignatures { get; set; }

            /// <summary>
            /// Method to convert the DTO to a domain entity
            /// </summary>
            /// <returns></returns>
            public ContractEntity ToDomainEntity()
            {
                return new ContractEntity(PlaintiffSignatures, DefendantSignatures);
            }
        }

        public class EvaluateContractHandler : IRequestHandler<EvaluateContractCommand, IResult>
        {
            
            private readonly IValidator<EvaluateContractCommand> _validator;
            private readonly IContractService _service;
            private readonly ILogger<EvaluateContractHandler> _logger;

            public EvaluateContractHandler(IValidator<EvaluateContractCommand> validator, IContractService service, ILogger<EvaluateContractHandler> logger)
            {
                _validator = validator;
                _service = service;
                _logger = logger;
            }

            public async Task<IResult> Handle(EvaluateContractCommand request, CancellationToken cancellationToken)
            {
                var result = _validator.Validate(request);
                if (!result.IsValid)
                    return Results.ValidationProblem(result.GetValidationProblems());

                var response = await _service.EvaluateContracts(request.ToDomainEntity());
                return Results.Ok(response);
            }
        }

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
