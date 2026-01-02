namespace app_ensinai.Modules.Media.Application.DTOs;

public class FileSearchDto
{
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
