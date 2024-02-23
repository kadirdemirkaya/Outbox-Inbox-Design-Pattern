using Outbox.Shared.Events;

namespace Outbox.Shared.Abstractions
{
    public interface IIntegrationEventHandler<TIntegrationEvent> : IntegrationEventHandler where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }


    public interface IntegrationEventHandler
    {

    }
}
