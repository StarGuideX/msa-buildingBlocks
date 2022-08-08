using Dapr.Client;
using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EventBus
{
    public class DaprEventBus : IEventBus
    {
        private const string DAPR_PUBSUB_NAME = "pubsub";

        private readonly DaprClient _dapr;
        private readonly ILogger _logger;

        public DaprEventBus(DaprClient dapr, ILogger<DaprEventBus> logger)
        {
            _dapr = dapr;
            _logger = logger;
        }

        public async Task PublishAsync(IntegrationEvent integrationEvent)
        {
            var topicName = integrationEvent.GetType().Name;

            _logger.LogInformation(
                "发布 事件 {@Event} to {PubsubName}.{TopicName}",
                integrationEvent,
                DAPR_PUBSUB_NAME,
                topicName);

            await _dapr.PublishEventAsync(DAPR_PUBSUB_NAME, topicName, (object)integrationEvent);
        }
    }
}
