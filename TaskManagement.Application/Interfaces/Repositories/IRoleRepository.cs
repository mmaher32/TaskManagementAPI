using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetByNameAsync(string name, CancellationToken ct);
}
