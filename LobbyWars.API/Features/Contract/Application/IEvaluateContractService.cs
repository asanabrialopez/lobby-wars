using LobbyWars.API.Features.Contract.Domain;

namespace LobbyWars.API.Features.Contract.Application
{
    public interface IEvaluateContractService
    {
        Task<EvaluateContractResponseDto> Invoke(ContractEntity contract);
    }
}
