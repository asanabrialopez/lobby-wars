using LobbyWars.Application.User.Service;
using LobbyWars.Domain.Events;
using MediatR;

namespace LobbyWars.API.Features.User.Application.Login
{
    public class LoginNotificationEventHandler : INotificationHandler<LoginEvent>
    {
        private readonly ILogger<LoginNotificationEventHandler> _logger;
        private readonly ISetLastLoginService _setLastLogin;

        public LoginNotificationEventHandler(ILogger<LoginNotificationEventHandler> logger, ISetLastLoginService service)
        {
            _logger = logger;
            _setLastLogin = service;
        }

        public async Task Handle(LoginEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("New notification: User logged in. {User}", notification.User);
            _setLastLogin.Invoke(notification.User.Email);

        }
    }
}
