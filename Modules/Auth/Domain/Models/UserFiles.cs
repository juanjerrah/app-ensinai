using app_ensinai.Modules.Auth.Domain.Enums;
using app_ensinai.Shared.Models;
using FileModel = app_ensinai.Modules.Auth.Domain.Models.Shared.File;

namespace app_ensinai.Modules.Auth.Domain.Models
{
    public class UserFiles : Entity
    {
        public EFilePurpose Purpose { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
        public FileModel File { get; set; }
        public Guid FileId { get; set; }
    }
}