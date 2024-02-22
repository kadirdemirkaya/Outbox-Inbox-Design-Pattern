using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Outbox.Shared.Repositories
{
    public class DapperRepository
    {
        private IDbConnection _connection;

        public DapperRepository(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }
        public IDbConnection Connection
        {
            get
            {
                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();
                return _connection;
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql)
          => await _connection.QueryAsync<T>(sql);
        public async Task<int> ExecuteAsync(string sql)
            => await _connection.ExecuteAsync(sql);

        public async Task<int> ExecuteAsync(string sql, params object[] parameters)
            => await _connection.ExecuteAsync(sql, parameters);


    }
}
