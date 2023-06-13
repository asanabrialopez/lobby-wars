using LobbyWars.Domain.Events;
using MediatR;

namespace LobbyWars.API.Features.EventHandler
{
    public class EvaluateContractsNotificationEventHandler : INotificationHandler<EvaluatedContractEvent>
    {
        private readonly ILogger<EvaluateContractsNotificationEventHandler> _logger;

        public EvaluateContractsNotificationEventHandler(ILogger<EvaluateContractsNotificationEventHandler> logger)
        {
            _logger = logger;

        }

        public async Task Handle(EvaluatedContractEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Nueva notificación: Evaluación de contrato {Contract}", notification.Contract);
        }
    }
}
