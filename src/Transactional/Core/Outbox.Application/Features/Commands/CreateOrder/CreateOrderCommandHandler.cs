using MediatR;
using Outbox.Application.Repositories;
using Outbox.Domain.Entities;
using Outbox.Shared.Abstractions;
using Outbox.Shared.IntegrationEvents;
using System.Text.Json;

namespace Outbox.Application.Features.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderOutboxRepository _orderOutboxRepository;
        private readonly IEventBus _eventBus;

        public CreateOrderCommandHandler(IOrderOutboxRepository orderOutboxRepository, IOrderRepository orderRepository, IEventBus eventBus)
        {
            _orderOutboxRepository = orderOutboxRepository;
            _orderRepository = orderRepository;
            _eventBus = eventBus;
        }

        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var order = Order.CreateOrder(request.CreateOrderDto.Quantity, request.CreateOrderDto.Description, request.CreateOrderDto.Price);
            await _orderRepository.AddAsync(order);

            OrderCreatedIntegrationEvent orderCreateEvent = new(order.Id, order.Quantity, order.Description, order.Price);

            var orderOutbox = OrderOutbox.CreateOrderOutbox(order.Id, nameof(OrderCreatedIntegrationEvent), JsonSerializer.Serialize(orderCreateEvent), Domain.Enums.OrderOutboxStatus.Started);

            await _orderOutboxRepository.AddAsync(orderOutbox);
            await _orderOutboxRepository.SaveChangesAsync();

            _eventBus.Publish(orderCreateEvent);

            return new(true);
        }
    }
}
