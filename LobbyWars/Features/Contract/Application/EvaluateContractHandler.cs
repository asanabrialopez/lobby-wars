using FluentValidation;
using LobbyWars.Application.Contract.Service;
using MediatR;

namespace LobbyWars.API.Features.Contract.Application
{
    /// <summary>
    /// This class handles an EvaluateContractCommand.
    /// </summary>
    public class EvaluateContractHandler : IRequestHandler<EvaluateContractCommand, IResult>
    {

        private readonly IValidator<EvaluateContractCommand> _validator;
        private readonly IEvaluateContractService _evaluateContract;
        private readonly ILogger<EvaluateContractHandler> _logger;

        public EvaluateContractHandler(IValidator<EvaluateContractCommand> validator, IEvaluateContractService service, ILogger<EvaluateContractHandler> logger)
        {
            _validator = validator;
            _evaluateContract = service;
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

                var response = await _evaluateContract.Invoke(request.ToDomainEntity());
                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error EvaluateContract:{ex.Message}", ex);
                throw;
            }
        }
    }
}
