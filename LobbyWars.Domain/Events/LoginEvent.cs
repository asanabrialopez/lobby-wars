using LobbyWars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
