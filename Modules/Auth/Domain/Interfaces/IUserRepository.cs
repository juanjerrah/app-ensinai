using app_ensinai.Modules.Auth.Domain.Models;
using app_ensinai.Shared.Extensions;

namespace app_ensinai.Modules.Auth.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByRefreshTokenAsync(string refreshToken);

    Task AddUserAsync(User user);
    Task UpdateUserPasswordAsync(string userId, string newPassword);
    Task UpdateUserAsync(User user);
}

