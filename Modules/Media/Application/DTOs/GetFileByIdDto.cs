namespace app_ensinai.Modules.Media.Application.DTOs;

public class GetFileByIdDto
{
    public required Guid FileId { get; set; }
    public bool IncludeUrl { get; set; } = true;
    public int UrlExpirationMinutes { get; set; } = 60;
}
