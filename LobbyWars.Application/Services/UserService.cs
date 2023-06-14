using LobbyWars.Application.DTOs;
using LobbyWars.Domain.Repositories;
using LobbyWars.SharedKernel;
using LobbyWars.SharedKernel.Services;

namespace LobbyWars.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly AppSettingsProvider _appSettings;
        public UserService(IUserRepository repository, AppSettingsProvider appSettings) 
        {
            _userRepository = repository;
            _appSettings = appSettings;
        }

        public async Task<LoginResponseDto> Login(string email, string password)
        {
            var result = _userRepository.GetByEmail(email: email).Result;
            string? accessToken = null;
            if(result != null)
            {
                if (result.PasswordHash == PasswordHasher.ComputeHash(password, result.PasswordSalt, _appSettings.Pepper))
                {
                    accessToken = JWToken.GenerateJwtToken(
                        result.Id.ToString(),
                        _appSettings.JWTSecret,
                        _appSettings.JWTIssuer,
                        _appSettings.JWTAudience);
                }
            }

            return new LoginResponseDto(accessToken);
        }

        public async Task SetLastLogin(string email)
        {
            _userRepository.SetLastLogin(email);
        }
    }
}