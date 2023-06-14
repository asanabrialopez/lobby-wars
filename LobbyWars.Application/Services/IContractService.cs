using LobbyWars.Application.DTOs;
using LobbyWars.Domain.Entities;

namespace LobbyWars.Application.Services
{
    public interface IContractService
    {
        Task<EvaluateContractResponseDto> EvaluateContracts(Contract contract);
    }
}
