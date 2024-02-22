using Dapper;
using Outbox.Shared.Abstractions;
using Outbox.Shared.IntegrationEvents;
using Outbox.Shared.Repositories;
using System.Reflection.PortableExecutable;
using System.Text.Json;

namespace Outbox.Product.API.Events.EventHandlers
{
    public class OrderCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
    {
        private DapperRepository _dapperRepository;
        private readonly ILogger<OrderCreatedIntegrationEventHandler> _logger;
        private readonly IConfiguration _configuration;
        public OrderCreatedIntegrationEventHandler(ILogger<OrderCreatedIntegrationEventHandler> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
            _dapperRepository = new(_configuration["DatabaseUrl"]);
        }

        public async Task Handle(OrderCreatedIntegrationEvent @event)
        {
            int result;
            try
            {
                int count = _dapperRepository.Connection.QueryFirstOrDefault<int>(@"SELECT COUNT(*) FROM OrderInboxes WHERE Id = @Id", new { Id = @event.Id });

                string sqlQuery = "INSERT INTO OrderInboxes (Id,Type,Payload,OrderOutboxStatus, CreatedDate, IsActive) VALUES (@Id,@Type,@Payload,@OrderOutboxStatus,@CreatedDate,@IsActive)";

                if (count == 0)
                {
                    // Datas will handle in this part ! 

                    int affectedRows = _dapperRepository.Connection.Execute(sqlQuery, new { Id = @event.Id, Type = typeof(OrderCreatedIntegrationEvent).Name, Payload = JsonSerializer.Serialize(@event), OrderOutboxStatus = 3, CreatedDate = DateTime.Now, IsActive = true });

                    result = await _dapperRepository.ExecuteAsync("UPDATE OrderOutboxes SET OrderOutboxStatus=@OrderOutboxStatus WHERE Id=@Id", new { OrderOutboxStatus = 3, Id = @event.Id });

                    _logger.LogInformation("OrderOutbox process succesfully");
                }
                else
                {
                    result = await _dapperRepository.ExecuteAsync("UPDATE OrderOutboxes SET OrderOutboxStatus=@OrderOutboxStatus WHERE Id=@Id", new { OrderOutboxStatus = 4, Id = @event.Id });

                    _logger.LogWarning("OrderOutbox process not succesfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("OrderOutbox process occured a error : " + ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
