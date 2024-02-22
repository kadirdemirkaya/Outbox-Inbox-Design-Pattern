using MediatR;
using Outbox.Application.Dtos;

namespace Outbox.Application.Features.Commands.CreateOrder
{
    public record CreateOrderCommandRequest(
        CreateOrderDto CreateOrderDto
    ) : IRequest<CreateOrderCommandResponse>;
}
