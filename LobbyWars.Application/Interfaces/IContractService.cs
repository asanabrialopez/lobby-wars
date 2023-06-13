using LobbyWars.Application.DTOs.Contracts;
using LobbyWars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobbyWars.Application.Interfaces
{
    public interface IContractService
    {
        Task<ContractResponseDto> EvaluateContracts(ContractEntity contract);
    }
}
