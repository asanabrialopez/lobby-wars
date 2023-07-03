
using LobbyWars.SharedKernel.Kernel.Domain;

namespace LobbyWars.API.Features.Contract.Domain
{
    public interface IUserRepository : IGenericRepository<UserEntity>
    {
        Task<UserEntity?> GetByEmail(string email);
        void SetLastLogin(string email);
    }
}
