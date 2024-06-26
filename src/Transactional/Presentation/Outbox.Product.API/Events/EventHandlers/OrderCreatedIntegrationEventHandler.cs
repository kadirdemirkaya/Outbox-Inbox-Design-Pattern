﻿using Dapper;
using Outbox.Domain.Entities;
using Outbox.Shared.Consumer;
using Outbox.Shared.IntegrationEvents;
using Outbox.Shared.Repositories;
using System.Data;
using System.Text.Json;

namespace Outbox.Product.API.Events.EventHandlers
{
    public class OrderCreatedIntegrationEventHandler : IKafkaHandler<string, OrderOutbox>
    {
        private DapperRepository _dapperRepository;
        private readonly ILogger<OrderCreatedIntegrationEventHandler> _logger;
        private readonly IConfiguration _configuration;
        private IDbConnection _dbConnection;
        public OrderCreatedIntegrationEventHandler(ILogger<OrderCreatedIntegrationEventHandler> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
            _dapperRepository = new(_configuration["DatabaseUrl"]);
            _dbConnection = _dapperRepository.Connection;
        }

        public async Task HandleAsync(string key, OrderOutbox value)
        {
            int result;

            try
            {
                int count = _dapperRepository.Connection.QueryFirstOrDefault<int>(@"SELECT COUNT(*) FROM OrderInboxes WHERE Id = @Id", new { Id = value.Id });

                string sqlQuery = "INSERT INTO OrderInboxes (Id,Type,Payload,OrderOutboxStatus, CreatedDate, IsActive) VALUES (@Id,@Type,@Payload,@OrderOutboxStatus,@CreatedDate,@IsActive)";

                if (count == 0)
                {
                    // Datas will handle in this part ! 

                    int affectedRows = _dapperRepository.Connection.Execute(sqlQuery, new { Id = value.Id, Type = typeof(OrderCreatedIntegrationEvent).Name, Payload = JsonSerializer.Serialize(value), OrderOutboxStatus = 3, CreatedDate = DateTime.Now, IsActive = true });
                    result = await _dapperRepository.ExecuteAsync("UPDATE OrderOutboxes SET OrderOutboxStatus=@OrderOutboxStatus WHERE Id=@Id", new { OrderOutboxStatus = 3, Id = value.Id });

                    _logger.LogInformation("OrderOutbox process succesfully");
                }
                else
                {
                    result = await _dapperRepository.ExecuteAsync("UPDATE OrderOutboxes SET OrderOutboxStatus=@OrderOutboxStatus WHERE Id=@Id", new { OrderOutboxStatus = 4, Id = value.Id });

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
