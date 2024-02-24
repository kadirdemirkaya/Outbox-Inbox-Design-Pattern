using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Outbox.Domain.Entities;
using Outbox.Shared.Abstractions;
using System.Net.NetworkInformation;

namespace Outbox.Transaction.Log.Service.Services
{
    public class ConsumerBackgroundService : BackgroundService
    {
        private readonly IEventBus _eventBus;
        private readonly IConfiguration _configuration;

        public ConsumerBackgroundService(IEventBus eventBus, IConfiguration configuration)
        {
            _eventBus = eventBus;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = _eventBus.GetConsumer();
            await Task.Delay(1000);
            while (true)
            {
                try
                {
                    var message = consumer.Consume();
                    var body = System.Text.Json.JsonSerializer.Deserialize<OrderOutbox>(message.Value);

                    if (body is not null)
                        _eventBus.Consume(body.Type, body.Payload);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
