using app_ensinai.Modules.Media.Domain.Interfaces.Repositories;
using app_ensinai.Shared.Extensions;
using Dapper;
using Npgsql;
using FileEntity = app_ensinai.Modules.Media.Domain.Models.File;

namespace app_ensinai.Modules.Media.Infrastructure.Repositories;

public class FileRepository : Repository<FileEntity>, IFileRepository
{
    protected override string TableName => "media.files";
    
    public FileRepository(NpgsqlConnection connection) : base(connection)
    {
    }

    public override async Task<int> AddAsync(FileEntity entity)
    {
        var sql = $@"INSERT INTO {TableName} 
            (id, file_name, file_size, content_type, bucket, file_type, created_at, updated_at) 
            VALUES (@Id, @FileName, @FileSize, @ContentType, @Bucket, @FileType, @CreatedAt, @UpdatedAt)";
        
        var parameters = new
        {
            Id = entity.Id,
            FileName = entity.FileName,
            FileSize = entity.FileSize,
            ContentType = entity.ContentType,
            Bucket = entity.Bucket,
            FileType = (int)entity.FileType,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };

        return await _connection.ExecuteAsync(sql, parameters);
    }

    public override async Task<int> UpdateAsync(FileEntity entity)
    {
        var sql = $@"UPDATE {TableName} 
            SET file_name = @FileName, 
                file_size = @FileSize, 
                content_type = @ContentType, 
                bucket = @Bucket, 
                file_type = @FileType, 
                updated_at = @UpdatedAt 
            WHERE id = @Id";
        
        var parameters = new
        {
            Id = entity.Id,
            FileName = entity.FileName,
            FileSize = entity.FileSize,
            ContentType = entity.ContentType,
            Bucket = entity.Bucket,
            FileType = (int)entity.FileType,
            UpdatedAt = entity.UpdatedAt
        };

        return await _connection.ExecuteAsync(sql, parameters);
    }

    public async Task<FileEntity?> GetFileByIdAsync(Guid id)
    {
        var query = $@"SELECT id, file_name, file_size, content_type, bucket, file_type, created_at, updated_at 
                      FROM {TableName} 
                      WHERE id = @Id";
        var parameters = new DynamicParameters();
        parameters.Add("Id", id);
        return await _connection.QuerySingleOrDefaultAsync<FileEntity>(query, parameters);
    }
}
