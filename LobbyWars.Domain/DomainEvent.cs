using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
