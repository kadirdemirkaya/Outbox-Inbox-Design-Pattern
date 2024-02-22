using Dapper;
using System.Linq.Expressions;

namespace Outbox.Shared.Abstractions
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> FindAllAsync();
        Task<T> FindByIdAsync(int id);
        Task<bool> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);

        Task<List<T>> GetFilterAll(Expression<Func<T, bool>> filter);
        Task<T> GetFilter(Expression<Func<T, bool>> filter);
        Task<List<T>> GetQueryAll(string query);
        Task<int> GetStoredProcedure(string storedProcedure, DynamicParameters dynamicParameters);
    }
}
