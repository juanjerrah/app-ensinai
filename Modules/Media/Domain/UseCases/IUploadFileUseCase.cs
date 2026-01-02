using app_ensinai.Modules.Media.Domain.Enums;
using app_ensinai.Shared.Patterns;
using FileEntity = app_ensinai.Modules.Media.Domain.Models.File;

namespace app_ensinai.Modules.Media.Domain.UseCases;

public interface IUploadFileUseCase
{
    Task<Result<FileEntity>> ExecuteAsync(Stream fileStream, string fileName, string contentType, long fileSize, EFileType fileType, bool isPrivate = false);
}
