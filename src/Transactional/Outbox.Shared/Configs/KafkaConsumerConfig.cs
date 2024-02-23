using Confluent.Kafka;

namespace Outbox.Shared.Configs
{
    public class KafkaConsumerConfig
    {
        public string Host { get; set; }
        public string Topic { get; set; }
        public string ConsumerGroupName { get; set; }
        public AutoOffsetReset AutoOffsetReset { get; set; } = AutoOffsetReset.Earliest;
    }
}
