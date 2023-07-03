using LobbyWars.Application;
using LobbyWars.Application.User.DTO;
using LobbyWars.Domain.Repositories;
using LobbyWars.SharedKernel;
using LobbyWars.SharedKernel.Services;

namespace LobbyWars.API.Features.User.Application.Login
{
    public class LoginService : ServiceBase, ILoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly AppSettingsProvider _appSettings;
        public LoginService(IUserRepository repository, AppSettingsProvider appSettings)
        {
            _userRepository = repository;
            _appSettings = appSettings;
        }

        public async Task<LoginResponseDto> Invoke(string email, string password)
        {
            var result = _userRepository.GetByEmail(email: email).Result;
            string? accessToken = null;
            if (result != null)
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
    }
}