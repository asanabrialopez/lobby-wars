namespace LobbyWars.API.Features.User.Application.SetLastLogin
{
    public interface ISetLastLoginService
    {
        Task Invoke(string email);
    }
}
