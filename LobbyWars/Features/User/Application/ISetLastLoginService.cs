namespace LobbyWars.Application.User.Service
{
    public interface ISetLastLoginService
    {
        Task Invoke(string email);
    }
}
