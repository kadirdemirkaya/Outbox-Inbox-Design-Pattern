using Confluent.Kafka;

namespace Outbox.Shared.Producer
{
    public class KafkaProducerConfig<Tk, Tv> : ProducerConfig
    {
        public string Topic { get; set; }
    }
}
