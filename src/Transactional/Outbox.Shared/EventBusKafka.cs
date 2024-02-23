using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Outbox.Shared.Configs;
using Outbox.Shared.Events;
using Polly;
using System.Net.Sockets;

namespace Outbox.Shared
{
    public class EventBusKafka : BaseEventBus
    {
        private KafkaProducerConfig _kafkaProducerConfig;
        private KafkaConsumerConfig _kafkaConsumerConfig;
        private ILogger<EventBusKafka> _logger;
        public IProducer<string, string> _produceBuilder;
        public IConsumer<string, string> _consumerBuilder;
        private KafkaPersistenceConnection _kafkaPersistenceConnection;

        public EventBusKafka(EventBusConfig eventBusConfig, IServiceProvider serviceProvider, bool? IsProducer) : base(eventBusConfig, serviceProvider)
        {
            if (eventBusConfig.Connection != null)
            {
                var connJson = JsonConvert.SerializeObject(eventBusConfig.Connection, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                _kafkaProducerConfig = JsonConvert.DeserializeObject<KafkaProducerConfig>(connJson);
            }
            else { }

            _kafkaPersistenceConnection = new(_kafkaProducerConfig);
            _produceBuilder = _kafkaPersistenceConnection.GetProducer();

            SubsMngr.OnEventRemoved += SubsManager_OnEventRemoved;
        }
        public EventBusKafka(EventBusConfig eventBusConfig, IServiceProvider serviceProvider) : base(eventBusConfig, serviceProvider)
        {
            if (eventBusConfig.Connection != null)
            {
                var connJson = JsonConvert.SerializeObject(eventBusConfig.Connection, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                _kafkaConsumerConfig = JsonConvert.DeserializeObject<KafkaConsumerConfig>(connJson);
            }
            else { }
            _kafkaPersistenceConnection = new(_kafkaConsumerConfig);
            _consumerBuilder = _kafkaPersistenceConnection.GetConsumer();

            SubsMngr.OnEventRemoved += SubsManager_OnEventRemoved;
        }

        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {
            eventName = ProcessEventName(eventName);

            _consumerBuilder.Unsubscribe();

            _consumerBuilder.Close();
        }

        public override void Publish(string serializeEvent, string type)
        {
            var policy = Policy.Handle<SocketException>()
                   .Or<ProduceException<string, string>>()
                   .WaitAndRetry(EventBusConfig.ConnectionRetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                   {
                       // log
                   });

            var eventName = type;
            eventName = ProcessEventName(eventName);

            //var body = Encoding.UTF8.GetBytes(serializeEvent);

            policy.Execute(async () =>
            {
                try
                {
                    var partition = new Partition(Math.Abs(serializeEvent.GetHashCode() % _kafkaProducerConfig.TopicPartitionsNumber));
                    await _produceBuilder.ProduceAsync(new TopicPartition(_kafkaProducerConfig.Topic, partition), new Message<string, string>
                    {
                        Key = DateTime.Now.ToShortDateString(),
                        Value = serializeEvent
                    });
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            });
        }

        public override void Publish(IntegrationEvent @event)
        {
            var policy = Policy.Handle<SocketException>()
                .Or<ProduceException<string, string>>()
                .WaitAndRetry(EventBusConfig.ConnectionRetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    // log
                });

            var eventName = @event.GetType().Name;
            eventName = ProcessEventName(eventName);

            var message = JsonConvert.SerializeObject(@event);
            //var body = Encoding.UTF8.GetBytes(message);

            policy.Execute(async () =>
            {
                try
                {
                    await _produceBuilder.ProduceAsync(_kafkaProducerConfig.Topic, new Message<string, string>
                    {
                        Key = DateTime.Now.ToShortDateString(),
                        Value = message
                    });
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            });
        }

        public override void Subscribe<T, TH>()
        {
            var eventName = typeof(T).Name;
            eventName = ProcessEventName(eventName);
            SubsMngr.AddSubscription<T, TH>();

            _consumerBuilder.Subscribe(_kafkaConsumerConfig.Topic);
        }

        public override void UnSubscribe<T, TH>()
        {
            SubsMngr.RemoveSubscription<T, TH>();

            _consumerBuilder.Unsubscribe();
        }

        public override async void Consume(string eventName, string message)
        {
            try
            {
                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                // logging
            }
        }

        public override IConsumer<string, string> GetConsumer()
            => _consumerBuilder;

        public override IProducer<string, string> GetProducer()
            => _produceBuilder;
    }
}
