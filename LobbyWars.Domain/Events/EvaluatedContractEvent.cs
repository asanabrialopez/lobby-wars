using LobbyWars.Domain.Entities;

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
