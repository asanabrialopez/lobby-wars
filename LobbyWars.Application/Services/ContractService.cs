using LobbyWars.Application.DTOs;

namespace LobbyWars.Application.Services
{
    public class ContractService : IContractService
    {
        /// <summary>
        /// Asynchronously evaluates a given contract and returns a DTO containing the evaluation result.
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        public async Task<EvaluateContractResponseDto> EvaluateContracts(Domain.Entities.Contract contract)
        {
            var winner = await contract.DetermineWinner();
            var missingSignatures = await contract.DetermineMissingSignatures();

            return new EvaluateContractResponseDto(winner: winner, missingSignatures: missingSignatures);
        }
    }
}
