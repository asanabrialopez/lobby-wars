using LobbyWars.API.Features.Contract.Domain;
using MediatR;

namespace LobbyWars.API.Features.Contract.Application
{
    /// <summary>
    /// This class represents a command for evaluating a contract.
    /// </summary>
    public class EvaluateContractCommand : IRequest<IResult>
    {
        /// <summary>
        /// Gets or sets the signatures of the plaintiff.
        /// </summary>
        public string PlaintiffSignatures { get; set; }

        /// <summary>
        /// Gets or sets the signatures of the defendant.
        /// </summary>
        public string DefendantSignatures { get; set; }

        /// <summary>
        /// Method to convert the DTO to a domain entity
        /// </summary>
        /// <returns></returns>
        public ContractEntity ToDomainEntity()
        {
            return new ContractEntity(PlaintiffSignatures, DefendantSignatures);
        }
    }
}
