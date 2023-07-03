using LobbyWars.Application.User.DTO;

namespace LobbyWars.API.Features.User.Application.Login
{
    public interface ILoginService
    {
        Task<LoginResponseDto> Invoke(string email, string password);
    }
}
