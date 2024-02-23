using Microsoft.Extensions.Hosting;
using Outbox.Shared.Abstractions;

namespace Outbox.Transaction.Log.Service.Services
{
    public class ConsumerBackgroundService : BackgroundService
    {
        private readonly IEventBus _eventBus;

        public ConsumerBackgroundService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumer = _eventBus.GetConsumer();
                    var message = consumer.Consume();
                    //var body = System.Text.Json.JsonSerializer.Deserialize<OrderOutbox>(message.Value);

                    //_eventBus.Consume(body., body.);

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
