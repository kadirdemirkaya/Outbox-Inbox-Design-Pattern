using Outbox.Application.Configs;
using Outbox.Domain.Entities;
using Outbox.Shared.Abstractions;
using Outbox.Shared.Repositories;
using Quartz;
using System.Data;

namespace Outbox.Job.Service.Jobs
{
    public class OrderOutboxPublishJob : IJob
    {
        private IEventBus _eventBus;
        private readonly IDbConnection _connection;
        private DapperRepository _dapperRepository;
        public OrderOutboxPublishJob(IEventBus eventBus)
        {
            _eventBus = eventBus;
            _dapperRepository = new(GetConfig.GetDatabaseConfig());
        }


        public bool _dataReaderState = true;
        public bool DataReaderState { get => _dataReaderState; }

        public void DataReaderReady() => _dataReaderState = true;
        public void DataReaderBusy() => _dataReaderState = false;

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                if (DataReaderState)
                {
                    DataReaderBusy();

                    List<OrderOutbox> orderOutboxes = (await _dapperRepository.QueryAsync<OrderOutbox>(@$"SELECT * from OrderOutboxes where OrderOutboxStatus={1}")).ToList();

                    foreach (var orderOutbox in orderOutboxes)
                    {
                        _eventBus.Publish(orderOutbox.Payload, orderOutbox.Type);

                        int result = await _dapperRepository.ExecuteAsync("UPDATE OrderOutboxes SET OrderOutboxStatus=@OrderOutboxStatus WHERE Id=@Id", new { OrderOutboxStatus = 2, Id = orderOutbox.Id });
                    }
                }
                DataReaderReady();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}