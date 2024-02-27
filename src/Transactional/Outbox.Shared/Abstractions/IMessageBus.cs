namespace Outbox.Shared.Abstractions
{
    public interface IMessageBus<TK, TV>
    {
        Task PublishAsync(TK key, TV message);
    }
}
