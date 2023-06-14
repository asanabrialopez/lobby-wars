using LobbyWars.Application.DTOs;

namespace LobbyWars.Application.Services
{
    public interface IUserService
    {
        Task<LoginResponseDto> Login(string email, string password);
        Task SetLastLogin(string email);
    }
}
