using LobbyWars.API.Features.Contract.Domain;
using MediatR;

namespace LobbyWars.API.Features.User.Application.Login
{
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
}
