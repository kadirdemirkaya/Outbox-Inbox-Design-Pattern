using Outbox.Domain.Entities;

namespace Outbox.Application.Repositories
{
    public interface IOrderOutboxRepository : IRepository<OrderOutbox>
    {
    }
}
