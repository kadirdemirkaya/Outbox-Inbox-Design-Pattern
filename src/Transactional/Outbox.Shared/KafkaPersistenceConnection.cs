using Confluent.Kafka;
using Outbox.Shared.Configs;

namespace Outbox.Shared
{
    public class KafkaPersistenceConnection
    {
        private KafkaProducerConfig _kafkaProducerConfig;
        private KafkaConsumerConfig _kafkaConsumerConfig;
        private IProducer<int, string> _produceBuilder;
        private IConsumer<int, string> _consumerBuilder;

        public KafkaPersistenceConnection(KafkaProducerConfig kafkaProducerConfig)
        {
            _kafkaProducerConfig = kafkaProducerConfig;
            var producerConfig = new ProducerConfig()
            {
                BootstrapServers = _kafkaProducerConfig.BootstrapServers,
                ClientId = _kafkaProducerConfig.ClientId,
                Acks = Acks.All
            };
            _produceBuilder = new ProducerBuilder<int, string>(producerConfig).Build();
        }

        public KafkaPersistenceConnection(KafkaConsumerConfig kafkaConsumerConfig)
        {
            _kafkaConsumerConfig = kafkaConsumerConfig;
            var consumerConfig = new ConsumerConfig()
            {
                GroupId = _kafkaConsumerConfig.ConsumerGroupName,
                BootstrapServers = _kafkaConsumerConfig.Host,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumerBuilder = new ConsumerBuilder<int, string>(consumerConfig).Build();
        }

        public IProducer<int, string> GetProducer()
            => _produceBuilder;

        public IConsumer<int, string> GetConsumer()
            => _consumerBuilder;
    }
}
