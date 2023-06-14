using Carter;
using Carter.ModelBinding;
using FluentValidation;
using LobbyWars.Application.DTOs;
using LobbyWars.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace LobbyWars.API.Features.Commands
{
    /// <summary>
    /// This class is a module that implements ICarterModule to provide routes for evaluate contracts.
    /// </summary>
    public class EvaluateContracts : ICarterModule
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
            .WithName(nameof(EvaluateContracts))
            .WithTags(nameof(Domain.Entities.Contract))
            .ProducesValidationProblem()
            .Produces<EvaluateContractResponseDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem();
        }

        /// <summary>
        /// This class represents a command for evaluating a contract.
        /// </summary>
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
            public Domain.Entities.Contract ToDomainEntity()
            {
                return new Domain.Entities.Contract(PlaintiffSignatures, DefendantSignatures);
            }
        }

        /// <summary>
        /// This class handles an EvaluateContractCommand.
        /// </summary>
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

            /// <summary>
            /// Handle an EvaluateContractCommand.
            /// </summary>
            /// <param name="request">Command for evaluate contract.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>Returns the result of evaluate contract.</returns>
            public async Task<IResult> Handle(EvaluateContractCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var result = _validator.Validate(request);
                    if (!result.IsValid)
                        return Results.ValidationProblem(result.GetValidationProblems());

                    var response = await _service.EvaluateContracts(request.ToDomainEntity());
                    return Results.Ok(response);
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Error EvaluateContract:{ex.Message}", ex);
                    throw;
                }
            }
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
