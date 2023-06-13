using LobbyWars.Domain.Events;
using MediatR;

namespace LobbyWars.API.Features.EventHandler
{
    public class LoginNotificationEventHandler : INotificationHandler<LoginEvent>
    {
        private readonly ILogger<LoginNotificationEventHandler> _logger;

        public LoginNotificationEventHandler(ILogger<LoginNotificationEventHandler> logger)
        {
            _logger = logger;

        }

        public async Task Handle(LoginEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("New notification: User logged in. {User}", notification.User);
        }
    }
}
