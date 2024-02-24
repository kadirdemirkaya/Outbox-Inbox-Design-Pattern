using Microsoft.EntityFrameworkCore;
using Outbox.Application.Repositories;
using Outbox.Domain.Entities.Base;
using Outbox.Persistence.Context;
using System.Linq.Expressions;

namespace Outbox.Persistence.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : BaseEntity
    {
        readonly OrderDbContext _context;
        public Repository(OrderDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table { get => _context.Set<T>(); }

        public async Task AddAsync(T model)
              => await Table.AddAsync(model);

        public async Task DeleteAsync(T model)
            => _context.Remove(model);

        public IQueryable<T> GetAll()
            => Table;

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method)
            => Table.Where(method);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
