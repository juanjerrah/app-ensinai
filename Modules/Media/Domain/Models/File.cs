using app_ensinai.Modules.Media.Domain.Enums;
using app_ensinai.Shared.Models;

namespace app_ensinai.Modules.Media.Domain.Models;

public class File(string fileName, long fileSize, string contentType, EFileType fileType, string bucket) : Entity
{
    public string FileName { get; set; } = fileName;
    public long FileSize { get; set; } = fileSize;
    public string ContentType { get; set; } = contentType;
    public string Bucket { get; set; } = bucket;
    public EFileType FileType { get; set; } = fileType;
}

