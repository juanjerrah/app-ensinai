namespace app_ensinai.Modules.Media.Application.DTOs;

public class FileUploadDto
{
    public IFormFile File { get; set; } = null!;
    public bool IsPrivate { get; set; } = false;
}
