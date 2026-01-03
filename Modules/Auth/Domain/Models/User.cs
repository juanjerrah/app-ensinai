using app_ensinai.Modules.Auth.Domain.Enums;
using app_ensinai.Shared.Models;

namespace app_ensinai.Modules.Auth.Domain.Models;

public class User(string name,
    string email,
    string passwordHash,
    EProfileType profileType,
    string salt,
    string refreshToken,
    DateTime expiryTime,
    string? shortDescription = null,
    string? detailedDescription = null) : Entity
{
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public string PasswordHash { get; set; } = passwordHash;
    public EProfileType ProfileType { get; set; } = profileType;
    public string? ShortDescription { get; set; } = shortDescription;
    public string? DetailedDescription { get; set; } = detailedDescription;
    public string Salt { get; set; } = salt;
    public string RefreshToken { get; set; } = refreshToken;
    public DateTime ExpiryTime { get; set; } = expiryTime;
    public bool Active { get; set; } = true;

    public ICollection<UserFiles> UserFiles { get; set; } = new List<UserFiles>();
    public ICollection<UserInterestArea> UserInterestAreas { get; set; } = new List<UserInterestArea>();

}
