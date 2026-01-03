using app_ensinai.Shared.Models;
using app_ensinai.Shared.Enums;

namespace app_ensinai.Modules.Auth.Domain.Models.Shared;

public class File : Entity
{
    public string FileName { get; set; } 
    public long FileSize { get; set; } 
    public string ContentType { get; set; } 
    public string Bucket { get; set; } 
    public EFileType FileType { get; set; } 
}

