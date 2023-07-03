using LobbyWars.Domain.Entities;

namespace LobbyWars.API.Features.Contract.Domain
{
    public class LoginEvent : DomainEvent
    {
        public LoginEvent(UserEntity user)
        {
            User = user;
        }

        public UserEntity User { get; }
    }
}
