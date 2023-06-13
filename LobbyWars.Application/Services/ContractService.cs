using LobbyWars.Application.DTOs.Contracts;
using LobbyWars.Application.Interfaces;
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
        public async Task<ContractResponseDto> EvaluateContracts(ContractEntity contract)
        {
            //contract.Validate();
            var winner = await contract.DetermineWinner();
            var missingSignatures = await contract.DetermineMissingSignatures();

            return new ContractResponseDto(winner: winner, missingSignatures: missingSignatures);
        }
    }
}
