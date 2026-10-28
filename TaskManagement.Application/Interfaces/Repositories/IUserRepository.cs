using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    Task<bool> EmailExistsAsync(string email, CancellationToken ct);
    Task<bool> UsernameExistsAsync(string username, CancellationToken ct);

    //Task<User?> GetUserWithRoleAsync(int userId);
    //Task<User?> GetUserWithRefreshTokensAsync(string refreshToken);
}
