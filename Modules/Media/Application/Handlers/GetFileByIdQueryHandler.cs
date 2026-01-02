using app_ensinai.Modules.Media.Application.DTOs;
using app_ensinai.Modules.Media.Domain.Interfaces.Services;
using app_ensinai.Modules.Media.Domain.UseCases;
using app_ensinai.Shared.Patterns;

namespace app_ensinai.Modules.Media.Application.Handlers;

public class GetFileByIdQueryHandler
{
    private readonly IGetFileUseCase _getFileUseCase;
    private readonly IS3Service _s3Service;

    public GetFileByIdQueryHandler(IGetFileUseCase getFileUseCase, IS3Service s3Service)
    {
        _getFileUseCase = getFileUseCase;
        _s3Service = s3Service;
    }

    public async Task<Result<FileResponseDto>> HandleAsync(
        Guid fileId,
        bool includeUrl = false,
        int urlExpirationMinutes = 60)
    {
        if (fileId == Guid.Empty)
            return Result<FileResponseDto>.Failure("FileId é obrigatório.", ErrorType.Validation);

        var result = await _getFileUseCase.GetByIdAsync(fileId);

        if (!result.IsSuccess)
            return Result<FileResponseDto>.Failure(result.Error);

        var fileEntity = result.Value!;

        string fileUrl = string.Empty;
        if (includeUrl)
            fileUrl = _s3Service.GeneratePresignedUrl(fileEntity.FileName, urlExpirationMinutes);


        var responseDto = new FileResponseDto
        {
            Id = fileEntity.Id,
            FileName = fileEntity.FileName,
            OriginalFileName = fileEntity.FileName,
            FileSize = fileEntity.FileSize,
            ContentType = fileEntity.ContentType,
            FileUrl = fileUrl,
            CreatedAt = fileEntity.CreatedAt,
            UpdatedAt = fileEntity.UpdatedAt
        };

        return Result<FileResponseDto>.Success(responseDto);
    }
}
