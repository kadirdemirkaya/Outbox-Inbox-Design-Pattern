using MediatR;

namespace Outbox.Application.Features.Commands.ReadyDatasForCreateOrder
{
    public record ReadyDatasForCreateOrderCommandRequest (
        
    ) : IRequest<ReadyDatasForCreateOrderCommandResponse>;
}
