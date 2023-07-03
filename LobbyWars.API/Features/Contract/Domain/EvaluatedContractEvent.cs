using LobbyWars.SharedKernel.Kernel.Domain;

namespace LobbyWars.API.Features.Contract.Domain
{
    public class EvaluatedContractEvent : DomainEvent
    {
        public EvaluatedContractEvent(ContractEntity enttity)
        {
            Contract = enttity;
        }

        public ContractEntity Contract { get; }
    }
}
