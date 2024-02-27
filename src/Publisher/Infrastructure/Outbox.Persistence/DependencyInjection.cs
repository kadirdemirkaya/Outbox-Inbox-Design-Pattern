using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Outbox.Application.Configs;
using Outbox.Application.Repositories;
using Outbox.Persistence.Context;
using Outbox.Persistence.Repositories;

namespace Outbox.Persistence
{
    public static class DependencyInjection
    {
        public static void PersistenceServicesInjection(this IServiceCollection services)
        {
            services.AddDbContext<OrderDbContext>(options => options.UseSqlServer(GetConfig.GetDatabaseConfig()));

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderOutboxRepository, OrderOutboxRepository>();
            services.AddScoped<IOrderInboxRepository, OrderInboxRepository>();
        }
    }
}
