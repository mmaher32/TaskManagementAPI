using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;

public sealed class TaskRepository(AppDbContext context)
    : Repository<TaskItem>(context), ITaskRepository
{
    public async Task<IReadOnlyList<TaskItem>> ListOwnedAsync(int userId, CancellationToken ct) =>
        await _dbSet.AsNoTracking().Where(task => task.UserId == userId)
            .OrderByDescending(task => task.Id).ToListAsync(ct);

    public Task<TaskItem?> FindOwnedAsync(int userId, int id, CancellationToken ct) =>
        _dbSet.FirstOrDefaultAsync(task => task.UserId == userId && task.Id == id, ct);
}
