using app_ensinai.Modules.Media.Domain.Interfaces.Repositories;
using app_ensinai.Shared.Patterns;
using FileEntity = app_ensinai.Modules.Media.Domain.Models.File;

namespace app_ensinai.Modules.Media.Domain.UseCases;

public class GetFileUseCase : IGetFileUseCase
{
    private readonly IFileRepository _fileRepository;

    public GetFileUseCase(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<Result<FileEntity>> GetByIdAsync(Guid fileId)
    {
        // Regra de negócio: Buscar o arquivo
        var fileEntity = await _fileRepository.GetFileByIdAsync(fileId);

        if (fileEntity == null)
            return Result<FileEntity>.Failure($"Arquivo com ID '{fileId}' não foi encontrado.", ErrorType.NotFound);

        return Result<FileEntity>.Success(fileEntity);
    }
}
