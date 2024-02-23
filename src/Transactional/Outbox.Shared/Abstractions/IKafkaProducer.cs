using Confluent.Kafka;

namespace Outbox.Shared.Abstractions
{
    public interface IKafkaProducer
    {
        Task<DeliveryResult<string, string>> ProduceAsync(string data, CancellationToken cancellationToken = default);
    }
}
