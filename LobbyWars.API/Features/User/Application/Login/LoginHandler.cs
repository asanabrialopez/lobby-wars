using Carter.ModelBinding;
using FluentValidation;
using LobbyWars.API.Features.Contract.Domain;
using MediatR;

namespace LobbyWars.API.Features.User.Application.Login
{
    /// <summary>
    /// This class handles a LoginCommand.
    /// </summary>
    public class LoginHandler : IRequestHandler<LoginCommand, IResult>
    {

        private readonly IValidator<LoginCommand> _validator;
        private readonly ILoginService _login;
        private readonly ILogger<LoginHandler> _logger;
        private readonly IMediator _mediator;

        public LoginHandler(IValidator<LoginCommand> validator, ILoginService service, ILogger<LoginHandler> logger, IMediator mediator)
        {
            _validator = validator;
            _login = service;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = _validator.Validate(request);
                if (!result.IsValid)
                    return Results.ValidationProblem(result.GetValidationProblems());

                var response = await _login.Invoke(request.Email, request.Password);

                await _mediator.Publish(new LoginEvent(request.ToDomainEntity()));
                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Login:{ex.Message}", ex);
                throw;
            }

        }
    }
}
