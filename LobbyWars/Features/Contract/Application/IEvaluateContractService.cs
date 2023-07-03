using LobbyWars.Application.Contract.DTO;
using LobbyWars.Domain.Entities;

namespace LobbyWars.API.Features.Contract.Application
{
    public interface IEvaluateContractService
    {
        Task<EvaluateContractResponseDto> Invoke(ContractEntity contract);
    }
}
