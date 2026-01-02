using app_ensinai.Shared.Extensions;
using FileEntity = app_ensinai.Modules.Media.Domain.Models.File;
namespace app_ensinai.Modules.Media.Domain.Interfaces.Repositories;

public interface IFileRepository : IRepository<FileEntity>
{
    Task<IEnumerable<FileEntity>> GetFilesByUserIdAsync(Guid userId);
    Task<FileEntity?> GetFileByIdAsync(Guid id);
}
