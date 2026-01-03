using app_ensinai.Shared.Models;

namespace app_ensinai.Modules.Auth.Domain.Models
{
    public class UserInterestArea : Entity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid InterestAreaId { get; set; }
        public InterestArea InterestArea { get; set; }
    }
}