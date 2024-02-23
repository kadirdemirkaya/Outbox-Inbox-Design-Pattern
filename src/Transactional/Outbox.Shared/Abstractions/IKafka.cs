using Confluent.Kafka;

namespace Outbox.Shared.Abstractions
{
    public interface IKafka
    {
        IConsumer<string, string> GetConsumer();

        IProducer<string, string> GetProducer();
    }
}
