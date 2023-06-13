﻿using LobbyWars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobbyWars.Domain.Events
{
    public class EvaluatedContractEvent : DomainEvent
    {
        public EvaluatedContractEvent(Contract enttity)
        {
            Contract = enttity;
        }

        public Contract Contract { get; }
    }
}
