using LobbyWars.API.Features.Contract.Domain;
using LobbyWars.Domain.Repositories;
using LobbyWars.SharedKernel;
using LobbyWars.SharedKernel.Kernel.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobbyWars.API.Features.User.Application.SetLastLogin
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
