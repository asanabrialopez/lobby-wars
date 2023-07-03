using LobbyWars.SharedKernel.Kernel.Domain;

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
