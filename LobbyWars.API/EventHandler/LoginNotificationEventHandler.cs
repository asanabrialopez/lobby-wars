using LobbyWars.Application.Services;
using LobbyWars.Domain.Events;
using MediatR;

namespace LobbyWars.API.EventHandler
{
    public class LoginNotificationEventHandler : INotificationHandler<LoginEvent>
    {
        private readonly ILogger<LoginNotificationEventHandler> _logger;
        private readonly IUserService _service;

        public LoginNotificationEventHandler(ILogger<LoginNotificationEventHandler> logger, IUserService service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task Handle(LoginEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("New notification: User logged in. {User}", notification.User);
            _service.SetLastLogin(notification.User.Email);
            
        }
    }
}
