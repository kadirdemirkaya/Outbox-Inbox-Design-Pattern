using Confluent.Kafka;

namespace Outbox.Shared.Configs
{
    public class KafkaProducerConfig
    {
        public int TopicPartitionsNumber { get; set; } = 50;
        public string BootstrapServers { get; set; }
        public string ClientId { get; set; }
        public string Topic { get; set; }
        public string Host { get; set; }
        public Acks Acks { get; set; } = Acks.All;
    }
}
