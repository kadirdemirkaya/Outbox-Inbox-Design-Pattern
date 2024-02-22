using MediatR;
using Microsoft.AspNetCore.Mvc;
using Outbox.Application.Dtos;
using Outbox.Application.Features.Commands.CreateOrder;
using Outbox.Application.Features.Commands.ReadyDatasForCreateOrder;

namespace Outbox.Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder(CreateOrderDto createOrderDto)
        {
            CreateOrderCommandRequest request = new(createOrderDto);
            CreateOrderCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("ReadyDataForCreateOrder")]
        public async Task<IActionResult> ReadyDataForCreateOrder()
        {
            ReadyDatasForCreateOrderCommandRequest request = new();
            ReadyDatasForCreateOrderCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
