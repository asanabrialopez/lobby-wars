using LobbyWars.Domain.Entities;
using LobbyWars.Infrastructure.Repositories;

namespace LobbyWars.API.Features.Contract.Domain
{
    public interface IUserRepository : IGenericRepository<UserEntity>
    {
        Task<UserEntity?> GetByEmail(string email);
        void SetLastLogin(string email);
    }
}
