using app_ensinai.Modules.Media.Domain.Interfaces.Repositories;
using app_ensinai.Modules.Media.Domain.Interfaces.Services;
using app_ensinai.Shared.Patterns;

namespace app_ensinai.Modules.Media.Domain.UseCases;

public class DeleteFileUseCase : IDeleteFileUseCase
{
    private readonly IFileRepository _fileRepository;
    private readonly IS3Service _s3Service;

    public DeleteFileUseCase(IFileRepository fileRepository, IS3Service s3Service)
    {
        _fileRepository = fileRepository;
        _s3Service = s3Service;
    }

    public async Task<Result> ExecuteAsync(Guid fileId)
    {
        // Regra de negócio: Verificar se o arquivo existe
        var fileEntity = await _fileRepository.GetFileByIdAsync(fileId);
        
        if (fileEntity == null)
            return Result.Failure($"Arquivo com ID '{fileId}' não foi encontrado.", ErrorType.NotFound);

        try
        {
            // Regra de negócio: Deletar do S3 primeiro
            await _s3Service.DeleteFileAsync(fileEntity.FileName);

            // Deletar do banco de dados
            await _fileRepository.DeleteAsync(fileEntity.Id);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Erro ao deletar o arquivo: {ex.Message}", ErrorType.Failure);
        }
    }
}
