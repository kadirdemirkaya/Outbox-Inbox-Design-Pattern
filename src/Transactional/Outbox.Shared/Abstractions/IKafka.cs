using Confluent.Kafka;

namespace Outbox.Shared.Abstractions
{
    public interface IKafka
    {
        IConsumer<int, string> GetConsumer();

        IProducer<int, string> GetProducer();
    }
}
