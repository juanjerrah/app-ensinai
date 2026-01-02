using Dapper;
using System.Data;

namespace app_ensinai.Shared.Extensions;

public abstract class Repository<T>(IDbConnection connection) : IRepository<T> where T : class
{
    private readonly IDbConnection _connection = connection;
    protected abstract string TableName { get; }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        var query = $"SELECT * FROM {TableName}";
        return await _connection.QueryAsync<T>(query);
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        var query = $"SELECT * FROM {TableName} WHERE id = @Id";
        var parameters = new DynamicParameters();
        parameters.Add("Id", id);
        return await _connection.QueryFirstOrDefaultAsync<T>(query, parameters);
    }

    public abstract Task<int> AddAsync(T entity);
    public abstract Task<int> UpdateAsync(T entity);

    public virtual async Task<int> DeleteAsync(Guid id)
    {
        var query = $"DELETE FROM {TableName} WHERE id = @Id";
        var parameters = new DynamicParameters();
        parameters.Add("Id", id);
        return await _connection.ExecuteAsync(query, parameters);
    }
}
