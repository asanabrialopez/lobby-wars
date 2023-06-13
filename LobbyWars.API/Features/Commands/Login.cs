using Carter;
using Carter.ModelBinding;
using FluentValidation;
using LobbyWars.API.Features.EventHandler;
using LobbyWars.Application.DTOs;
using LobbyWars.Application.Services;
using LobbyWars.Domain.Entities;
using LobbyWars.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Contracts;

namespace LobbyWars.API.Features.Commands
{
    public class Login : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/user", async (IMediator mediator, LoginCommand command) =>
            {
                return await mediator.Send(command);
            })
            .WithName(nameof(Login))
            .WithTags(nameof(User))
            .ProducesValidationProblem()
            .Produces<LoginResponseDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem();
        }

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
            public User ToDomainEntity()
            {
                return new User(Email, Password);
            }
        }

        public class LoginHandler : IRequestHandler<LoginCommand, IResult>
        {
            
            private readonly IValidator<LoginCommand> _validator;
            private readonly IUserService _service;
            private readonly ILogger<LoginHandler> _logger;
            private readonly IMediator _mediator;

            public LoginHandler(IValidator<LoginCommand> validator, IUserService service, ILogger<LoginHandler> logger, IMediator mediator)
            {
                _validator = validator;
                _service = service;
                _logger = logger;
                _mediator = mediator;
            }

            public async Task<IResult> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var result = _validator.Validate(request);
                if (!result.IsValid)
                    return Results.ValidationProblem(result.GetValidationProblems());

                var response = await _service.Login(request.Email, request.Password);

                await _mediator.Publish(new LoginEvent(request.ToDomainEntity()));
                return Results.Ok(response);
            }
        }

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
