using LobbyWars.Application.Contract.DTO;
using LobbyWars.Domain.Entities;

namespace LobbyWars.API.Features.Contract.Application
{
    public class EvaluateContractService : ServiceBase, IEvaluateContractService
    {
        /// <summary>
        /// Asynchronously evaluates a given contract and returns a DTO containing the evaluation result.
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        public async Task<EvaluateContractResponseDto> Invoke(ContractEntity contract)
        {
            var winner = await contract.DetermineWinner();
            var missingSignatures = await contract.DetermineMissingSignatures();

            return new EvaluateContractResponseDto(winner: winner, missingSignatures: missingSignatures);
        }
    }
}
