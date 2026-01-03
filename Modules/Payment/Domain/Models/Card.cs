using app_ensinai.Modules.Payment.Domain.Enums;
using app_ensinai.Shared.Models;

namespace app_ensinai.Modules.Payment.Domain.Models;

public class Card : Entity
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public ECardBrand Brand { get; set; }
    public string LastFour { get; set; }
    public int ExpirationMonth { get; set; }
    public int ExpirationYear { get; set; }
}