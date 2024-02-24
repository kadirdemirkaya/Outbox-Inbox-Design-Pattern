using Outbox.Application.Repositories;
using Outbox.Domain.Entities;
using Outbox.Persistence.Context;

namespace Outbox.Persistence.Repositories
{
    public class OrderOutboxRepository : Repository<OrderOutbox>, IOrderOutboxRepository
    {
        public OrderOutboxRepository(OrderDbContext context) : base(context)
        {
        }
    }
}
