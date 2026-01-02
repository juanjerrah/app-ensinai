using app_ensinai.Modules.Media.Domain.Interfaces.Repositories;
using app_ensinai.Modules.Media.Domain.Interfaces.Services;
using app_ensinai.Shared.Patterns;
using FileEntity = app_ensinai.Modules.Media.Domain.Models.File;

namespace app_ensinai.Modules.Media.Domain.UseCases;

public class UploadFileUseCase : IUploadFileUseCase
{
    private readonly IFileRepository _fileRepository;
    private readonly IS3Service _s3Service;

    public UploadFileUseCase(IFileRepository fileRepository, IS3Service s3Service)
    {
        _fileRepository = fileRepository;
        _s3Service = s3Service;
    }

    public async Task<Result<FileEntity>> ExecuteAsync(
        Stream fileStream, 
        string fileName, 
        string contentType, 
        long fileSize, 
        bool isPrivate = false)
    {
        // Regra de negócio: Sanitizar nome do arquivo
        var sanitizedFileName = SanitizeFileName(fileName);
        
        // Regra de negócio: Gerar nome único para evitar conflitos
        var uniqueFileName = $"{Guid.NewGuid()}_{sanitizedFileName}";

        try
        {
            // Upload para S3
            var fileUrl = await _s3Service.UploadFileAsync(fileStream, uniqueFileName, contentType, isPrivate);

            // Criar entidade de arquivo
            var fileEntity = new FileEntity(uniqueFileName, fileSize, contentType, "");

            // Persistir no banco de dados
            await _fileRepository.AddAsync(fileEntity);

            return Result<FileEntity>.Success(fileEntity);
        }
        catch (Exception ex)
        {
            return Result<FileEntity>.Failure($"Erro ao fazer upload do arquivo: {ex.Message}", ErrorType.Failure);
        }
    }

    private static string SanitizeFileName(string fileName)
    {
        // Remove caracteres inválidos
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
        
        // Limita o tamanho do nome
        if (sanitized.Length > 200)
            sanitized = sanitized[..200];

        return sanitized;
    }
}
