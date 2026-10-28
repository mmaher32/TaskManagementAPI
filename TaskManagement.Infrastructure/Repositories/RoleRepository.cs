using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;

public class RoleRepository(AppDbContext context) : Repository<Role>(context), IRoleRepository
{
    public Task<Role?> GetByNameAsync(string name, CancellationToken ct)
    => FirstOrDefaultAsync(role => role.Name == name, ct);
}
