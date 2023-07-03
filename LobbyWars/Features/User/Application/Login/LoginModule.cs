using Carter;
using Carter.ModelBinding;
using FluentValidation;
using LobbyWars.Application.User.DTO;
using LobbyWars.Application.User.Service;
using LobbyWars.Domain.Entities;
using LobbyWars.Domain.Events;
using MediatR;

namespace LobbyWars.API.Features.User.Application.Login
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

        /// <summary>
        /// This class represents a command for logging in a user.
        /// </summary>
        public class LoginCommand : IRequest<IResult>
        {
            /// <summary>
            /// Gets or sets the email.
            /// </summary>
            public string Email { get; set; }

            /// <summary>
            /// Gets or sets the password.
            /// </summary>
            public string Password { get; set; }

            /// <summary>
            /// Method to convert the DTO to a domain entity
            /// </summary>
            /// <returns></returns>
            public UserEntity ToDomainEntity()
            {
                return new UserEntity(Email, Password);
            }
        }

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

        /// <summary>
        /// This class validates a LoginCommand.
        /// </summary>
        public class CreateProductValidator : AbstractValidator<LoginCommand>
        {
            public CreateProductValidator()
            {
                RuleFor(r => r.Email)
                    .NotEmpty();
                RuleFor(r => r.Password)
                    .NotEmpty();
            }
        }
    }
}
