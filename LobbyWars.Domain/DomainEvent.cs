using MediatR;

namespace LobbyWars.Domain
{
    /// <summary>
    /// Base event
    /// </summary>
    public abstract class DomainEvent : INotification
    {
        protected DomainEvent()
        {
            DateOccurred = DateTimeOffset.UtcNow;
        }
        public DateTimeOffset DateOccurred { get; protected set; } = DateTime.UtcNow;
    }
}
