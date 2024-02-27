using Microsoft.EntityFrameworkCore;
using Outbox.Application.Configs;
using Outbox.Domain.Entities;

namespace Outbox.Persistence.Context
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext()
        {
        }

        public OrderDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderInbox> OrderInboxes { get; set; }
        public DbSet<OrderOutbox> OrderOutboxes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConfig.GetDatabaseConfig());
            }
        }
    }
}
