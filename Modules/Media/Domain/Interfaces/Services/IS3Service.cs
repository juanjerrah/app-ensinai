namespace app_ensinai.Modules.Media.Domain.Interfaces.Services;

public interface IS3Service
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string? contentType = null, bool isPrivate = false);
    Task<bool> DeleteFileAsync(string fileName);
    Task<Stream> DownloadFileAsync(string fileName);
    Task<bool> FileExistsAsync(string fileName);
    string GeneratePresignedUrl(string fileName, int expirationMinutes = 60);
    string GetFileUrl(string fileName, bool isPrivate = false, int expirationMinutes = 60);
}