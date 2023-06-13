using LobbyWars.Application.DTOs;
using LobbyWars.Domain.Entities;
using LobbyWars.SharedKernel.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobbyWars.Application.Services
{
    public class ContractService : IContractService
    {
        public async Task<EvaluateContractResponseDto> EvaluateContracts(Domain.Entities.Contract contract)
        {
            var winner = await contract.DetermineWinner();
            var missingSignatures = await contract.DetermineMissingSignatures();

            return new EvaluateContractResponseDto(winner: winner, missingSignatures: missingSignatures);
        }
    }
}
