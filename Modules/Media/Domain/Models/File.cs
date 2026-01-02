using app_ensinai.Modules.Media.Domain.Enums;
using app_ensinai.Shared.Models;

namespace app_ensinai.Modules.Media.Domain.Models;

public class File : Entity
{
    public File(string fileName, long fileSize, string contentType, EFileType fileType, string bucket)
    {
        FileName = fileName;
        FileSize = fileSize;
        ContentType = contentType;
        Bucket = bucket;
        FileType = fileType;
    }

    public string FileName { get; set; }
    public long FileSize { get; set; }
    public string ContentType { get; set; }
    public string Bucket { get; set; }
    public EFileType FileType { get; set; }
}

