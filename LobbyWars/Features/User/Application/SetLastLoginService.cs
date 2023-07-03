using LobbyWars.Domain.Repositories;
using LobbyWars.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobbyWars.Application.User.Service
{
    public class SetLastLoginService : ServiceBase, ISetLastLoginService
    {
        private readonly IUserRepository _userRepository;
        public SetLastLoginService(IUserRepository repository)
        {
            _userRepository = repository;
        }

        public async Task Invoke(string email)
        {
            _userRepository.SetLastLogin(email);
        }
    }
}
