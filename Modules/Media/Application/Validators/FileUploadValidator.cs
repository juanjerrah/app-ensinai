using app_ensinai.Modules.Media.Application.DTOs;

namespace app_ensinai.Modules.Media.Application.Validators;

public class FileUploadValidator
{
    private const long MaxFileSize = 100 * 1024 * 1024; // 100 MB
    private static readonly string[] AllowedContentTypes = 
    [
        "image/jpeg", "image/png", "image/gif", "image/webp",
        "application/pdf",
        "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        "text/plain", "text/csv"
    ];

    public (bool IsValid, string ErrorMessage) Validate(FileUploadDto fileUpload)
    {
        if (fileUpload.File == null || fileUpload.File.Length == 0)
            return (false, "Nenhum arquivo foi enviado.");

        if (fileUpload.File.Length > MaxFileSize)
            return (false, $"O arquivo excede o tamanho máximo permitido de {MaxFileSize / (1024 * 1024)} MB.");

        if (string.IsNullOrWhiteSpace(fileUpload.File.ContentType) || 
            !AllowedContentTypes.Contains(fileUpload.File.ContentType.ToLower()))
            return (false, $"Tipo de arquivo não permitido: {fileUpload.File.ContentType}");

        return (true, string.Empty);
    }
}
