using Confluent.Kafka;

namespace Outbox.Shared.Configs
{
    public class KafkaConfig
    {
        public string? Host { get; set; }
        public string? Topic { get; set; }
        public string? ConsumerGroupName { get; set; }
        public AutoOffsetReset AutoOffsetReset { get; set; } = AutoOffsetReset.Earliest;
        public int? TopicPartitionsNumber { get; set; } = 50;
        public string? BootstrapServers { get; set; }
        public string? ClientId { get; set; }
        public Acks Acks { get; set; } = Acks.All;
    }
}
