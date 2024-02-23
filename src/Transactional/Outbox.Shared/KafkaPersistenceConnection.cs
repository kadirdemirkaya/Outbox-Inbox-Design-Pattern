using Confluent.Kafka;
using Outbox.Shared.Configs;

namespace Outbox.Shared
{
    public class KafkaPersistenceConnection
    {
        private KafkaProducerConfig _kafkaProducerConfig;
        private KafkaConsumerConfig _kafkaConsumerConfig;
        private IProducer<string, string> _produceBuilder;
        private IConsumer<string, string> _consumerBuilder;

        public KafkaPersistenceConnection(KafkaProducerConfig kafkaProducerConfig)
        {
            _kafkaProducerConfig = kafkaProducerConfig;
            var producerConfig = new ProducerConfig()
            {
                BootstrapServers = _kafkaProducerConfig.Host,
                ClientId = _kafkaProducerConfig.ClientId,
                Acks = _kafkaProducerConfig.Acks
            };
            _produceBuilder = new ProducerBuilder<string, string>(producerConfig).Build();
        }

        public KafkaPersistenceConnection(KafkaConsumerConfig kafkaConsumerConfig)
        {
            _kafkaConsumerConfig = kafkaConsumerConfig;
            var consumerConfig = new ConsumerConfig()
            {
                GroupId = _kafkaConsumerConfig.ConsumerGroupName,
                BootstrapServers = _kafkaConsumerConfig.Host,
                AutoOffsetReset = _kafkaConsumerConfig.AutoOffsetReset
            };
            _consumerBuilder = new ConsumerBuilder<string, string>(consumerConfig).Build();
        }

        public IProducer<string, string> GetProducer()
            => _produceBuilder;

        public IConsumer<string, string> GetConsumer()
            => _consumerBuilder;
    }
}
