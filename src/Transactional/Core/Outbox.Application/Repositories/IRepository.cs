using System.Linq.Expressions;

namespace Outbox.Application.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetWhere(Expression<Func<T, bool>> method);
        Task AddAsync(T model);
        Task DeleteAsync(T model);
        Task SaveChangesAsync();
    }
}
