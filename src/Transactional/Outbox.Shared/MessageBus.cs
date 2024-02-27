using Outbox.Shared.Abstractions;
using Outbox.Shared.Producer;

namespace Outbox.Shared
{
    public class MessageBus<TK, TV> : IMessageBus<TK, TV>
    {
        public readonly KafkaProducer<TK, TV> _producer;
        public MessageBus(KafkaProducer<TK, TV> producer)
        {
            _producer = producer;
        }
        public async Task PublishAsync(TK key, TV message)
        {
            await _producer.ProduceAsync(key, message);
        }
    }
}
