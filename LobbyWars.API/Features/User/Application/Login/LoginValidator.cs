using FluentValidation;

namespace LobbyWars.API.Features.User.Application.Login
{
    /// <summary>
    /// This class validates a LoginCommand.
    /// </summary>
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(r => r.Email)
                .NotEmpty();
            RuleFor(r => r.Password)
                .NotEmpty();
        }
    }
}
