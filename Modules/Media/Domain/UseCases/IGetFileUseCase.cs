using app_ensinai.Shared.Patterns;
using FileEntity = app_ensinai.Modules.Media.Domain.Models.File;

namespace app_ensinai.Modules.Media.Domain.UseCases;

public interface IGetFileUseCase
{
    Task<Result<FileEntity>> GetByIdAsync(Guid fileId);
}
