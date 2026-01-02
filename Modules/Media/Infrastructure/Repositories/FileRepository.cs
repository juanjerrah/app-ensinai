using System.Data;
using app_ensinai.Modules.Media.Domain.Interfaces.Repositories;
using app_ensinai.Shared.Extensions;
using Dapper;
using FileEntity = app_ensinai.Modules.Media.Domain.Models.File;

namespace app_ensinai.Modules.Media.Infrastructure.Repositories;

public class FileRepository : Repository<FileEntity>, IFileRepository
{
    protected override string TableName => "files";
    private readonly IDbConnection _connection;

    public FileRepository(IDbConnection connection) : base(connection)
    {
        _connection = connection;
    }


    public override Task<int> AddAsync(FileEntity entity)
    {
        var sql = $"INSERT INTO {TableName} (Name, Content, UserId) VALUES (@Name, @Content, @UserId)";
        var parameters = new
        {
            Name = entity.FileName,
            Content = entity.ContentType,
        };

        return _connection.ExecuteAsync(sql, parameters);
    }

    public override Task<int> UpdateAsync(FileEntity entity)
    {
        var sql = $"UPDATE {TableName} SET Name = @Name, Content = @Content WHERE Id = @Id";
        var parameters = new
        {
            Id = entity.Id,
            Name = entity.FileName,
            Content = entity.ContentType
        };

        return _connection.ExecuteAsync(sql, parameters);
    }

    public Task<IEnumerable<FileEntity>> GetFilesByUserIdAsync(Guid userId)
    {
        var query = $"SELECT * FROM {TableName} WHERE UserId = @UserId";
        var parameters = new DynamicParameters();
        parameters.Add("UserId", userId);
        return _connection.QueryAsync<FileEntity>(query, parameters);
    }

    public Task<FileEntity?> GetFileByIdAsync(Guid id)
    {
        var query = $"SELECT * FROM {TableName} WHERE Id = @Id";
        var parameters = new DynamicParameters();
        parameters.Add("Id", id);
        return _connection.QuerySingleOrDefaultAsync<FileEntity>(query, parameters);
    }
}
