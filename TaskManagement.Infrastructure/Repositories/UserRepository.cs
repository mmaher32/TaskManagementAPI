using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) :
    Repository<User>(context), IUserRepository
{
    public Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    => FirstOrDefaultAsync(user => user.Email == email, ct);
    public Task<bool> EmailExistsAsync(string email, CancellationToken ct)
    => AnyAsync(user => user.Email == email, ct);
    public Task<bool> UsernameExistsAsync(string username, CancellationToken ct)
    => AnyAsync(user => user.UserName == username, ct);
}
