using LobbyWars.Domain.Entities;

namespace LobbyWars.Domain.Events
{
    public class LoginEvent : DomainEvent
    {
        public LoginEvent(User user)
        {
            User = user;
        }

        public User User { get; }
    }
}
