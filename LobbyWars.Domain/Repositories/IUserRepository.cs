using LobbyWars.Domain.Entities;
using LobbyWars.Infrastructure.Repositories;

namespace LobbyWars.Domain.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmail(string email);
        void SetLastLogin(string email);
    }
}
