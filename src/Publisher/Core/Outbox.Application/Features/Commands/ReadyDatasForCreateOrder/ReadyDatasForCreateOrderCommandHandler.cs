using MediatR;
using Outbox.Application.Repositories;
using Outbox.Domain.Entities;
using Outbox.Shared.IntegrationEvents;
using System.Text.Json;

namespace Outbox.Application.Features.Commands.ReadyDatasForCreateOrder
{
    public class ReadyDatasForCreateOrderCommandHandler : IRequestHandler<ReadyDatasForCreateOrderCommandRequest, ReadyDatasForCreateOrderCommandResponse>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderOutboxRepository _orderOutboxRepository;
        private double _price;
        private int _quantity;
        public ReadyDatasForCreateOrderCommandHandler(IOrderRepository orderRepository, IOrderOutboxRepository orderOutboxRepository)
        {
            _orderRepository = orderRepository;
            _orderOutboxRepository = orderOutboxRepository;
            _price = 150;
            _quantity = 10;
        }

        public async Task<ReadyDatasForCreateOrderCommandResponse> Handle(ReadyDatasForCreateOrderCommandRequest request, CancellationToken cancellationToken)
        {
            for (int i = 0; i < 5; i++)
            {
                var order = Order.CreateOrder(_quantity + i, $"random description example : {i}", _price + i);
                await _orderRepository.AddAsync(order);

                OrderCreatedIntegrationEvent orderCreateEvent = new(order.Id, order.Quantity, order.Description, order.Price);

                var orderOutbox = OrderOutbox.CreateOrderOutbox(order.Id, nameof(OrderCreatedIntegrationEvent), JsonSerializer.Serialize(orderCreateEvent), Domain.Enums.OrderOutboxStatus.Started);

                await _orderOutboxRepository.AddAsync(orderOutbox);
                await _orderOutboxRepository.SaveChangesAsync();
            }

            return new(true);
        }
    }
}
