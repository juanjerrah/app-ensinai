using app_ensinai.Shared.Models;

namespace app_ensinai.Modules.Auth.Domain.Models;

public class Student : Entity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
}
