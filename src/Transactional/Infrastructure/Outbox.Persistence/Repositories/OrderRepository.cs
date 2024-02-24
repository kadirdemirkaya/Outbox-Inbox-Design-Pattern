using Outbox.Application.Repositories;
using Outbox.Domain.Entities;
using Outbox.Persistence.Context;

namespace Outbox.Persistence.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(OrderDbContext context) : base(context)
        {
        }
    }
}
