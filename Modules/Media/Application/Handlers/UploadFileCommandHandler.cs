using app_ensinai.Modules.Media.Application.Validators;
using app_ensinai.Modules.Media.Domain.Interfaces.Services;
using app_ensinai.Modules.Media.Domain.UseCases;
using app_ensinai.Modules.Media.Application.DTOs;
using app_ensinai.Shared.Patterns;

namespace app_ensinai.Modules.Media.Application.Handlers;

public class UploadFileCommandHandler(
    IUploadFileUseCase uploadFileUseCase,
    IS3Service s3Service,
    FileUploadValidator validator)
{
    private readonly IUploadFileUseCase _uploadFileUseCase = uploadFileUseCase;
    private readonly IS3Service _s3Service = s3Service;
    private readonly FileUploadValidator _validator = validator;

    public async Task<Result<FileResponseDto>> HandleAsync(FileUploadDto fileUploadDto)
    {
        var (IsValid, ErrorMessage) = _validator.Validate(fileUploadDto);
        if (!IsValid)
            return Result<FileResponseDto>.Failure(ErrorMessage, ErrorType.Validation);

        using var stream = fileUploadDto.File.OpenReadStream();
        
        var result = await _uploadFileUseCase.ExecuteAsync(
            stream,
            fileUploadDto.File.FileName,
            fileUploadDto.File.ContentType,
            fileUploadDto.File.Length,
            fileUploadDto.IsPrivate
        );

        if (!result.IsSuccess)
            return Result<FileResponseDto>.Failure(result.Error);

        var fileEntity = result.Value!;

        // Gerar URL para acesso ao arquivo
        var fileUrl = _s3Service.GetFileUrl(fileEntity.FileName, fileUploadDto.IsPrivate);

        var responseDto = new FileResponseDto
        {
            Id = fileEntity.Id,
            FileName = fileEntity.FileName,
            OriginalFileName = fileUploadDto.File.FileName,
            FileSize = fileEntity.FileSize,
            ContentType = fileEntity.ContentType,
            FileUrl = fileUrl,
            CreatedAt = fileEntity.CreatedAt,
            UpdatedAt = fileEntity.UpdatedAt
        };

        return Result<FileResponseDto>.Success(responseDto);
    }
}
