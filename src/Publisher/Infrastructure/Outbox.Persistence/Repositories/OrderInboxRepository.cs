using Outbox.Application.Repositories;
using Outbox.Domain.Entities;
using Outbox.Persistence.Context;

namespace Outbox.Persistence.Repositories
{
    public class OrderInboxRepository : Repository<OrderInbox>, IOrderInboxRepository
    {
        public OrderInboxRepository(OrderDbContext context) : base(context)
        {
        }
    }
}
