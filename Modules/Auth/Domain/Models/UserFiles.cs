using app_ensinai.Modules.Auth.Domain.Enums;
using app_ensinai.Shared.Models;

namespace app_ensinai.Modules.Auth.Domain.Models
{
    public class UserFiles : Entity
    {
        public EFilePurpose Purpose { get; set; }
        public User User { get; set; } = null!;
        public Guid UserId { get; set; }
        
        public Guid FileId { get; set; }
    }
}